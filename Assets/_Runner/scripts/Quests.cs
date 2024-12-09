using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Quests : MonoBehaviour, IDataPersistence
{
    [SerializeField] bool isGame = false;
    public bool[] missoes_concluidas = new bool[8];

    [Header("Dados pop-up")]
    [SerializeField] string[] textoMissoes;
    [SerializeField] string[] nomeMissoes;
    [SerializeField] TMP_Text textoConquista;
    [SerializeField] TMP_Text nomeConquista;
    [SerializeField] GameObject[] estrelaImagem;
    [SerializeField] GameObject[] imagensConquista;

    [Header("Elementos UI")]
    [SerializeField] GameObject popUp;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] imag_conquistasBloqueadas;

    [Header("Acessar dados")]
    [SerializeField] Pontuacao pontuacaoDistancia;
    [SerializeField] Player player;
    [SerializeField] GameManager manager;
    [SerializeField] SkinsManager skinManager;
    [SerializeField] ObjectManager objectManager;
    [HideInInspector] public bool cheatAtivo;

    private void Awake()
    {
        skinManager = GetComponent<SkinsManager>();
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
    }
    private void Start()
    {
        for (int i = 0; i > missoes_concluidas.Length; i++)
        {
            if (missoes_concluidas[i] == true)
            {
                imag_conquistasBloqueadas[i].SetActive(true);
            }
        }
    }
    private void Update()
    {
        if (cheatAtivo == true)
        {
            manager.powerupsPartida = 25;
            pontuacaoDistancia.pontosTotais = 13000;
            skinManager.skinsDesbloqueadas_numero = 3;
            skinManager.skins_desbloqueadas[0] = true;
            skinManager.skins_desbloqueadas[1] = true;
            skinManager.skins_desbloqueadas[2] = true;
            manager.peixe_moeda = 350;

            for (int i = 0; i < missoes_concluidas.Length; i++)
            {
                missoes_concluidas[i] = true;
            }
            cheatAtivo = false;
        }

        if (skinManager == null)
        {
            skinManager = GameObject.Find("InterfaceManager").GetComponent<SkinsManager>();
        }
        if (manager == null)
        {
            manager = GameObject.Find("GeneralManager").GetComponent<GameManager>();
        }

        if (isGame == true)
        {
            ConferirPopUp();
            ConferirImagens();
        }
        else
        {
            ConferirImagens();
        }

        // Debug.LogError($"pontuacaoDistancia.pontosTotais : {pontuacaoDistancia.pontosTotais}");
        // Debug.LogError($"!missoes_concluidas[0] : {!missoes_concluidas[0]}");
        // Debug.LogError($"missao_ativa : {missao_ativa}");
    }

    public void ConferirPopUp()
    {
        // Debug.Log("conferioupopupaa");

        if ((!missoes_concluidas[0] && pontuacaoDistancia.pontosTotais >= 600) || cheatAtivo == true) // missao 1
        {
            missoes_concluidas[0] = true;
            AtualizarPopUp(0);
        }
        if ((!missoes_concluidas[1] && manager.powerupsPartida >= 25) || cheatAtivo == true) // missao 2
        {
            missoes_concluidas[1] = true;
            AtualizarPopUp(1);
            // Debug.Log("ativeiMissao");
        }
        if ((!missoes_concluidas[2] && GameManager.Instance.lunaCorrendo == true) || cheatAtivo == true) // missao 3
        {
            missoes_concluidas[2] = true;
            AtualizarPopUp(2);
        }
        if ((!missoes_concluidas[3] && pontuacaoDistancia.pontosTotais >= 1200) || cheatAtivo == true) // missao 4
        {
            missoes_concluidas[3] = true;
            AtualizarPopUp(3);
        }
        if ((!missoes_concluidas[4] && skinManager.skinsDesbloqueadas_numero >= 3) || cheatAtivo == true) // missao 5
        {
            missoes_concluidas[4] = true;
            AtualizarPopUp(4);
        }
        if ((!missoes_concluidas[5] && (player.modoFrutas == true && player.peixeMoedasPartida == 50)) || cheatAtivo == true) // missao 6
        {
            missoes_concluidas[5] = true;
            AtualizarPopUp(5);
        }
        if ((!missoes_concluidas[6] && player.peixeMoedasPartida == 350) || cheatAtivo == true)// missao 7
        {
            missoes_concluidas[6] = true;
            AtualizarPopUp(6);
        }

        // missao 8
        int _missoesDesbloqueadas = 0;
        foreach (bool m in missoes_concluidas)
        {
            if (m == true) { _missoesDesbloqueadas++; }
        }
        if ((_missoesDesbloqueadas == 7 && !missoes_concluidas[7]) || cheatAtivo == true)
        {
            missoes_concluidas[7] = true;
            AtualizarPopUp(7);
        }


    }

    public void AtualizarPopUp(int i)
    {
        if (i < 0 || i >= textoMissoes.Length)
        {
            // Debug.LogError($"Valor da missao ativa muito maior que array");
            return;
        }

        popUp.SetActive(true);
        animator.SetTrigger("Ativar");
        // Debug.Log("i:" + i);

        textoConquista.text = textoMissoes[i];
        nomeConquista.text = nomeMissoes[i];

        foreach (GameObject o in imagensConquista)
        {
            // Debug.Log("entrouforeach");
            o.SetActive(false);
        }
        imagensConquista[i].SetActive(true);
        if (i == 0)
        {
            // Debug.Log("entrouif");
            estrelaImagem[0].SetActive(true); // estrela-branco
            estrelaImagem[1].SetActive(false); // estrela-normal
        }

        // iniciando a coroutine para aguardar e desativar o popUp
        StartCoroutine(DesativarPopUpDelay(4.5f));
        // Debug.Log("chamouPopup");
    }
    private IEnumerator DesativarPopUpDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popUp.SetActive(false);
    }

    public void ConferirImagens()
    {
        for (int i = 0; i < missoes_concluidas.Length; i++)
        {
            if (missoes_concluidas[i] == true)
            {
                // Debug.LogError("missoes_concluidas[i]: " + missoes_concluidas[i]);
                // Debug.LogError("mag_conquistasBloqueadas[i]: " + imag_conquistasBloqueadas[i]);
                imag_conquistasBloqueadas[i].SetActive(false);
            }
        }
    }

    public void LoadData(GameData data)
    {
        this.missoes_concluidas = data.missoes_concluidas;
    }

    public void SaveData(GameData data)
    {
        data.missoes_concluidas = this.missoes_concluidas;
    }
}