using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.fileManipulator = new FilePersistenceManipulator(nomeArquivo);
        // procurando todos os objetos que precisam de dados
        this.obj_dataPersistences = FindAllPersistencesObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // carrega qualquer arquivo de DataGame usando manipulador de data
        this.gameData = fileManipulator.Load();

        // se nao achar entao criamos um novo jogo :D
        if (this.gameData != null)
        {
            Debug.Log("sem gameData, criando um novo");
            NewGame();
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
        // SaveGame();
    }
    private void OnApplicationPause(bool pause)
    {
        // SaveGame();
    }

    List<IDataPersistence> FindAllPersistencesObjects()
    {
        // o using Linq permite que procuremos todos os objetos que herdem de MonoBehaviour
        // e implementem a interface de IDataPersistence

        // assim, estamos os contando
        IEnumerable<IDataPersistence> dataObjetos = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        // e em seguida os passando em uma lista
        return new List<IDataPersistence>(dataObjetos);
    }
}
