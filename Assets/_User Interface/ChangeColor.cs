using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public RawImage imagem;  
    public float velocidade_mudar_cor = 1f;   

    void Update()
    {
        // criando uma transicao de cores pelo espectro (arco-iris)
        float matiz = Mathf.PingPong(Time.time * velocidade_mudar_cor, 1f); // gerando um valor entre 0 e 1 ao longo do tempo
        Color cor = Color.HSVToRGB(matiz, 1f, 1f); // convertendo de HSV para RGB, saturacao e valor são 1 (cores vivas)

        imagem.color = cor;
    }
}
