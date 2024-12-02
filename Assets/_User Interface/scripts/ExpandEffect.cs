using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandEffect : MonoBehaviour
{
    public Vector3 expandScale = new Vector3(1.5f, 1.5f, 1.5f); 
    public float animationDuration = 0.5f; 
    private Vector3 originalScale; 
    private bool isAnimating = false; 

    void Start()
    {
        originalScale = transform.localScale; // Salvar a escala original
    }

    public void OnButtonClick()
    {
        if (!isAnimating) // Evitar múltiplas animações simultâneas
            StartCoroutine(ExpandAndShrink());
    }

    private System.Collections.IEnumerator ExpandAndShrink()
    {
        isAnimating = true;
        float elapsedTime = 0;

        // Expansão
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, expandScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = expandScale;

        // Retorno ao tamanho original
        elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(expandScale, originalScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;

        isAnimating = false;
    }
}