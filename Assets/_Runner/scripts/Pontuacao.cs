using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Pontuacao : MonoBehaviour, IDataPersistence
{
    // ideia do codigo:
    // responsavel pela pontuacao de acordo com a movimentacao em z do jogador

    public Player player;

    float pontos = 0;
    public int pontosTotais = 0;

    [Header("Canvas")]
    public TMP_Text t_pontuacao;
    public TMP_Text t_pontuacaoInfo;

    void Update()
    {
        if (player.machucado < 2)
        {  
            // atualizando os pontos
            pontos += Time.deltaTime * 5;
        }

        // exibindo a pontuacao formatada no texto
        string pontosTexto = pontos.ToString("F0");
        t_pontuacao.text = pontosTexto;

        // atualizando pontosTotais com o valor exibido
        pontosTotais = int.Parse(pontosTexto);

        if (t_pontuacaoInfo.gameObject.activeInHierarchy == true)
        {
            t_pontuacaoInfo.text = pontosTexto;
        }

    }

    public void SaveData(GameData data)
    {
        // implementando a logica de salvar os valores
        data.total_distancia = this.pontosTotais;
    }

    public void LoadData(GameData data)
    {
        // implementando a logica de carregar os valores
        this.pontosTotais = data.total_distancia;
    }
}
