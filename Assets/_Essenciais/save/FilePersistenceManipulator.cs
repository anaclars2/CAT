using System;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class FilePersistenceManipulator
{
    string nomeArquivo = "";

    public FilePersistenceManipulator(string nomeArquivo)
    {
        this.nomeArquivo = nomeArquivo + ".txt";
    }
    public GameData Load()
    {
        string caminho = Path.Combine(Application.persistentDataPath, nomeArquivo);
        GameData dataJson = null;
        if (File.Exists(caminho))
        {
            try
            {
                string leitor = "";
                using (FileStream stream = new FileStream(caminho, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        leitor = reader.ReadToEnd();
                    }
                }

                // tornando em dados do jogo denovo
                dataJson = JsonUtility.FromJson<GameData>(leitor);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Erro tentando carregar o arquivo no caminho: {caminho}\n{e}");
            }
        }
        return dataJson;
    }

    public void Save(GameData data)
    {
        string caminho = Path.Combine(Application.persistentDataPath, nomeArquivo);
        try
        {
            caminho = Path.Combine(Application.persistentDataPath, nomeArquivo);
            string dataJson = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(caminho, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataJson);
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"Erro tentando salvar o arquivo no caminho: {caminho}\n{e}");
        }
    }
}