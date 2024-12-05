using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    // ideia do codigo:
    // responsavel pela pontuacao de acordo com a movimentacao em z do jogador

    // atualizacao 27.10.2024: remocao de adicao de pontuacao por desviar

    public Player player;

    float pontos = 0;
    float distancia_z;
    public int pontosTotais = 0;

    [Header("Canvas")]
    public TMP_Text t_pontuacao;

    void Update()
    {
        // pontos por distancia percorrida
        distancia_z = player.transform.position.z;
        pontos = distancia_z / 10; // pra ficar um numero menor
        pontosTotais += (int)pontos;

        t_pontuacao.text = pontos.ToString("F0");
    }
}
