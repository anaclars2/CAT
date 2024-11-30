using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausar : MonoBehaviour
{
    public GameObject painel_pause;
    public bool runner = false;
    // public Player player;

    public void Pause()
    {
        Time.timeScale = 0f;
        if (runner == true)
        {
            /*if (player != null)
            {
                player.parar_input_player = true;
            }*/
        }
        // painel_pause.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        /*if (runner == true)
        {
            if (player != null)
            {
                player.parar_input_player = false;
            }
        }*/
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
}
