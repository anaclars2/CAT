using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationYourSelf : MonoBehaviour
{
    static float velocidadeRotacao = 120f;
    private float rotacaoAtual = 0f;

    void Update()
    {
        float rotationStep = velocidadeRotacao * Time.deltaTime;
        rotacaoAtual = (rotacaoAtual + rotationStep) % 360f;
        transform.rotation = Quaternion.Euler(0f, rotacaoAtual, 0f);
    }
}
