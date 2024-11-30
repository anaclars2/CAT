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
    public TMP_Text dialogo;
    public TextAsset inkJSON;
    bool escolhasAtivas = false;
    public TMP_Text nomeExibicao;
    public Image spriteExibicao; // onde deve passar as animacoes dos personagem
    public Animator animatorSprite;

    [Header("Escolhas UI")]
    public GameObject[] escolhas;
    TMP_Text[] textoEscolhas;

    const string speakerTAG = "speaker"; // quem esta falando no momento 
    const string portraitTAG = "portrait";
    
    private void Start()
    {
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

    public void IniciarDialogo()
    {
        historiaAtual = new Story(inkJSON.text);
        ContinuarDialogo();
    }

    public void IniciarDialogo(TextAsset _inkJSON)
    {
        historiaAtual = new Story(_inkJSON.text);
        ContinuarDialogo();
    }

    public void ContinuarDialogo()
    {
        if (historiaAtual.canContinue)
        {

            dialogo.text = historiaAtual.Continue();
            MostrarEscolhas();
            ManipularTag(historiaAtual.currentTags);
        }
        else
        {
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

    public void ManipularTag(List<string> tagsAtuais)
    {
        foreach (string tag in tagsAtuais)
        {
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
                    break;
                case portraitTAG:
                    Debug.Log("portrait: " + tagValor);
                    animatorSprite.Play(tagValor);
                    break;
                default:
                    Debug.Log("tag estranha: " + tagValor);
                    break;

            }
        }
    }
}
