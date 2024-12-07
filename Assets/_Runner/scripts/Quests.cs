using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quests : MonoBehaviour, IDataPersistence
{
    [HideInInspector] public int missao_ativa; // vai ser manipulada pelo ink
    [SerializeField] bool isGame = false;

    public bool[] missoes_concluidas = new bool[8];

    [Header("Conquistas PopUp e Botao")]
    [SerializeField] string[] textoMissoes;
    [SerializeField] TMP_Text p_conquista;
    [SerializeField] GameObject popUp;
    [SerializeField] Animator animator;
    [SerializeField] Text b_conquista;

    [Header("Acessar dados")]
    [SerializeField] Pontuacao pontuacaoDistancia;
    [SerializeField] Player player;
    [SerializeField] GameManager manager;

    int conquistaAtual = 0;

    private void Update()
    {
        if (isGame)
        {
            Conferir();
        }

        // Debug.LogError($"pontuacaoDistancia.pontosTotais : {pontuacaoDistancia.pontosTotais}");
        // Debug.LogError($"!missoes_concluidas[0] : {!missoes_concluidas[0]}");
        // Debug.LogError($"missao_ativa : {missao_ativa}");
    }

    private void Start()
    {
        missao_ativa = 0;
    }
    public void Conferir()
    {
        if (missao_ativa < 0 || missao_ativa >= missoes_concluidas.Length)
        {
            Debug.LogError($"Valor da missao ativa muito maior que array: {missao_ativa}");
            return;
        }

        switch (missao_ativa)
        {
            case 0:
                if (!missoes_concluidas[0] && pontuacaoDistancia.pontosTotais >= 600)
                {
                    missoes_concluidas[0] = true;
                    AtualizarPopUp(missao_ativa);
                }
                break;
            case 1:
                if (!missoes_concluidas[1] && manager.powerupsPartida >= 10) // missao 2
                {
                    missoes_concluidas[1] = true;
                    AtualizarPopUp(missao_ativa);
                }
                break;
            case 2:
                if (!missoes_concluidas[2] && pontuacaoDistancia.pontosTotais >= 1200) // missao 3
                {
                    missoes_concluidas[2] = true;
                    AtualizarPopUp(missao_ativa);
                }
                break;
        }
    }

    public void AtualizarPopUp(int i)
    {
        if (i < 0 || i >= textoMissoes.Length)
        {
            Debug.LogError($"Valor da missao ativa muito maior que array");
            return;
        }

        popUp.SetActive(true);
        animator.SetTrigger("Ativar");
        Debug.Log("i:" + i);

        p_conquista.text = textoMissoes[i];

        // iniciando a coroutine para aguardar e desativar o popUp
        StartCoroutine(DesativarPopUpDelay(6f));
    }

    private IEnumerator DesativarPopUpDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popUp.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        this.missoes_concluidas = data.missoes_concluidas;
        this.missao_ativa = data.missao_ativa;
    }

    public void SaveData(GameData data)
    {
        data.missoes_concluidas = this.missoes_concluidas;
        data.missao_ativa = this.missao_ativa;
    }

    public void ProximaConquista()
    {
        // avancando para o proximo objeto
        AtualizarIndices((conquistaAtual + 1) % textoMissoes.Length);
    }

    public void ConquistaAnterior()
    {
        // voltando para o objeto anterior
        AtualizarIndices((conquistaAtual - 1 + textoMissoes.Length) % textoMissoes.Length);
    }

    void AtualizarIndices(int novoIndice)
    {
        conquistaAtual = novoIndice;
        if (missoes_concluidas[conquistaAtual] == true)
        {
            b_conquista.text = textoMissoes[conquistaAtual];
        }
        else
        {
            ProximaConquista();
        }

    }

}