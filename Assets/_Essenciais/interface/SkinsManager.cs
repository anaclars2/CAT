using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SkinsManager : MonoBehaviour, IDataPersistence
{
    [Header("Tipo Cena")]
    [SerializeField] bool isGame = false;

    [Header("Skins e Gerenciamento")]
    public bool[] skins_desbloqueadas = new bool[3];
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject censura;
    [SerializeField] GameObject[] skinsGame = new GameObject[3];
    PostProcessVolume volume;
    ColorGrading colorGrading;
    public int skinEscolhida_indice;

    [Header("Camera")]
    public GameObject objectCamera;

    [Header("UI e Navegação")]
    [SerializeField] Vector3[] posicoesObjetos;
    [SerializeField] Text nomeSkin;
    [SerializeField] Text descricaoSkin;
    public TMP_Text peixe_moeda;

    public string[] nomes;
    public string[] descricoes;
    [SerializeField] Button botaoA;
    [SerializeField] Button botaoB;

    int objetoAtual_indice = 0;
    int textos_indice = 0;

    private void Awake()
    {
        ConferirCena(SceneManager.GetActiveScene().buildIndex);
        if (isGame == false)
        {
            AtualizarIndices(skinEscolhida_indice);
        }
    }

    void Start()
    {
        if (isGame == false)
        {
            volume = objectCamera.gameObject.GetComponent<PostProcessVolume>();
            if (volume == null) { Debug.LogError("Post processing volume nulo >:C"); }
            if (volume.profile.TryGetSettings<ColorGrading>(out colorGrading))
            {
                // Debug.Log("Color grading encontrado!");
            }

            // adicionando listeners para os botoes
            botaoA.onClick.AddListener(MoverParaProximo);
            botaoB.onClick.AddListener(MoverParaAnterior);

            Atualizar();
            VerificarCensura();
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (i != skinEscolhida_indice)
                {
                    skinsGame[i].SetActive(false);
                }
                else
                {
                    skinsGame[i].SetActive(true);
                }
            }
        }
    }

    void Update()
    {
        // Debug.Log($"skin escolhida:{skinEscolhida_indice} \nindice objeto atual: {objetoAtual_indice}");

        if (isGame == false)
        {
            // Debug.Log($"INDICE OBJETO ATUAL: {objetoAtual_indice} z STATUS OBJETO ATUAL: {skins_desbloqueadas[objetoAtual_indice]}");
            VerificarCensura();
            peixe_moeda.text = gameManager.peixe_moeda.ToString();
        }

        ConferirCena(SceneManager.GetActiveScene().buildIndex);
    }

    public void ComprarSkin()
    {
        if (isGame == false)
        {
            int precoSkin = 600 * (objetoAtual_indice + 1);

            // Debug.Log($"Tentando comprar skin {objetoAtual_indice} - Preço: {precoSkin}, Peixe-Moeda: {gameManager.peixe_moeda}");

            if (gameManager.peixe_moeda >= precoSkin && !skins_desbloqueadas[objetoAtual_indice])
            {
                gameManager.peixe_moeda -= precoSkin;
                skins_desbloqueadas[objetoAtual_indice] = true;
                SoundManager.Instance.SomComprar();
                Debug.Log($"Skin {objetoAtual_indice} desbloqueada!");
                Debug.Log($"Peixe-Moeda: {gameManager.peixe_moeda}");

                Atualizar();
                VerificarCensura();
            }
            else
            {
                SoundManager.Instance.SomErro();
            }
        }
    }


    private void MoverParaProximo()
    {
        // avancando para o proximo objeto
        AtualizarIndices((objetoAtual_indice + 1) % posicoesObjetos.Length);
    }

    private void MoverParaAnterior()
    {
        // voltando para o objeto anterior
        AtualizarIndices((objetoAtual_indice - 1 + posicoesObjetos.Length) % posicoesObjetos.Length);
    }

    private void Atualizar()
    {
        if (objectCamera != null)
        {
            // atualizando a posicao da camera e textos
            objectCamera.transform.localPosition = posicoesObjetos[objetoAtual_indice]; // esse script deve estar na camera

            // Debug.Log($"posicaoObjeto: {posicoesObjetos[objetoAtual_indice]}");
            nomeSkin.text = nomes[textos_indice];
            descricaoSkin.text = descricoes[textos_indice];
        }
    }

    private void VerificarCensura()
    {
        // atualizando a censura com base no estado da skin
        if (skins_desbloqueadas[objetoAtual_indice] == true)
        {
            // se a skin esta desbloqueada
            if (censura != null)
            {
                censura.SetActive(false);
            }
            if (colorGrading != null)
            {
                colorGrading.active = false;
            }
        }
        else
        {
            // se a skin esta bloqueada
            if (censura != null)
            {
                censura.SetActive(true);
            }
            if (colorGrading != null)
            {
                colorGrading.active = true;
            }
        }
    }
    void AtualizarIndices(int novoIndice)
    {
        objetoAtual_indice = novoIndice;
        skinEscolhida_indice = objetoAtual_indice;
        textos_indice = objetoAtual_indice % nomes.Length;

        Atualizar();
        VerificarCensura();
    }


    public void SaveData(GameData data)
    {
        data.skins_desbloqueadas = this.skins_desbloqueadas;
        data.skinEscolhida_indice = this.skinEscolhida_indice;
    }

    public void LoadData(GameData data)
    {
        this.skins_desbloqueadas = data.skins_desbloqueadas;
        AtualizarIndices(data.skinEscolhida_indice);
    }

    void ConferirCena(int indice)
    {
        Debug.Log("indiceCena: " + indice);
        Debug.Log("isGame: " + isGame);
        if (indice == 0)
        {
            isGame = false;
        }
        else if (indice == 1) // significa que é o runner
        {
            isGame = true;
        }
    }
}
