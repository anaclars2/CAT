using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    public SceneController scene_controller;
    [SerializeField] Quests quests;
    int toques_sequenciais;
    public int index_cena_atual;
    bool iniciar_temp = false;
    public float temporizador = 2.5f;
    float duracao;
    Scene cena_atual;
    public Player player;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        // Scene cena_atual = SceneManager.GetActiveScene();
        // index_cena_atual = cena_atual.buildIndex;
        // Debug.Log("INDEX CENA ATUAL: " + index_cena_atual);
    }

    public void Update()
    {
        // Debug.Log("TOQUES SEQUENCIAIS: " + toques_sequenciais);
        // Debug.Log("TEMPORIZADOR: " + iniciar_temp);
        if (duracao <= temporizador && iniciar_temp == true)
        {
            duracao = duracao + Time.deltaTime;

            if (duracao > temporizador)
            {
                iniciar_temp = false;
                duracao = 0;
            }
        }

        // verificando qual cheat o player quer utilizar
        if (iniciar_temp == false)
        {
            if (toques_sequenciais == 4) // vidas infinitas
            {
                player.cheat_vida_infinita = true;
            }
            else if (toques_sequenciais >= 5)
            {
                quests.cheatAtivo = true;
                Debug.Log("cheatAtivo" + quests.cheatAtivo);
            }

            if (toques_sequenciais != 0)
            {
                // restaurando para zero 
                // depois de cumprir o cheat desejado e/ou nao
                // houver um cheat com esse numero de toques
                toques_sequenciais = 0;
            }
        }
    }

    public void ContabilizarTouchs()
    {
        if (toques_sequenciais == 0)
        {
            toques_sequenciais++;
            iniciar_temp = true;
        }
        else if (toques_sequenciais >= 1)
        {
            toques_sequenciais++;
            duracao = 0;
        }

        Debug.Log("touchs" + toques_sequenciais);
    }
}
