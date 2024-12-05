using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculosDinamicos : MonoBehaviour
{
    // ideia do script
    // movimentar a caminhonete e tambem instanciar as frutas-obstaculos

    public bool caminhonete = false;
    public bool fruta = false;
    public GameObject[] frutas;
    float timer = 0f, cooldown = 4f;

    float velocidadeFrontal = 25f;
    float movimentoArco = 12f;
    float velocidadeVertical;


    void Update()
    {
        if (caminhonete == true)
        {
            // movimento do inimigo
            transform.position = transform.position + new Vector3(0, 0, 10) * Time.deltaTime;

            // instanciando as frutas-obstaculos
            if (timer >= 0f)
            {
                timer = timer + Time.deltaTime;
                if (timer >= cooldown)
                {
                    InstanciarFrutas();
                    // resetando o timer
                    timer = 0f;
                }
            }
        }
        else if (fruta == true)
        {
            // movimento do obstaculo
            // transform.position = transform.position + new Vector3(0, 0, -15) * Time.deltaTime;

            // reduzindo a velocidade vertical para simular a gravidade
            velocidadeVertical -= 9.8f * Time.deltaTime;

            // movendo a fruta para frente (no eixo Z) e aplicando a curva (eixo Y)
            transform.position += new Vector3(0, velocidadeVertical, -velocidadeFrontal) * Time.deltaTime;

            // parando o movimento quando a fruta atingir o chão (y = 0)
            if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                // Destroy(gameObject);
            }
        }
    }

    void InstanciarFrutas()
    {
        int i = (int)UnityEngine.Random.Range(0, frutas.Length);
        Instantiate(frutas[i]);
    }
}