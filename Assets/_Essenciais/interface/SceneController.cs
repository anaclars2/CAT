using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

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
        DesativarInputs();
        SceneManager.LoadScene(i);

        if (gameManager.questAtiva == true && i == 1)
        {
            gameManager.partidasQuest++;
        }
    }

    public void CarregarCenaData(int i)
    {
        DataPersistenceManager.Instance.SaveGame();
        DesativarInputs();
        SceneManager.LoadSceneAsync(i);

        if (gameManager.questAtiva == true && i == 1)
        {
            gameManager.partidasQuest++;
        }
    }

    public void CarregarRunner()
    {
        if (gameManager.questAtiva == true)
        {
            gameManager.partidasQuest++;
        }

        DesativarInputs();
        SceneManager.LoadScene(1);
    }

    private void DesativarInputs()
    {
        foreach (var device in InputSystem.devices)
        {
            InputSystem.DisableDevice(device);
            InputSystem.EnableDevice(device);
        }
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("jogo se encerrando");
    }
}
