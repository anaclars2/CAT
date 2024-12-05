using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameManager manager;
    [SerializeField] TouchManager touch;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip[] animationClip;

    [Header("Movimentacao")]
    [SerializeField] float velocidade = 20f;
    float swipe_minimo = 50f; // minimo para considerar um deslize ;D
    Vector3 posicao_alvo;
    Vector3 posicao_base; // posicao inicial, onde define no editor
    [SerializeField] float altura_pulo;

    float timer = 0f;
    float wait_time = 0.5f;
    bool escorregou_pulou = false;

    [Header("Estados")]
    public bool power_invulnerabilidade = false;
    public bool power_ima = false;
    public bool cheat_vida_infinita = false;
    public int machucado = 0;
    public bool em_luna = false;

    [Header("Canvas")]
    [SerializeField] GameObject feedback_machucado;
    [SerializeField] GameObject tela_segunda_chance;
    [SerializeField] GameObject tela_game;

    public bool parar_input_player = false; // quando o player morrer
    // public PlayerDataManager playerDataManager;

    // machucado = 0, normal
    // machucado = 1, lentidao
    // machucado = 2, perdeu

    int contador = 0;
    bool recentemente_machucado = false; // garantir q nao vai receber dano 2x

    // trata-se das meshs para a alteracao de quando estiver montado e nao
    #region EmLuna 
    [Header("Mesh")]
    [SerializeField] GameObject joe_luna;
    [SerializeField] GameObject only_joe;
    #endregion

    public int powerupsPartida = 0;
    public int danoFrutas = 0;
    void Awake()
    {
        danoFrutas = 0;
        powerupsPartida = 0;
        // machucado = 0;

        // para ele comecar com a posicao definida na cena
        posicao_alvo = transform.position;
        posicao_base = transform.position;

        #region PlayerDataJson
        // playerDataManager = new PlayerDataManager();
        // playerDataManager.SalvarDados(this);
        #endregion
    }

    void Update()
    {
        if (em_luna == true)
        {
            joe_luna.SetActive(true);
            only_joe.SetActive(false);
        }
        else
        {
            joe_luna.SetActive(false);
            only_joe.SetActive(true);
        }

        #region Movimentacao
        if (parar_input_player == false)
        {
            // movimentando continuamente o player para frente no eixo z
            Vector3 nova_posicao_z = new Vector3(transform.position.x, transform.position.y, transform.position.z + velocidade * Time.deltaTime);

            // movendo o player diretamente para a nova posicao com velocidade constante no eixo z
            transform.position = nova_posicao_z;

            if (posicao_alvo != transform.position)
            {
                // movendo o player diretamente a posicao alvo com uma velocidade constante
                Vector3 nova_posicao_lateral = new Vector3(posicao_alvo.x, transform.position.y, transform.position.z);

                // continuando movendo o player gradualmente ate chegar na posicao alvo
                // garantindo que o movimento lateral nao afeta o eixo z
                transform.position = Vector3.MoveTowards(transform.position, nova_posicao_lateral, velocidade * Time.deltaTime);
            }
        }

        // verificando se o jogador esta pulando
        if (posicao_alvo.y != transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, posicao_alvo.y, velocidade * Time.deltaTime), transform.position.z);

            if (Mathf.Abs(transform.position.y - posicao_alvo.y) < 0.1f)
            {
                posicao_alvo.y = posicao_base.y;
            }
        }
        #endregion
    }

    public void Movimentacao()
    {
        if (touch != null && parar_input_player == false)
        {
            Vector2 deslize_touch = touch.swipeDirection;

            // verficando se o swipe é significativo o suficiente
            if (deslize_touch.magnitude < swipe_minimo) return;

            // verificando se o swipe no eixo x é maior que no eixo y
            if (Mathf.Abs(deslize_touch.x) > Mathf.Abs(deslize_touch.y) && timer == 0)
            {
                // se o swipe foi maior no eixo y
                if (transform.position.x == 0 || transform.position.x == 3 || transform.position.x == -3)
                {
                    // se o swipe foi na direcao positiva no eixo y
                    if (deslize_touch.y > 0)
                    {
                        // animator e som :X
                        if (em_luna != true)
                        {
                            animator.Rebind();
                            animator.Play("pular");
                        }
                        else
                        {

                        }
                        soundManager.SomPular();

                        posicao_alvo = new Vector3(transform.position.x, altura_pulo, transform.position.z);
                        Debug.Log("Player pulou");
                        escorregou_pulou = true;
                    }
                }
            }
        }
    }

    // coroutina com atraso de 2 segundos
    private IEnumerator AcoesAposMorte()
    {
        yield return new WaitForSeconds(2f);

        // executa as acoes apos o atraso
        soundManager.MusicaSegundaChance();
        feedback_machucado.SetActive(false);
        tela_game.SetActive(false);
        tela_segunda_chance.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Frutas") || other.gameObject.CompareTag("Obstaculo")) && !recentemente_machucado)
        {
            recentemente_machucado = true;
            if (power_invulnerabilidade == false && cheat_vida_infinita == false)
            {
                machucado++;
                feedback_machucado.SetActive(true);
            }

            if (machucado >= 2)
            {
                if (em_luna != true)
                {
                    animator.SetTrigger("morreu");
                }
                else
                {

                }
                soundManager.SomMorrer();
                parar_input_player = true;

                StartCoroutine(AcoesAposMorte());
            }

            if (other.gameObject.CompareTag("Frutas"))
            {
                danoFrutas++;
            }
        }

        #region Pontuacao
        else if (other.gameObject.CompareTag("MoedaPeixe"))
        {
            manager.peixe_moeda++;
            Destroy(other.gameObject);
        }
        #endregion

        #region PowerUps
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            int powerup = (int)UnityEngine.Random.Range(1, 4);
            Debug.Log("powerup random " + powerup);
            powerupsPartida++;

            switch (powerup)
            {
                case 1:
                    // manager.Invulnerabilidade();
                    Destroy(other.gameObject);
                    break;
                case 2:
                    // manager.Ima();
                    Destroy(other.gameObject);
                    break;
                case 3:
                    // manager.BonusSaude();
                    machucado = 0;
                    feedback_machucado.SetActive(false);
                    Debug.Log("PEGOU BONUS SAUDE");
                    Destroy(other.gameObject);
                    break;
            }
        }
        #endregion

        // resetando depois de delay
        Invoke(nameof(ResetDelayMachucado), 2f);
    }

    void ResetDelayMachucado()
    {
        recentemente_machucado = false;
    }
}
