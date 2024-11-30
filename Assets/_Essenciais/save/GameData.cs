using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int missao_aceita;
    public bool[] missoes_concluidas;
    public bool[] skins_desbloqueadas;
    public float[] progresso_missao;

    public int total_distancia;
    public int total_moedas;
    public bool desviou_frutas;
    public bool pegou_vacina;

    public string progresso_historia; // public int fala_atual

    // quando comecar um novo jogo os valores nesse construtor
    // seram os valores padroes/iniciais
    public GameData()
    {
        this.missoes_concluidas = new bool[8];
        this.progresso_missao = new float[8];
        this.skins_desbloqueadas = new bool[3];

        this.missao_aceita = 100;
        for (int i = 0; i < 8; i++)
        {
            this.missoes_concluidas[i] = false;
            this.progresso_missao[i] = 0;
            if (i < 3)
            {
                this.skins_desbloqueadas[i] = false;
            }

        }

        this.total_distancia = 0;
        this.total_moedas = 0;
        this.desviou_frutas = false;
        this.pegou_vacina = false;

        this.progresso_historia = "";
    }
}
