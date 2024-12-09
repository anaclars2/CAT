using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public bool isGameObject = false;
    [SerializeField] bool isInstanciador = false;
    [SerializeField] bool rotacionar180 = true;

    [Header("isGameObject")]
    [SerializeField] static float duracaoMovimento = 8f; // duracao do movimento em segundos
    public static float moveSpeed = 20f;
    float timer = 0f;

    [Header("GameObjects")]
    [SerializeField] GameObject[] blocosDesign;
    [SerializeField] GameObject powerUp;

    // timer para instanciar objetos
    [SerializeField] float intervaloInstanciar_blocos = 2f;
    [SerializeField] float intervaloInstanciar_poweup = 20f;
    float instanciarTimer_blocos = 0f;
    float instanciarTimer_powerUp = 0f;

    [Header("Referencia")]
    [SerializeField] Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (player.machucado < 2)
        {
            if (isGameObject == true)
            {
                // movimentando o gameObject ao longo do eixo Z
                if (timer < duracaoMovimento)
                {
                    if (rotacionar180)
                    {
                        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                    }
                    timer += Time.deltaTime;
                }
                else
                {
                    if (transform.parent != null)
                    {
                        Destroy(transform.parent.gameObject);
                        // Destroy(this.gameObject);
                    }
                }
            }

            // logica do temporizador para instanciar blocos
            if (isInstanciador == true)
            {
                instanciarTimer_blocos += Time.deltaTime;
                // Debug.Log(instanciarTimer);
                // Debug.Log("intervalo: " + intervaloInstanciar);

                if (instanciarTimer_blocos > intervaloInstanciar_blocos)
                {
                    // Debug.Log("oi");
                    InstanciarBlocos();
                    instanciarTimer_blocos = 0f;
                }
            }

            // logica do temporizador para instanciar objetos
            if (isInstanciador == true)
            {
                instanciarTimer_powerUp += Time.deltaTime;
                if (instanciarTimer_powerUp > intervaloInstanciar_poweup)
                {
                    Instanciar(powerUp);
                    instanciarTimer_powerUp = 0f;
                }
            }
        }
    }

    void InstanciarBlocos()
    {
        int instanciar = UnityEngine.Random.Range(0, blocosDesign.Length);
        Instantiate(blocosDesign[instanciar], new Vector3(0, 0, 24), Quaternion.Euler(0, 0, 0));
    }

    void Instanciar(GameObject gameObject)
    {
        intervaloInstanciar_poweup = UnityEngine.Random.Range(5, 26);

        int[] opcoesX = { -3, 0, 3 };
        int x = UnityEngine.Random.Range(0, opcoesX.Length);
        int z = UnityEngine.Random.Range(4, 41);

        Instantiate(gameObject, new Vector3(opcoesX[x], 0, z), Quaternion.Euler(0, 0, 0));
    }
}