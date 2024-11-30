using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System;

public class DialogueSystem : MonoBehaviour
{
    int interlocutor; // se 1 = joe, se 2 = luna, se 3 = narrador
    string nome;
    string dialogo;

    [Header("Elementos UI")]
    public TMP_Text t_dialogo;
    public TMP_Text t_nome;
    public GameObject gameobject_dialogo;

    [Header("Animacoes")]
    public Sprite[] animacoes_luna;
    public Sprite[] animacoes_joe;
    public Image image; // onde deve passar as animacoes dos personagem
    public int indice_animacao1;
    public int indice_animacao2;

    [Header("Audio")]
    public AudioClip[] sons_joe;
    public AudioClip[] sons_luna;
    public AudioClip[] narrador;
    public AudioSource audioSource;
    [HideInInspector] public int indice_fala_som;

    // efeito de maquina de escrever
    float delay = 0.06f;
    string texto_atual = "";
    int indice_char_atual = 0;
    float timer = 0;
    bool parar_som = false;
    bool efeito_maquina = true;

    // efeito falar da animacao
    float delay_sprite = 0.1f;
    float timer_sprite = 0;
    bool sprite1 = false;
    bool mudar_sprite = true;

    private void Update()
    {
        // efeito maquina de escrever e audio
        if (efeito_maquina && indice_char_atual < texto_atual.Length)
        {
            timer = timer + Time.deltaTime;

            if (timer >= delay)
            {
                // adicionando o proximo caractere
                t_dialogo.text = t_dialogo.text + texto_atual[indice_char_atual];

                // avancando para o proximo caractere
                indice_char_atual++;
                timer = 0;

                if (parar_som == false && audioSource.isPlaying == false)
                {
                    if (interlocutor == 1 && sons_joe.Length > 0)
                    {
                        /*// audio source receber som joe
                        // para cada letra tem que sortear um som e mandar pro audio source
                        // tem que ser em loop ao mesmo tempo que as letras aparecem
                        if (audioSource.isPlaying == false)
                        {
                            int som_sorteado = (int)UnityEngine.Random.Range(0, sons_joe.Length);
                            audioSource.clip = sons_joe[som_sorteado];
                        }*/
                        audioSource.clip = sons_joe[indice_fala_som];
                    }
                    else if (interlocutor == 2 && sons_luna.Length > 0)
                    {
                        /*int som_sorteado = (int)UnityEngine.Random.Range(0, sons_luna.Length);
                        audioSource.clip = sons_luna[som_sorteado];*/
                        audioSource.clip = sons_luna[indice_fala_som];
                    }
                    else if (interlocutor == 3 && narrador.Length > 0)
                    {
                        audioSource.clip = narrador[indice_fala_som];
                    }

                    audioSource.Play();
                }
            }
        }
        else
        {
            if (interlocutor == 1) // joe
                image.sprite = animacoes_joe[indice_animacao1];
            else if (interlocutor == 2) // luna
                image.sprite = animacoes_luna[indice_animacao1];
            
            mudar_sprite = false;
        }

        // efeito de falar do personagem
        if (mudar_sprite == true)
        {
            timer_sprite = timer_sprite + Time.deltaTime;
            if (timer_sprite >= delay_sprite)
            {
                timer_sprite = 0;
                sprite1 = !sprite1;
            }

            if (sprite1 == false)
            {
                if (interlocutor == 1) // joe
                    image.sprite = animacoes_joe[indice_animacao2];
                else if (interlocutor == 2) // luna
                    image.sprite = animacoes_luna[indice_animacao2];
            }
            else
            {
                if (interlocutor == 1) // joe
                    image.sprite = animacoes_joe[indice_animacao1];
                else if (interlocutor == 2) // luna
                    image.sprite = animacoes_luna[indice_animacao1];
            }
        }
    }

    public void ProximaFala() // acelere a proxima fase
    {
        if (t_dialogo.text.Length < texto_atual.Length)
        {
            // mostrar o texto completo da fala atual se ele ainda nao estiver completo
            t_dialogo.text = texto_atual;
            audioSource.Stop();
            parar_som = true;
            efeito_maquina = false;
            sprite1 = true;
            mudar_sprite = false;
        }
        else
        {
            // se o texto ja estiver completo, avançar para a proxima fala
            parar_som = false;
            FalaAtual();
            efeito_maquina = true;
            sprite1 = true;
            mudar_sprite = false;
        }
    }

    public void FalaAtual() // chamando os elementos para a fala atual
    {
        t_dialogo.text = ""; // limpando o texto
        indice_char_atual = 0; // resetar para a nova fala
        audioSource.Stop();

        t_nome.text = nome;
        texto_atual = dialogo;
        Debug.Log($"texto_atual: {texto_atual}");
        efeito_maquina = true;
        mudar_sprite = true;
    }

    public void DefinirFalaAtual(string _nome, string _dialogo, int _interlocutor, int _indice_animacao1, int _indice_animacao2)
    {
        nome = _nome;
        dialogo = _dialogo;
        interlocutor = _interlocutor;
        indice_animacao1 = _indice_animacao1;
        indice_animacao2 = _indice_animacao2;
    }
}
