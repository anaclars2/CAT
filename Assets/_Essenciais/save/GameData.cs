using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[] missoes_concluidas;
    public bool[] skins_desbloqueadas;

    public int total_distancia;
    public int total_powerups;
    public int total_moedas;
    public bool desviou_frutas;
    public int skinEscolhida_indice; 

    public bool questAtiva;
    // public int partidasQuest;

    // quando comecar um novo jogo os valores nesse construtor
    // seram os valores padroes/iniciais
    public GameData()
    {
        this.missoes_concluidas = new bool[8];
        this.skins_desbloqueadas = new bool[3];

        for (int i = 0; i < 8; i++)
        {
            this.missoes_concluidas[i] = false;
            if (i < 3)
            {
                this.skins_desbloqueadas[i] = false;
            }

        }

        this.skinEscolhida_indice = 0;
        this.total_distancia = 0;
        this.total_moedas = 0;
        this.desviou_frutas = false;
        this.total_powerups = 0;

        this.questAtiva = false;
        // this.partidasQuest = 0;
    }
}
