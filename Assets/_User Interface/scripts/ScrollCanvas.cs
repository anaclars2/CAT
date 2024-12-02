using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCanvas : MonoBehaviour
{
    public RawImage imagem;
    public float x, y;

    void Update()
    {
        imagem.uvRect = new Rect(imagem.uvRect.position + new Vector2(x,y) * Time.deltaTime, imagem.uvRect.size);
    }
}
