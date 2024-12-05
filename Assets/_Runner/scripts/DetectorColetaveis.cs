using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorColetaveis : MonoBehaviour
{
    // ideia do script:
    // garantir o powerup do imã, assim quando estiver ativo
    // ele puxa os objetos!!! a lógica é que num primeiro instante 
    // o detector ao colidir com os objetos os coloca em um vetor
    // e os puxa ate o player (p.s.: que destroi eles), por fim
    // quando acaba a colisao ele limpa o vetor para nao dar erros errantes

    public Player player;
    public Transform transform_player;
    float velocidade_objeto = 25f; // velocidade com que os objetos sao puxados
    
    int vetor_tamanho = 10;
    Transform[] objetos_puxados;
    int numero_objetos = 0;

    private void Start()
    {
        // procurando o player na cena e pegando o transform dele
        transform_player = GameObject.FindGameObjectWithTag("Player").transform;
        objetos_puxados = new Transform[vetor_tamanho];
    }

    private void OnTriggerEnter(Collider other)
    {
        // detecta peca grande da nave e poeira estelar
        // if ((other.gameObject.CompareTag("PecaGrande") || other.gameObject.CompareTag("PoeiraEstelar")))
        if (player.power_ima == true && (other.gameObject.CompareTag("PeixeMoeda")))
        {
            // adicionando o objeto ao vetor se ainda houver espaço
            if (numero_objetos < vetor_tamanho)
            {
                objetos_puxados[numero_objetos] = other.transform;
                numero_objetos++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // removendo o objeto do vetor
        Transform objeto_remover = other.transform;

        for (int i = 0; i < numero_objetos; i++)
        {
            if (objetos_puxados[i] == objeto_remover)
            {
                // movendo todos os objetos após o removido uma posicao para a esquerda
                // isso garante que nao vai dar erro de referencia
                for (int j = i; j < numero_objetos - 1; j++)
                {
                    objetos_puxados[j] = objetos_puxados[j + 1];
                }

                // limpando a ultima poiscao e ajustando o numero de objetos
                objetos_puxados[numero_objetos - 1] = null;
                numero_objetos--;

                break; // saindo do loop depoiss que encontra e remove o objeto
            }
        }
    }

    void Update()
    {
        // verificando se tem um objeto para puxar
        // if (numero_objetos > 0 && player.power_ima == true)
        if (numero_objetos > 0)
        {
            for (int i = numero_objetos - 1; i >= 0; i--)
            {
                Transform objeto_puxado = objetos_puxados[i];
                if (objeto_puxado != null)
                {
                    // movendo o objeto na direcao do player
                    objeto_puxado.position = Vector3.MoveTowards(objeto_puxado.position, transform_player.position, velocidade_objeto * Time.deltaTime);

                    // se o objeto estiver muito perto do jogador, entao para de puxar
                    if (Vector3.Distance(objeto_puxado.position, transform_player.position) < 0.1f)
                    {
                        // removendo o objeto do vetor e ajustando a contagem
                        objetos_puxados[i] = objetos_puxados[numero_objetos - 1];
                        objetos_puxados[numero_objetos - 1] = null;
                        numero_objetos--;
                    }
                }
                else
                {
                    // removendo objetos nulos do vetor e ajustando a contagem
                    objetos_puxados[i] = objetos_puxados[numero_objetos - 1];
                    objetos_puxados[numero_objetos - 1] = null;
                    numero_objetos--;
                }
            }
        }
    }
}

