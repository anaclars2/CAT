using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class SkinsManager : MonoBehaviour, IDataPersistence
{
    [Header("Tipo Cena")]
    public bool isNovel = false;

    [Header("Skins e Gerenciamento")]
    public bool[] skins_desbloqueadas = new bool[3];
    public GameManager gameManager;
    public GameObject censura;
    PostProcessVolume volume;
    ColorGrading colorGrading;
    public int skinEscolhida_indice; 

    [Header("Camera")]
    public GameObject objectCamera;

    [Header("UI e Navegação")]
    public Vector3[] posicoesObjetos;
    public Text nomeSkin;
    public Text descricaoSkin;
    public TMP_Text peixe_moeda;

    public string[] nomes;
    public string[] descricoes;
    public Button botaoA;
    public Button botaoB;
    public Button botaoComprar;

    private int objetoAtual_indice = 0;
    private int textos_indice = 0;

    void Start()
    {
        if (isNovel == false)
        {
            volume = objectCamera.gameObject.GetComponent<PostProcessVolume>();
            if (volume == null) { Debug.LogError("Post processing volume nulo >:C"); }
            if (volume.profile.TryGetSettings<ColorGrading>(out colorGrading))
            {
                Debug.Log("Color grading encontrado!");
            }

            // adicionando listeners para os botoes
            botaoA.onClick.AddListener(MoverParaProximo);
            botaoB.onClick.AddListener(MoverParaAnterior);

            Atualizar();
            VerificarCensura();
        }
    }

    void Update()
    {
        if (isNovel == false)
        {
            Debug.Log($"INDICE OBJETO ATUAL: {objetoAtual_indice} z STATUS OBJETO ATUAL: {skins_desbloqueadas[objetoAtual_indice]}");
            VerificarCensura();
            peixe_moeda.text = gameManager.peixe_moeda.ToString();
        }
    }

    public void ComprarSkin()
    {
        int precoSkin = 600 * (objetoAtual_indice + 1);
        Debug.Log($"Tentando comprar skin {objetoAtual_indice} - Preço: {precoSkin}, Peixe-Moeda: {gameManager.peixe_moeda}");
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

    public void EscolherSkin()
    {

    }

    private void MoverParaProximo()
    {
        // avancando para o proximo objeto
        objetoAtual_indice = (objetoAtual_indice + 1) % posicoesObjetos.Length;
        textos_indice = (textos_indice + 1) % nomes.Length;

        Atualizar();
        VerificarCensura();
    }

    private void MoverParaAnterior()
    {
        // voltando para o objeto anterior
        objetoAtual_indice = (objetoAtual_indice - 1 + posicoesObjetos.Length) % posicoesObjetos.Length;
        textos_indice = (textos_indice - 1 + nomes.Length) % nomes.Length;

        Atualizar();
        VerificarCensura();
    }

    private void Atualizar()
    {
        // atualizando a posicao da camera e textos
        objectCamera.transform.localPosition = posicoesObjetos[objetoAtual_indice]; // esse script deve estar na camera
        Debug.Log($"posicaoObjeto: {posicoesObjetos[objetoAtual_indice]}");
        nomeSkin.text = nomes[textos_indice];
        descricaoSkin.text = descricoes[textos_indice];
    }

    private void VerificarCensura()
    {
        // atualizando a censura com base no estado da skin
        if (skins_desbloqueadas[objetoAtual_indice] == true)
        {
            // se a skin esta desbloqueada
            censura.SetActive(false);
            if (colorGrading != null)
            {
                colorGrading.active = false;
            }
        }
        else
        {
            // se a skin esta bloqueada
            censura.SetActive(true);
            if (colorGrading != null)
            {
                colorGrading.active = true;
            }
        }
    }

    public void SaveData(GameData d)
    {
        // Implementar lógica de salvar
    }

    public void LoadData(GameData d)
    {
        // Implementar lógica de carregar
    }
}
