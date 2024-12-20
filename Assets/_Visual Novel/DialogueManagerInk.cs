using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.IO;


public class DialogueManagerInk : MonoBehaviour
{
    // ink
    Story historiaAtual;
    [SerializeField] TextAsset inkJSON;
    bool escolhasAtivas = false;
    [SerializeField] Animator animatorSprite_Joe;
    [SerializeField] Animator animatorSprite_Luna;

    // efeito maquina de escrever
    string textoAtual = "";
    bool efeitoMaquina = false;
    int indiceChar; // indice do char do texto atual
    [Header("Efeito TypeWritter")]
    float timerTypeWritter;
    [SerializeField] float delayTypeWritter = 0.08f;

    [Header("Escolhas UI")]
    public GameObject[] escolhas;
    TMP_Text[] textoEscolhas;

    [Header("Dialogo UI")]
    [SerializeField] TMP_Text nomeExibicao;
    // public Image spriteExibicaoA, spriteExibicaoB; // onde deve passar as animacoes dos personagem
    public TMP_Text dialogo;
    public TMP_Text avisoQuest;
    public TMP_Text avisoNome;

    const string speakerTAG = "speaker"; // quem esta falando no momento 
    const string placeTAG = "place", questTAG = "quest", correrTAG = "correrL";

    [Header("Sprite")]
    [SerializeField] SkinsManager skinsManager;

    const string portraitTAG_LunaA = "portraitLunaA";
    const string portraitTAG_LunaB = "portraitLunaB";

    #region Se eu quiser que o arquivo ink determine os sprites 
    /* const string portraitTAG_JoeB = "portraitJoeA";
        const string portraitTAG_JoeA = "portraitJoeB";"; */
    #endregion

    // const string portraitTAG_Luna = "portraitLuna";
    const string portraitTAG_Joe = "portraitJoe"; // se portraitJoe:normal fica normal, se for portrait:sorrindo ele sorri

    string spriteValor_Falar = ""; // guardando o valor da sprite para dar a ideia de fala
    string spriteValor_NaoFalar = "";
    bool mudarSprite = false;
    float timerSprite;
    [SerializeField] float delaySprite = 0.8f;

    bool spriteA = false; // spriteA � sempre a sem falar
    bool isLuna = false;
    bool isJoe = false;

    [Header("Audio")]
    [SerializeField] AudioClip[] audioClip_dialogo;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool pararSource = false;
    bool narradorSom = false;

    [Range(1, 5)][SerializeField] int frequenciaAudio = 2;
    float minPitch = 0.65f;
    float maxPitch = 2;

    [Header("Background")]
    [SerializeField] Animator animatorPlace;
    [SerializeField] Image fadeImage;
    float fadeDuration = 1f; 

    // vai randomizar uma primeira vez o valor (tom e clipe) de uma letra, exemplo "a"
    // e o reutilizar sempre o mesmo valor deixando parecido com uma alfabeto
    [SerializeField] bool deixarPrevisivel;

    [Header("Quests")]
    string questKey = "";
    public bool terminarQuest = false;

    private void Awake()
    {
        audioSource = GameObject.Find("AudioSourceSFX").GetComponent<AudioSource>();
        if (audioSource == null) { Debug.Log("nao consegui achar audioSource"); }

        skinsManager = GameObject.Find("InterfaceManager").GetComponent<SkinsManager>();
        if (skinsManager == null) { Debug.Log("nao consegui achar skinsManager"); }
    }

    private void Start()
    {
        dialogo.text = "";
        CarregarAoIniciar();
        ContinuarDialogo();

        textoEscolhas = new TMP_Text[escolhas.Length];
        int i = 0;
        foreach (GameObject escolha in escolhas)
        {
            textoEscolhas[i] = escolha.GetComponentInChildren<TMP_Text>();
            i++;
        }

        escolhasAtivas = false;
    }

    private void Update()
    {
        if (terminarQuest == true || GameManager.Instance.partidasQuest == 3)
        {
            GameManager.Instance.questAtiva = false;
            GameManager.Instance.partidasQuest = 0;
            if (GameManager.Instance.lunaCorrendo == true)
            {
                GameManager.Instance.lunaCorrendo = false;
            }
        }

        #region TypeWritter
        // efeito maquina de escrever
        if (efeitoMaquina == true && indiceChar < textoAtual.Length)
        {
            timerTypeWritter = timerTypeWritter + Time.deltaTime;
            if (timerTypeWritter >= delayTypeWritter)
            {
                // adicionando o proximo caractere
                dialogo.text = dialogo.text + textoAtual[indiceChar];

                // reproduzindo som do narrador
                if (narradorSom == true)
                {
                    PlaySom(indiceChar, dialogo.text[indiceChar]);
                }

                else if (spriteA == false)
                {
                    PlaySom(indiceChar, dialogo.text[indiceChar]);
                }

                // avancando para o proximo caractere 
                indiceChar++;
                timerTypeWritter = 0;
            }

        }
        else
        {
            // para nao parar com eles de boca aberta
            if (isLuna == true)
            {
                animatorSprite_Luna.Play(spriteValor_NaoFalar);
            }
            else if (isJoe == true)
            {
                animatorSprite_Joe.Play(spriteValor_NaoFalar);
            }

            mudarSprite = false;
        }
        #endregion

        MudarSprite();

        if (GameManager.Instance.questAtiva == true && GameManager.Instance.partidasQuest == 0)
        {
            escolhas[0].SetActive(false);
            escolhas[1].SetActive(false);
        }
    }

    void MudarSprite()
    {
        if (mudarSprite == true)
        {
            timerSprite = timerSprite + Time.deltaTime;
            if (timerSprite >= delaySprite)
            {
                timerSprite = 0;
                spriteA = !spriteA;
            }

            if (spriteA == false) // spriteA = false � a de falar
            {
                if (isLuna == true)
                {
                    animatorSprite_Luna.Play(spriteValor_Falar);

                }
                else if (isJoe == true)
                {
                    animatorSprite_Joe.Play(spriteValor_Falar);
                }
            }
            else
            {
                if (isLuna == true)
                {
                    animatorSprite_Luna.Play(spriteValor_NaoFalar);
                }
                else if (isJoe == true)
                {
                    animatorSprite_Joe.Play(spriteValor_NaoFalar);
                }
            }
        }
    }
    void PlaySom(int numeroChar, char caractereAtual)
    {
        if (numeroChar % frequenciaAudio == 0) // a cada tres caracteres faz um audio
        {
            if (audioSource == true)
            {
                audioSource.Stop();
            }

            AudioClip clipAtual = null;
            if (deixarPrevisivel == true)
            {
                int hashCode = caractereAtual.GetHashCode();

                // escolhendo o clip para o "x" caractere
                int previsivelIndex = hashCode % audioClip_dialogo.Length;
                clipAtual = audioClip_dialogo[previsivelIndex];

                // escolhendo o pitch para o "x" caractere
                int minPitch_INT = (int)(minPitch * 100);
                int maxPitch_INT = (int)(maxPitch * 100);
                int pitchRange_INT = maxPitch_INT - minPitch_INT;

                // verificando porque se for 0, vai dar erro :P
                // basicamente, o minPitch_INT e maxPitch_INT nao podem ser os mesmos
                if (pitchRange_INT != 0)
                {
                    int previsivelPitch_INT = (hashCode % pitchRange_INT) + minPitch_INT;
                    float previsivelPitch = previsivelPitch_INT / 100f;
                    audioSource.pitch = previsivelPitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }

                audioSource.PlayOneShot(clipAtual);
            }
            else
            {
                // mudando o tom do audio
                float randomPitch = UnityEngine.Random.Range(minPitch, maxPitch);
                audioSource.pitch = randomPitch;

                // randomizando
                int randomClip = UnityEngine.Random.Range(0, audioClip_dialogo.Length);
                audioSource.PlayOneShot(audioClip_dialogo[randomClip]);
            }
        }
    }

    /*  public void IniciarDialogo()
      {
          historiaAtual = new Story(inkJSON.text);

          ContinuarDialogo();
      }*/

    public void IniciarDialogo(TextAsset _inkJSON)
    {
        historiaAtual = new Story(_inkJSON.text);
        dialogo.text = "";
        ContinuarDialogo();
    }

    public void ContinuarDialogo()
    {
        if (GameManager.Instance.questAtiva == true)
        {
            Debug.Log("quest A atIVA");
            avisoQuest.gameObject.SetActive(true);
            avisoNome.gameObject.SetActive(true);
            dialogo.gameObject.SetActive(false);
            nomeExibicao.gameObject.SetActive(false);

            avisoQuest.text = "Voc� deve jogar 3 partidas para desbloquear o restante da hist�ria";

            animatorSprite_Joe.gameObject.SetActive(false);
            animatorSprite_Luna.gameObject.SetActive(false);
        }
        else
        {
            avisoQuest.gameObject.SetActive(false);
            dialogo.gameObject.SetActive(true);
            avisoNome.gameObject.SetActive(false);
            nomeExibicao.gameObject.SetActive(true);
        }

        if (dialogo.text.Length < textoAtual.Length && GameManager.Instance.questAtiva == false)
        {
            // mostrar o texto completo da fala atual se ele ainda nao estiver completo
            dialogo.text = textoAtual;
            efeitoMaquina = false;
        }
        else if (historiaAtual.canContinue && GameManager.Instance.questAtiva == false)
        {
            // limpando o texto e o estado atual antes de exibir a nova fala
            dialogo.text = "";
            indiceChar = 0;
            timerTypeWritter = 0;

            // se o texto ja estiver completo, avan�ar para a proxima fala
            textoAtual = historiaAtual.Continue(); // dialogo.text = historiaAtual.Continue();
            MostrarEscolhas();
            ManipularTag(historiaAtual.currentTags); // sprite
            efeitoMaquina = true;
        }
        /* else
         {
             efeitoMaquina = false;
             if (!escolhasAtivas)
                 dialogo.text = "";

         }*/

        SalvarHistoria();
    }

    public void MostrarEscolhas()
    {
        if (GameManager.Instance.questAtiva == false)
        {
            List<Choice> escolhasAtuais = historiaAtual.currentChoices;
            if (escolhasAtuais.Count > 0)
            {
                escolhasAtivas = true;
            }
            else
            {
                escolhasAtivas = false;
            }

            if (escolhasAtuais.Count > escolhas.Length)
            {
                Debug.LogError("Mais escolhas que a interface suporta :D N�o tem gameobject textmeshpro para todos");
            }
            else
            {
                int i = 0;
                foreach (Choice escolha in escolhasAtuais)
                {
                    escolhas[i].gameObject.SetActive(true);
                    textoEscolhas[i].text = escolha.text;
                    i++;
                }

                for (int j = i; j < escolhas.Length; j++)
                {
                    escolhas[j].gameObject.SetActive(false);
                }

                // StartCoroutine(PrimeiraEscolhaSelecionada());
            }
        }
        else
        {
            escolhas[0].SetActive(false);
            escolhas[1].SetActive(false);
        }
    }

    /* IEnumerator PrimeiraEscolhaSelecionada()
      {
          EventSystem.current.SetSelectedGameObject(null);
          yield return new WaitForEndOfFrame();
          EventSystem.current.SetSelectedGameObject(escolhas[0].gameObject);
      }*/

    public void FazerEscolha(int escolhaIndice)
    {
        historiaAtual.ChooseChoiceIndex(escolhaIndice);
        ContinuarDialogo();
        escolhasAtivas = false;
    }

    void ManipularTag(List<string> tagsAtuais) // sprite
    {
        foreach (string tag in tagsAtuais)
        {
            // separando as tags, assim, speaker:Leona
            // salva a key "speaker" e o valor "Leona"
            string[] separarTag = tag.Split(':');
            if (separarTag.Length != 2)
            {
                Debug.LogError("tag nao apropriada");
            }
            string tagKey = separarTag[0].Trim();
            string tagValor = separarTag[1].Trim();

            switch (tagKey)
            {
                case speakerTAG:
                    // Debug.Log("speaker: " + tagValor);
                    nomeExibicao.text = tagValor;

                    if (tagValor == "Narrador")
                    {
                        animatorSprite_Joe.gameObject.SetActive(false);
                        animatorSprite_Luna.gameObject.SetActive(false);

                        mudarSprite = false;
                        isLuna = false;
                        isJoe = false;
                        narradorSom = true;
                    }
                    else if (tagValor == "???" || tagValor == "Luna")
                    {
                        mudarSprite = true;
                        isJoe = false;
                        isLuna = true;
                        narradorSom = false;

                        animatorSprite_Joe.gameObject.SetActive(false);
                        animatorSprite_Luna.gameObject.SetActive(true);
                    }
                    else if (tagValor == "Joe")
                    {
                        mudarSprite = true;
                        isJoe = true;
                        isLuna = false;
                        narradorSom = false;

                        animatorSprite_Joe.gameObject.SetActive(true);
                        animatorSprite_Luna.gameObject.SetActive(false);
                    }
                    break;

                #region Se eu quiser que o arquivo ink determine os sprites 
                /*case portraitTAG_JoeA: // nao-falar
                    animatorSprite_Joe.gameObject.SetActive(true);
                    animatorSprite_Luna.gameObject.SetActive(false);

                    // Debug.Log("portraitJoe: " + tagValor);
                    animatorSprite_Joe.Play(tagValor);

                    spriteValor_NaoFalar = "";
                    spriteValor_NaoFalar = tagValor;

                    isLuna = false;
                    isJoe = true;
                    break;
                case portraitTAG_JoeB: // falar
                    spriteValor_Falar = "";
                    spriteValor_Falar = tagValor;
                    mudarSprite = true;
                    break;*/
                #endregion

                case portraitTAG_LunaA: // nao-falar
                    animatorSprite_Joe.gameObject.SetActive(false);
                    animatorSprite_Luna.gameObject.SetActive(true);

                    // Debug.Log("portraitLuna: " + tagValor);
                    animatorSprite_Luna.Play(tagValor);

                    spriteValor_NaoFalar = "";
                    spriteValor_NaoFalar = tagValor;

                    isLuna = true;
                    isJoe = false;
                    narradorSom = false;
                    break;
                case portraitTAG_LunaB: // falar
                    spriteValor_Falar = "";
                    spriteValor_Falar = tagValor;
                    mudarSprite = true;
                    narradorSom = false;
                    break;

                case portraitTAG_Joe:
                    animatorSprite_Joe.gameObject.SetActive(true);
                    animatorSprite_Luna.gameObject.SetActive(false);

                    isJoe = true;
                    isLuna = false;
                    narradorSom = false;

                    if (skinsManager == null)
                    {
                        Debug.Log("Skin manager nulo");
                    }

                    // se portraitJoe:normal fica normal, se for portrait:sorrindo ele sorri
                    else if (skinsManager != null)
                    {
                        switch (skinsManager.skinEscolhida_indice)
                        {
                            case 0: // skin base
                                spriteValor_NaoFalar = "joe-base-normal";
                                spriteValor_Falar = "joe-base-falando";
                                Debug.Log("Skin escolhida nao-falar:" + spriteValor_NaoFalar);
                                Debug.Log("Skin escolhida falar:" + spriteValor_Falar);

                                if (tagValor == "sorrindo")
                                {
                                    spriteValor_NaoFalar = "joe-base-sorrindo";
                                    Debug.Log("Skin escolhida sorrir:" + spriteValor_NaoFalar);
                                }
                                break;

                            case 1: // skin fazendeiro
                                spriteValor_NaoFalar = "joe-fazendeiro-normal";
                                spriteValor_Falar = "joe-fazendeiro-falando";

                                if (tagValor == "sorrindo")
                                {
                                    spriteValor_NaoFalar = "joe-fazendeiro-sorrindo";
                                }
                                break;

                            case 2: // skin astronauta
                                spriteValor_NaoFalar = "joe-astronauta-normal";
                                spriteValor_Falar = "joe-astronauta-falando";

                                if (tagValor == "sorrindo")
                                {
                                    spriteValor_NaoFalar = "joe-astronauta-sorrindo";
                                }
                                break;
                        }
                    }

                    mudarSprite = true;
                    break;

                case placeTAG:
                    FadeChangeBackground(tagValor);
                    break;

                case questTAG:
                    // visual novel > jogar 3x partidas > desbloquea resto
                    if (GameManager.Instance.questAtiva == false)
                    { GameManager.Instance.partidasQuest = 0; }

                    questKey = tagValor;
                    GameManager.Instance.questAtiva = true;
                    break;

                case correrTAG:
                    if (tagValor == "true") // se a luna for correr
                    {
                        GameManager.Instance.lunaCorrendo = true;
                    }
                    break;
                default:
                    Debug.Log("tag estranha: " + tagValor);
                    break;

            }
        }
    }


    public void FadeChangeBackground(string cenario)
    {
        StartCoroutine(FadeOutAndChangeBackground(cenario));
    }
    IEnumerator FadeOutAndChangeBackground(string cenario)
    {
        // Faz o fade-out (aumenta a opacidade)
        yield return StartCoroutine(Fade(0, 0.5f));

        // Muda o background
        animatorPlace.Play(cenario);

        // Faz o fade-in (diminui a opacidade)
        yield return StartCoroutine(Fade(1, 0));
    }
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        Color color = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Garante que a opacidade final seja exatamente a esperada
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }


    const string nomeArquivo = "InkStoryState.json";
    int indiceFalaAnterior = -1;

    public void SalvarHistoria()
    {
        if (historiaAtual == null)
        {
            Debug.LogError("N�o h� hist�ria atual para salvar.");
            return;
        }

        // Serializa o estado da hist�ria
        string estadoSerializado = historiaAtual.state.ToJson();

        // Caminho completo do arquivo de salvamento
        string caminhoArquivo = System.IO.Path.Combine(Application.persistentDataPath, nomeArquivo);

        // Salva o estado no arquivo
        File.WriteAllText(caminhoArquivo, estadoSerializado);
        Debug.Log($"Estado salvo em: {caminhoArquivo}");
    }

    public void CarregarHistoria()
    {
        // Caminho completo do arquivo de salvamento
        string caminhoArquivo = System.IO.Path.Combine(Application.persistentDataPath, nomeArquivo);

        // Verifica se o arquivo existe
        if (!File.Exists(caminhoArquivo))
        {
            Debug.LogWarning("Nenhum arquivo de estado salvo encontrado.");
            return;
        }

        // L� o estado do arquivo
        string estadoSerializado = File.ReadAllText(caminhoArquivo);

        // Restaura o estado na hist�ria atual
        if (historiaAtual != null)
        {
            historiaAtual.state.LoadJson(estadoSerializado);
            Debug.Log("Estado carregado com sucesso.");
        }
        else
        {
            Debug.LogError("Hist�ria atual � nula. N�o foi poss�vel carregar o estado.");
        }
    }

    public void CarregarAoIniciar()
    {
        if (historiaAtual == null)
        {
            historiaAtual = new Story(inkJSON.text);
        }
        CarregarHistoria();
    }
}
