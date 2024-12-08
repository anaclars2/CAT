using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction touchPositionAction;
    InputAction touchPressAction;

    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    [HideInInspector] public Vector2 swipeDirection;
    bool isSwiping; // deslizando :D

    public Player player;
    public CheatManager cheat;

    public void Awake()
    {
        // acessando as acoes do new input system, asset TouchScreen
        // em outras palavras, esta obtendo o componente PlayerInput
        // e acessando as acoes de toque definidas
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    #region Eventos
    // é chamado automaticamente pela Unity
    // quando o gameObject que esta anexado esse script esta ativado
    private void OnEnable()
    {
        // quando o toque for realizado chama TouchStarted()
        touchPressAction.performed += TouchStarted;
        // quando o toque for cancelado/parado chama TouchEnded()
        touchPressAction.canceled += TouchEnded;
    }

    // é chamado automaticamente pela Unity
    // quando o gameObject que esta anexado esse script esta desativado
    private void OnDisable()
    {
        touchPressAction.performed -= TouchStarted;
        touchPressAction.canceled -= TouchEnded;
    }
    #endregion

    // é chamado quando um toque é detectado
    // assim, ele armazena a posicao inicial do toque
    // e define isSwiping como verdadeiro
    private void TouchStarted(InputAction.CallbackContext context)
    {
        startTouchPosition = touchPositionAction.ReadValue<Vector2>();
        isSwiping = true;
    }

    // é chamado quando o toque termina
    // armazena a posicao final do toque e define isSwiping como falso
    // além disso, ele chama DetectSwipe() para verificar se houve um gesto de deslizar
    private void TouchEnded(InputAction.CallbackContext context)
    {
        endTouchPosition = touchPositionAction.ReadValue<Vector2>();
        isSwiping = false;
        DetectSwipe();
    }

    // calculando a direcao do deslizar e verificando se a magnitude do deslizar é maior que 50 unidades
    // se for, ele considera que um deslizar foi detectado, normaliza a direcao do deslizar
    // e desenha uma linha vermelha no mundo 3D entre as posicoes inicial e final do toque
    private void DetectSwipe()
    {
        swipeDirection = endTouchPosition - startTouchPosition;
        if (swipeDirection.magnitude > 20)
        {
            // normalizado é de 0 a 1
            // Debug.Log("Swipe detected: " + swipeDirection.normalized);
            Vector3 startWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(startTouchPosition.x, startTouchPosition.y, 10));
            Vector3 endWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(endTouchPosition.x, endTouchPosition.y, 10));
            Debug.DrawLine(startWorldPosition, endWorldPosition, Color.red, 2.0f);

            // ...

            if (player != null)
            {
                player.Movimentacao();
            }
        }

        // se a magnitude, tamanho do vetor, do touch for menor que 50
        // significa que nao houve deslize
        else
        {
            if (cheat != null)
            {
                cheat.ContabilizarTouchs();
            }
        }
    }
}
