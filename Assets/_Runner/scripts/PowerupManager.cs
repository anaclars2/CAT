using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Numerics;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] Player player;

    // power ups
    float invulnerabilidade_timer = 0f, ima_timer = 0f, bonussaude_timer = 0f;
    bool iv_timer_ativar = false, im_timer_ativar = false, bs_timer_ativar = false;
    float duracao_efeito = 7f;

    [Header("Elementos UI")]
    [SerializeField] GameObject timer_powerup; // objeto do gameObject
    [SerializeField] TMP_Text nome_powerup;
    [SerializeField] Image fill_powerup;


    public void Invulnerabilidade() // se o timer do invulnerabilidade tiver ativado
    {
        DesativarPowerUps(); // desativando qualquer power-up anterior

        if (iv_timer_ativar)
        {
            invulnerabilidade_timer = 0f;
            fill_powerup.fillAmount = 1;
        }
        else
        {
            Debug.Log("invulnerabilidade manager");
            player.power_invulnerabilidade = true;
            iv_timer_ativar = true;
            timer_powerup.SetActive(true);
            AtivarPowerUp();
            nome_powerup.text = "INVULNERABILIDADE";
        }
    }

    public void Ima() // se for chamado significa que esta ativado
    {
        DesativarPowerUps(); // desativando qualquer power-up anterior

        if (im_timer_ativar)
        {
            // reiniciando o timer se pegar outro coletavel igual
            // enquanto ja esta com uma primeira ativa
            ima_timer = 0f;
            fill_powerup.fillAmount = 1;
        }
        else
        {
            Debug.Log("ima manager");
            player.power_ima = true;
            im_timer_ativar = true;
            timer_powerup.SetActive(true);
            AtivarPowerUp();
            nome_powerup.text = "IMA";
        }
    }

    public void BonusSaude()
    {
        DesativarPowerUps(); // desativando qualquer power-up anterior

        if (bs_timer_ativar)
        {
            bonussaude_timer = 0f;
            fill_powerup.fillAmount = 1;
        }
        else
        {
            Debug.Log("bonus saude manager");
            bs_timer_ativar = true;
            timer_powerup.SetActive(true);
            AtivarPowerUp();
            nome_powerup.text = "BONUS SAUDE";
        }
    } // se for chamado significa que esta ativado

    void DesativarPowerUps()
    {
        // desativando todos os timers e resetando os efeitos
        iv_timer_ativar = false;
        im_timer_ativar = false;
        bs_timer_ativar = false;

        player.power_invulnerabilidade = false;
        player.power_ima = false;

        // resetando os timers
        invulnerabilidade_timer = 0f;
        ima_timer = 0f;
        bonussaude_timer = 0f;

        // desativando a UI do power-up
        DesativarPowerUp();
        timer_powerup.SetActive(false);
        fill_powerup.fillAmount = 1;
    }

    private void Update()
    {
        Timers();
    }

    void Timers()
    {
        if (fill_powerup.fillAmount != 0 && fill_powerup.IsActive() == true)
        {
            fill_powerup.fillAmount -= Time.deltaTime / duracao_efeito;
        }
        else if (fill_powerup.fillAmount == 0)
        {
            fill_powerup.fillAmount = 1;
            timer_powerup.SetActive(false);
            Debug.Log("timer acabou sprite");
        }

        if (iv_timer_ativar == true) // se o timer do invulnerabilidade tiver ativado
        {
            if (invulnerabilidade_timer >= 0f)
            {
                invulnerabilidade_timer += Time.deltaTime;
                if (invulnerabilidade_timer >= duracao_efeito)
                {
                    player.power_invulnerabilidade = false;

                    // resetando o timer
                    invulnerabilidade_timer = 0f;
                    iv_timer_ativar = false;
                }
            }
        }

        if (bs_timer_ativar == true) // se o timer do bonus saude tiver ativado
        {
            if (bonussaude_timer >= 0f)
            {
                bonussaude_timer += Time.deltaTime;
                if (bonussaude_timer >= duracao_efeito)
                {
                    // resetando o timer
                    bonussaude_timer = 0f;
                    bs_timer_ativar = false;
                }
            }
        }

        if (im_timer_ativar == true) // se o timer do ima tiver ativado
        {
            if (ima_timer >= 0f)
            {
                ima_timer += Time.deltaTime;
                if (ima_timer >= duracao_efeito)
                {
                    player.power_ima = false;

                    // resetando o timer
                    ima_timer = 0f;
                    im_timer_ativar = false;
                }
            }
        }
    }

    void AtivarPowerUp()
    {
        StartCoroutine(MoverTimerPowerUp(-438)); // destino: -438
    }

    void DesativarPowerUp()
    {
        StartCoroutine(MoverTimerPowerUp(-849)); // destino: -849
    }

    IEnumerator MoverTimerPowerUp(float destinoX)
    {
        UnityEngine.Vector2 posInicial = timer_powerup.transform.localPosition;
        UnityEngine.Vector2 posFinal = new UnityEngine.Vector2(destinoX, posInicial.y);

        float duracao = 0.5f; // duracao da animacao em segundos
        float tempo = 0;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            timer_powerup.transform.localPosition = UnityEngine.Vector2.Lerp(posInicial, posFinal, tempo / duracao);
            yield return null; // esoerando o proximo frame
        }

        // garatindo a posicao final exata
        timer_powerup.transform.localPosition = posFinal;
    }
}
