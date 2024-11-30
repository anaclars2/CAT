using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // public CenaData cenaData;
    // public CenaDataManager cenaDataManager;

    private void Awake()
    {
        /*if (cenaDataManager == null)
        {
            cenaDataManager = new CenaDataManager();
        }
        if (cenaData == null)
        {
            cenaData = new CenaData();
        }

        cenaDataManager.CarregarDados(cenaData);*/
    }

    public void SalvarCena()
    {
       // cenaDataManager.SalvarDados(cenaData);
    }

    public void CarregarCena()
    {
        // SceneManager.LoadScene(cenaData.id_cena);
    }
    public void CarregarCena(int i)
    {
        SceneManager.LoadScene(i);
    }   

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Aplicação se encerrando");
    }
}
