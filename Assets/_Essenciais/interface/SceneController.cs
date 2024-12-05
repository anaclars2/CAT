using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void CarregarCena(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void CarregarCenaData(int i)
    {
        SceneManager.LoadSceneAsync(i);
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Aplicação se encerrando");
    }
}
