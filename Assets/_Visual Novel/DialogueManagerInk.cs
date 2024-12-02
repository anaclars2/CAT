using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManagerInk : MonoBehaviour
{
    // ink
    Story historiaAtual;
    public TextAsset inkJSON;
    bool escolhasAtivas = false;
    public Animator animatorSprite_Joe;
    public Animator animatorSprite_Luna;

    // efeito maquina de escrever
    string textoAtual = "";
    bool efeitoMaquina = false;
    int indiceChar; // indice do char do texto atual
    [Header("Efeito TypeWritter")]
    float timerTypeWritter;
    public float delayTypeWritter;

    [Header("Escolhas UI")]
    public GameObject[] escolhas;
    TMP_Text[] textoEscolhas;

    [Header("Dialogo UI")]
    public TMP_Text nomeExibicao;
    // public Image spriteExibicaoA, spriteExibicaoB; // onde deve passar as animacoes dos personagem
    public TMP_Text dialogo;

    const string speakerTAG = "speaker"; // quem esta falando no momento 

    [Header("Sprite")]
    public SkinsManager skinsManager;

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
    public float delaySprite;

    bool spriteA = false; // spriteA é sempre a sem falar
    bool isLuna = false;
    bool isJoe = false;

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
        Debug.Log($"MUDARSPRITE == {mudarSprite}");

        // efeito maquina de escrever
        if (efeitoMaquina == true && indiceChar < textoAtual.Length)
        {
            timerTypeWritter = timerTypeWritter + Time.deltaTime;
            if (timerTypeWritter >= delayTypeWritter)
            {
                // adicionando o proximo caractere
                dialogo.text = dialogo.text + textoAtual[indiceChar];

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
                    Debug.Log($"SPRITE FALAR == {spriteValor_Falar}");
                }
                else if (isJoe == true)
                {
                    animatorSprite_Joe.Play(spriteValor_Falar);
                    Debug.Log($"SPRITE FALAR == {spriteValor_Falar}");
                }
            }
            else
            {
                if (isLuna == true)
                {
                    animatorSprite_Luna.Play(spriteValor_NaoFalar);
                    Debug.Log($"SPRITE NAO FALAR == {spriteValor_NaoFalar}");
                }
                else if (isJoe == true)
                {
                    animatorSprite_Joe.Play(spriteValor_NaoFalar);
                    Debug.Log($"SPRITE NAO FALAR == {spriteValor_NaoFalar}");
                }
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

    public void ManipularTag(List<string> tagsAtuais) // sprite
    {
        foreach (string tag in tagsAtuais)
        {
            // separando as tags, assim, speaker:Leona
            // salva a key "speaker" e o valor "Leona"
            string[] separarTag = tag.Split(':');
            if (separarTag.Length != 2)
            {
                Debug.LogError("Tag não apropriada");
            }
            string tagKey = separarTag[0].Trim();
            string tagValor = separarTag[1].Trim();

            switch (tagKey)
            {
                case speakerTAG:
                    Debug.Log("speaker: " + tagValor);
                    nomeExibicao.text = tagValor;

                    if (tagValor == "Narrador")
                    {
                        animatorSprite_Joe.gameObject.SetActive(false);
                        animatorSprite_Luna.gameObject.SetActive(false);

                        mudarSprite = false;
                        isLuna = false;
                        isJoe = false;
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
                    break;
                case portraitTAG_LunaB: // falar
                    spriteValor_Falar = "";
                    spriteValor_Falar = tagValor;
                    mudarSprite = true;
                    break;

                case portraitTAG_Joe:
                    animatorSprite_Joe.gameObject.SetActive(true);
                    animatorSprite_Luna.gameObject.SetActive(false);

                    isJoe = true;
                    isLuna = false;

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
