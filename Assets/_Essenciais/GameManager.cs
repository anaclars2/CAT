using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public int peixe_moeda;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveData(GameData data)
    {
        // implementando a logica de salvar os valores
        data.total_moedas = this.peixe_moeda;
    }

    public void LoadData(GameData data)
    {
        // implementando a logica de carregar os valores
        this.peixe_moeda = data.total_moedas;
    }

}
