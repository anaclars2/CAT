using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculosTrigger : MonoBehaviour
{
    // ideia do script:
    // utiliza de triggers para auxiliar na deteccao de quando os obstaculos dinamicos
    // devem ocorrer atraves da deteccao do player

    [Header("Trator")]
    [SerializeField] bool trator = false;
    [SerializeField] Animator animatorTrator;

    [Header("Arvore")]
    [SerializeField] bool arvore = false;
    [SerializeField] Animator animatorArvore;

    private void Start()
    {
        if (arvore == true)
        {
            animatorArvore.SetTrigger("cair");
        }
        else if (trator == true)
        {
            animatorTrator.SetTrigger("mover");
        }
    }
}
