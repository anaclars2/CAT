using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameManager manager;
    [SerializeField] PowerupManager powerManager;
    [SerializeField] TouchManager touch;

    [Header("Animacao")]
    [SerializeField] Animator[] animator;
    [SerializeField] Animator animatorLuna;
    // public AnimationClip[] animationClip;

    [Header("Movimentacao")]
    [SerializeField] float velocidade = 20f;
    float swipe_minimo = 50f; // minimo para considerar um deslize ;D
    Vector3 posicao_alvo, posicao_base;
    public float altura_pulo;

    float timer = 0f;
    float wait_time = 0.5f;
    bool escorregou_pulou = false;

    [Header("Estados")]
    public bool power_invulnerabilidade = false;
    public bool power_ima = false;
    public bool cheat_vida_infinita = false;
    public int machucado = 0;
    public bool montariaLuna = false;
    bool recentemente_machucado = false; // garantir q nao vai receber dano 2x
    public bool parar_input_player = false; // quando o player morrer

    [Header("Elementos UI")]
    [SerializeField] GameObject tela_info;
    [SerializeField] GameObject tela_game;
    [SerializeField] TMP_Text texto_peixeMoeda;
    [SerializeField] TMP_Text texto_peixeMoedaInfo;
    [SerializeField] TMP_Text texto_powerupInfo;

    [Header("Mesh")]
    // public GameObject lunaMesh;

    [Header("Player morreu")]
    [SerializeField] BackgroundScroller[] backScroller = new BackgroundScroller[2];

    [HideInInspector] public int danoFrutas = 0;
    [HideInInspector] public bool modoFrutas = false;

    int peixeMoedasPartida = 0;
    void Awake()
    {
        // retornando tudo p valor original
        parar_input_player = false;
        ObjectManager.moveSpeed = 20f;
        backScroller[0].scrollSpeed = 0.5f;
        backScroller[1].scrollSpeed = 0.5f;

        tela_game.SetActive(true);
        tela_info.SetActive(false);
        danoFrutas = 0;
        machucado = 0;

        // para ele comecar com a posicao definida na cena
        posicao_alvo = transform.position;
        posicao_base = transform.position;

        if (manager == null)
        {
            manager = GameObject.Find("GeneralManager").GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if (manager == null)
        {
            manager = GameObject.Find("GeneralManager").GetComponent<GameManager>();
        }

        texto_peixeMoeda.text = peixeMoedasPartida.ToString();
        if (texto_peixeMoedaInfo.gameObject.activeInHierarchy == true)
        {
            texto_peixeMoedaInfo.text = peixeMoedasPartida.ToString();
        }
        if (texto_powerupInfo.gameObject.activeInHierarchy == true)
        {
            texto_powerupInfo.text = manager.powerupsPartida.ToString();
        }

        /* if (montariaLuna == true)
         {
             lunaMesh.SetActive(true);
         }
         else
         {
             lunaMesh.SetActive(false);
         }*/

        MovimentacaoUpdate();
    }

    public void MovimentacaoUpdate()
    {
        #region Movimentacao
        if (!parar_input_player)
        {
            // mantendo a posicao no eixo z fixa
            transform.position = new Vector3(transform.position.x, transform.position.y, posicao_base.z);

            if (posicao_alvo != transform.position)
            {
                // movendo o player diretamente a posição alvo no eixo x e y
                Vector3 nova_posicao = new Vector3(posicao_alvo.x, Mathf.MoveTowards(transform.position.y, posicao_alvo.y, velocidade * Time.deltaTime), posicao_base.z);
                transform.position = Vector3.MoveTowards(transform.position, nova_posicao, velocidade * Time.deltaTime);

                // travando o valor de X em posicoes fixas
                if (Mathf.Abs(posicao_alvo.x - transform.position.x) < 0.1f)
                {
                    if (transform.position.x > 1.5f)
                    {
                        transform.position = new Vector3(3, transform.position.y, posicao_base.z);
                    }
                    else if (transform.position.x < -1.5f)
                    {
                        transform.position = new Vector3(-3, transform.position.y, posicao_base.z);
                    }
                    else
                    {
                        transform.position = new Vector3(0, transform.position.y, posicao_base.z);
                    }
                }
            }
        }
        #endregion

        #region Timer eixo y
        // logica do timer para escorregar ou pular
        if (escorregou_pulou)
        {
            if (timer >= 0f)
            {
                timer += Time.deltaTime;
                if (timer >= wait_time)
                {
                    // forcando retorno ao chao
                    posicao_alvo = new Vector3(transform.position.x, posicao_base.y, posicao_base.z);
                    escorregou_pulou = false;

                    // resetando o timer
                    timer = 0f;
                }
            }
        }

        // garantindo que o jogador nao fique no ar apos o tempo de pulo
        if (!escorregou_pulou && Mathf.Abs(transform.position.y - posicao_base.y) > 0.1f)
        {
            transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, posicao_base.y, velocidade * Time.deltaTime), posicao_base.z);
        }
        #endregion
    }
    public void Movimentacao()
    {
        if (touch != null && !parar_input_player)
        {
            Vector2 deslize_touch = touch.swipeDirection;

            // verificando se o swipe é significativo o suficiente
            if (deslize_touch.magnitude < swipe_minimo) return;

            if (Mathf.Abs(deslize_touch.x) > Mathf.Abs(deslize_touch.y) && timer == 0)
            {
                posicao_alvo.y = posicao_base.y;

                // deslize na direcao positiva no eixo x
                if (deslize_touch.x > 0)
                {
                    switch (transform.position.x)
                    {
                        case -3:
                            posicao_alvo = new Vector3(0, transform.position.y, posicao_base.z);
                            break;
                        case 0:
                            posicao_alvo = new Vector3(3, transform.position.y, posicao_base.z);
                            break;
                    }
                }
                // deslize na direcao negativa no eixo x
                else if (deslize_touch.x < 0)
                {
                    switch (transform.position.x)
                    {
                        case 3:
                            posicao_alvo = new Vector3(0, transform.position.y, posicao_base.z);
                            break;
                        case 0:
                            posicao_alvo = new Vector3(-3, transform.position.y, posicao_base.z);
                            break;
                    }
                }
            }
            // se o deslize for maior no eixo y
            else if (transform.position.x == 0 || transform.position.x == 3 || transform.position.x == -3)
            {
                if (deslize_touch.y > 0 && !escorregou_pulou)
                {
                    soundManager.SomPular();
                    posicao_alvo = new Vector3(transform.position.x, altura_pulo, posicao_base.z);
                    escorregou_pulou = true;
                    if (manager.lunaCorrendo != true)
                    {
                        foreach (Animator a in animator)
                        {
                            if (a.gameObject.activeInHierarchy == true)
                                a.SetTrigger("pulou");
                        }
                    }
                    else
                    {
                        animatorLuna.SetTrigger("pulou");
                    }
                }
            }
        }
    }

    // coroutina com atraso de 2 segundos
    private IEnumerator AcoesAposMorte()
    {
        yield return new WaitForSeconds(3f);

        // executa as acoes apos o atraso
        soundManager.MusicaSegundaChance();
        tela_game.SetActive(false);
        tela_info.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Fruta") || other.gameObject.CompareTag("Obstaculo")) && !recentemente_machucado)
        {
            recentemente_machucado = true;
            if (power_invulnerabilidade == false && cheat_vida_infinita == false)
            {
                /* foreach (Animator a in animator)
                 {
                     a.SetTrigger("dano");

                 }*/
                machucado++;
                soundManager.SomDano();
            }

            if (machucado >= 2)
            {
                if (montariaLuna != true)
                {
                    if (manager.lunaCorrendo != true)
                    {
                        foreach (Animator a in animator)
                        {
                            a.SetTrigger("morreu");
                        }
                    }
                    else
                    {
                        animatorLuna.SetTrigger("morreu");
                    }

                }
                else
                {

                }

                // se ele morrer 
                if (manager.lunaCorrendo != true)
                {
                    soundManager.SomMorrerJoe();
                } 
                else
                {
                    soundManager.SomMorrerLuna();
                }

                parar_input_player = true;
                ObjectManager.moveSpeed = 0f;
                backScroller[0].scrollSpeed = 0f;
                backScroller[1].scrollSpeed = 0f;

                StartCoroutine(AcoesAposMorte());
            }

            if (other.gameObject.CompareTag("Fruta"))
            {
                danoFrutas++;
            }
        }

        #region Pontuacao
        else if (other.gameObject.CompareTag("PeixeMoeda"))
        {
            manager.peixe_moeda++;
            peixeMoedasPartida++;
            soundManager.SomPeixeMoeda();
            Destroy(other.gameObject);
        }
        #endregion

        #region PowerUps
        else if (other.gameObject.CompareTag("PowerUp"))
        {
            int powerup = (int)UnityEngine.Random.Range(1, 4);
            manager.powerupsPartida++;
            manager.powerups++;

            switch (powerup)
            {
                case 1:
                    powerManager.Invulnerabilidade();
                    Destroy(other.gameObject);
                    break;

                case 2:
                    powerManager.Ima();
                    Destroy(other.gameObject);
                    break;

                case 3:
                    powerManager.BonusSaude();
                    machucado = 0;
                    // tela_machucado.SetActive(false);
                    Destroy(other.gameObject);
                    break;
            }
        }
        #endregion

        // resetando depois de delay
        Invoke(nameof(ResetDelayMachucado), 3f);
    }

    void ResetDelayMachucado()
    {
        recentemente_machucado = false;
    }
}