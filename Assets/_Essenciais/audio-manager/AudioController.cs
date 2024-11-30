using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // controlando a troca de musicas e dos efeitos sonoros

    // referenciando os audios sources, em outras palavras, os caras que sao fonte de som
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // array dos sons
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] sfx;

    // criando um singleton para o audio ser uma instancia global e unica
    // public static AudioController Singleton;

    private void Awake()
    {
        /*  if (Singleton == null)
          {
              // se nao tiver singleton ativo, entao vai atirar
              // se tiver vai destruir o "extra" para que eu seja unico
              Singleton = this;
          }
          else
          {
              Destroy(gameObject);
          }

          // transformando o singleton ativo nao seja destruido ao mudar de cena
          DontDestroyOnLoad(gameObject);
              */
    }

    // tocando a musica e os efeitos sonoros
    public void TocarMusic(int index)
    {
        // acessando a musica no array
        AudioClip clip = music[index];

        // audio source parar de tocar musica
        musicSource.Stop();

        // atribuindo a musica para o audio source
        musicSource.clip = clip;

        // tocar a musica e em modo de loop
        musicSource.loop = true;
        musicSource.Play();
    }

    public void TocarSFX(int index)
    {
        // acessando o efeito no array
        AudioClip clip = sfx[index];

        // tocar o efeito uma vez so
        sfxSource.PlayOneShot(clip);
    }
}