using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Arquivo Json Config")]
    public string nomeArquivo;

    GameData gameData;
    public static DataPersistenceManager Instance;
    List<IDataPersistence> obj_dataPersistences;
    FilePersistenceManipulator fileManipulator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.fileManipulator = new FilePersistenceManipulator(nomeArquivo);
    }

    public void NewGame()
    {
        Debug.Log("criado um gameData");
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // carrega qualquer arquivo de DataGame usando manipulador de data
        this.gameData = fileManipulator.Load();

        // se nao achar entao criamos um novo jogo :D
        if (this.gameData == null)
        {
            Debug.Log("sem gameData, criando um novo");
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.Log("sem gameData ainda");
        }

        // em seguida, vamos enviar os dados desse arquivo gameData
        // para todos que precisam dele
        foreach (IDataPersistence dataObj in obj_dataPersistences)
        {
            dataObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogError("gameData é null ao tentar salvar");
            return;
        }

        // vai passar os dados para os scripts os deixando atualizados
        foreach (IDataPersistence dataObj in obj_dataPersistences)
        {
            dataObj.SaveData(gameData);
        }
        // salvar os dados usando o manipulador de data
        Debug.Log("save :d");

        fileManipulator.Save(gameData);
    }

    // para salvar o jogo quando sairmos dele e quando darmos pause
    private void OnApplicationQuit()
    {
        if (gameData != null)
        {
            SaveGame();
        }
        else
        {
            Debug.Log("onApplicationQuit gameData nulo");
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (gameData != null)
        {
            SaveGame();
        }
        else
        {
            Debug.Log("onApplicationPause gameData nulo");
        }
    }

    List<IDataPersistence> FindAllPersistencesObjects()
    {
        // o using Linq permite que procuremos todos os objetos que herdem de MonoBehaviour
        // e implementem a interface de IDataPersistence

        // assim, estamos os contando
        IEnumerable<IDataPersistence> dataObjetos = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        // e em seguida os passando em uma lista
        return new List<IDataPersistence>(dataObjetos);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded+= OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // procurando todos os objetos que precisam de dados
        this.obj_dataPersistences = FindAllPersistencesObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }
}
