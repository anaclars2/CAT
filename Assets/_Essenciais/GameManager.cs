using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public int peixe_moeda;
    public int powerups;
    public int powerupsPartida;
    public int partidasQuest = 0;
    public static GameManager Instance;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject skins;
    [HideInInspector] public bool skinsAtiva = false;
    public bool lunaCorrendo;
    public bool questAtiva = false;

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
        Debug.LogError("chamou o save de gameManager vei p salvar");

        data.total_moedas = this.peixe_moeda;
        data.total_powerups = this.powerups;
        data.questAtiva = this.questAtiva;

    }

    public void LoadData(GameData data)
    {
        // implementando a logica de carregar os valores
        this.peixe_moeda = data.total_moedas;
        this.powerups = data.total_powerups;
        this.questAtiva = data.questAtiva;

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            AtivarSkins();
        }
    }

    public void AtivarSkins(bool b)
    {
        skinsAtiva = b;
    }
    void AtivarSkins()
    {
        if (skinsAtiva == true)
        {
            menu = GameObject.Find("Canvas").transform.Find("menu").gameObject;
            skins = GameObject.Find("Canvas").transform.Find("skins").gameObject;
            menu.SetActive(false);
            skins.SetActive(true);
            skinsAtiva = false;
        }
    }
}
