using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

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

    const string speakerTAG = "speaker"; // quem esta falando no momento 

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

    bool spriteA = false; // spriteA é sempre a sem falar
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

    // vai randomizar uma primeira vez o valor (tom e clipe) de uma letra, exemplo "a"
    // e o reutilizar sempre o mesmo valor deixando parecido com uma alfabeto
    [SerializeField] bool deixarPrevisivel;

    private void Awake()
    {
        audioSource = GameObject.Find("AudioSourceSFX").GetComponent<AudioSource>();
        if (audioSource == null) { Debug.Log("nao consegui achar audioSource"); }
    }

    private void Start()
    {
        dialogo.text = "";
        IniciarDialogo();

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

            if (spriteA == false) // spriteA = false é a de falar
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

    public void IniciarDialogo()
    {
        historiaAtual = new Story(inkJSON.text);
        ContinuarDialogo();
    }

    public void IniciarDialogo(TextAsset _inkJSON)
    {
        historiaAtual = new Story(_inkJSON.text);
        dialogo.text = "";
        ContinuarDialogo();
    }

    public void ContinuarDialogo()
    {
        if (dialogo.text.Length < textoAtual.Length)
        {
            // mostrar o texto completo da fala atual se ele ainda nao estiver completo
            dialogo.text = textoAtual;
            efeitoMaquina = false;
        }
        else if (historiaAtual.canContinue)
        {
            // limpando o texto e o estado atual antes de exibir a nova fala
            dialogo.text = "";
            indiceChar = 0;
            timerTypeWritter = 0;

            // se o texto ja estiver completo, avançar para a proxima fala
            textoAtual = historiaAtual.Continue(); // dialogo.text = historiaAtual.Continue();
            MostrarEscolhas();
            ManipularTag(historiaAtual.currentTags); // sprite
            efeitoMaquina = true;
        }
        else
        {
            efeitoMaquina = false;
            if (!escolhasAtivas)
                DesativarDialogo();
        }
    }

    public void DesativarDialogo()
    {
        dialogo.text = "";
    }

    public void MostrarEscolhas()
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
            Debug.LogError("Mais escolhas que a interface suporta :D Não tem gameobject textmeshpro para todos");
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

                    if (skinsManager != null)
                    {
                        switch (skinsManager.skinEscolhida_indice)
                        {
                            case 0: // skin base
                                spriteValor_NaoFalar = "joe-base-normal";
                                spriteValor_Falar = "joe-base-falando";

                                if (tagValor == "sorrindo")
                                {
                                    spriteValor_NaoFalar = "joe-base-sorrindo";
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

                default:
                    Debug.Log("tag estranha: " + tagValor);
                    break;

            }
        }
    }
}
