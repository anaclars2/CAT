using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("GeneralManager").GetComponent<GameManager>();
    }
    public void AtivarSkins(bool b)
    {
        gameManager.skinsAtiva = b;
    }

    public void CarregarCena(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void CarregarCenaData(int i)
    {
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(i);
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Aplicação se encerrando");
    }
}
