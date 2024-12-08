using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausar : MonoBehaviour
{
    // [SerializeField] GameObject painel_pause;
    bool runner = false;
    [SerializeField] Player player;

    private void Awake()
    {
        if (player == null)
        {
            player = gameObject.GetComponent<Player>();
        }

        ConferirCena(SceneManager.GetActiveScene().buildIndex);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        if (runner == true)
        {
            player.parar_input_player = true;
        }
        // painel_pause.SetActive(true);
    }

    public void Continue()
    {
        if (runner == true)
        {
            player.parar_input_player = false;
        }
        Time.timeScale = 1f;
        // painel_pause.SetActive(false);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void ConferirCena(int indice)
    {
        if (indice == 0)
        {
            runner = false;
        }
        else if (indice == 1) // significa que é o runner
        {
            runner = true;
        }
    }
}
