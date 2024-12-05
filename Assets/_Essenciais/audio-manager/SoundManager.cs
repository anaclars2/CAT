using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioController audioController;

    // lista de musicas
    // 0 - fazenda
    // 1 - segunda chance

    [HideInInspector] public static SoundManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        // audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
        if (audioController == null) { Debug.Log("AudioController nulo"); }
    }

    public void MusicaFazenda()
    {
        audioController.TocarMusic(0);
    }
    public void MusicaSegundaChance()
    {
        audioController.TocarMusic(1);
    }


    // lista de efeitos sonoros
    // 0 - clickar
    // 1 - pular
    // 2 - morrer
    // 3 - slider meow
    // 4 - relogio
    // 5 - pegar peixe moeda
    // 6 - selecionar 
    // 7 - comprar 
    // 8 - erro

    public void SomClicar()
    {
        audioController.TocarSFX(0);
    }
    public void SomPular()
    {
        audioController.TocarSFX(1);
    }
    public void SomMorrer()
    {
        audioController.TocarSFX(2);
    }
    public void SomSlider()
    {
        audioController.TocarSFX(3);
    }
    public void SomRelogio()
    {
        audioController.TocarSFX(4);
    }
    public void SomPeixeMoeda()
    {
        audioController.TocarSFX(5);
    }
    public void SomSelecionar()
    {
        audioController.TocarSFX(6);
    }
    public void SomComprar()
    {
        audioController.TocarSFX(7);
    }
    public void SomErro()
    {
        audioController.TocarSFX(8);
    }
}
