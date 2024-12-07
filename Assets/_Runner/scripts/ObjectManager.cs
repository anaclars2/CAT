using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] bool isGameObject = false;
    [SerializeField] bool isTrigger = false;
    [SerializeField] bool rotacionar180 = true;

    [Header("isGameObject")]
    [SerializeField] static float duracaoMovimento = 15f; // duracao do movimento em segundos
    [SerializeField] static float moveSpeed = 20f;
    float timer = 0f;

    [Header("Blocos")]
    [SerializeField] GameObject[] blocosDesign;

    // timer para instanciar objetos
    [SerializeField] float intervaloInstanciar = 2f;
    float instanciarTimer = 0f;

    void Update()
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
        }

        // logica do temporizador para instanciar objetos
        if (isTrigger == true)
        {
            instanciarTimer += Time.deltaTime;
            // Debug.Log(instanciarTimer);
            // Debug.Log("intervalo: " + intervaloInstanciar);

            if (instanciarTimer > intervaloInstanciar)
            {
                // Debug.Log("oi");
                InstanciarBloco();
                instanciarTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
        {
            Destroy(other.gameObject);
        }
    }

    void InstanciarBloco()
    {
        int instanciar = UnityEngine.Random.Range(0, blocosDesign.Length);
        Instantiate(blocosDesign[instanciar], new Vector3(0, 0, 24), Quaternion.Euler(0, 0, 0));
    }
}