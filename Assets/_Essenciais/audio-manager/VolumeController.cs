using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour, IDataPersistence
{
    public AudioMixer audio_mixer;
    public Slider slider_volume_master;
    public Slider slider_volume_musics;
    public Slider slider_volume_sfx;
    bool boolMaster = false, boolMusic = false, boolSfx = false;


    [HideInInspector] public static VolumeController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        IniciarSliders();
    }

    void IniciarSliders()
    {
        // if (GameObject.Find("slider-musica") == true && boolMusic == false)
        if (boolMaster == false)
        {
            slider_volume_master = FindSliderByName("slider-master");
            // slider_volume_master.value = PlayerPrefs.GetFloat("volumeMASTERKey", 0.75f);
            boolMaster = true;

            // slider_volume_master = GameObject.Find("slider-master").GetComponent<Slider>();
        }
        if (boolSfx == false)
        {
            slider_volume_sfx = FindSliderByName("slider-sfx");
            // slider_volume_sfx.value = PlayerPrefs.GetFloat("VolumeSFXKey", 0.75f);
            boolSfx = true;
        }
        if (boolMusic == false)
        {
            slider_volume_musics = FindSliderByName("slider-musica");
            // slider_volume_musics.value = PlayerPrefs.GetFloat("volumeMUSICSKey", 0.75f);
            boolMusic = true;
        }
    }

    private Slider FindSliderByName(string name)
    {
        // buscando em todos os GameObjects do tipo slider, incluindo inativos
        Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();

        foreach (Slider slider in allSliders)
        {
            if (slider.gameObject.name == name)
            {
                return slider;
            }
        }

        return null; // retorna null se o objeto não for encontrado
    }

    private void Update()
    {
        // Debug.Log(boolMaster);
        if (boolMaster == false || boolMusic == false || boolSfx == false)
        {
            IniciarSliders();
        }
        else
        {
            ConferirCena(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Start()
    {
        // carregando os valores salvos ou definindo os valores padrao
        /*if (slider_volume_master != null)
        {
            slider_volume_master.value = PlayerPrefs.GetFloat("volumeMASTERKey", 0.75f); // se o volume é de -80db a 20db, entao 0 é 75%
            boolMaster = true;
        }
        if (slider_volume_musics != null)
        {
            slider_volume_musics.value = PlayerPrefs.GetFloat("volumeMUSICSKey", 0.75f);
            boolMusic = true;
        }
        if (slider_volume_sfx != null)
        {
            slider_volume_sfx.value = PlayerPrefs.GetFloat("VolumeSFXKey", 0.75f);
            boolSfx = true;
        }*/

        // aplicando os valores salvos no AudioMixer

        // .AddListener é um metodo que chama automaticamente SetVolume...
        // quando registra alteracoes num evento, que nesse caso é o .onValueChanged
        // ou seja, quando o valor do slider que determina o volume mudar
        if (slider_volume_master != null)
        {
            SetVolumeMaster(slider_volume_master.value);
            slider_volume_master.onValueChanged.AddListener(SetVolumeMaster);
        }
        if (slider_volume_musics != null)
        {
            SetVolumeMusic(slider_volume_musics.value);
            slider_volume_musics.onValueChanged.AddListener(SetVolumeMusic);
        }
        if (slider_volume_sfx != null)
        {
            SetVolumeSFX(slider_volume_sfx.value);
            slider_volume_sfx.onValueChanged.AddListener(SetVolumeSFX);
        }
    }

    public void SetVolumeMaster(float volume)
    {
        // convertendo o valor linear do slider (0.0001 a 1) para uma escala logaritmica, que é mais adequada para o controle de volume
        // Mathf.Log10(volume) calcula o logaritmo base 10 do valor do slider
        // por fim, multiplicar por 20 ajusta a escala para decibéis(dB), que é a unidade usada pelo AudioMixer para controlar o volume
        audio_mixer.SetFloat("VolumeMASTER", Mathf.Log10(volume) * 20);

        // salvando o valor no PlayerPrefs
        // PlayerPrefs.SetFloat("volumeMASTERKey", volume);
        // Debug.Log("setvolumeMaster: " + volume);
    }

    public void SetVolumeMusic(float volume)
    {
        audio_mixer.SetFloat("VolumeMUSICS", Mathf.Log10(volume) * 20);
        // PlayerPrefs.SetFloat("volumeMUSICSKey", volume);
        // Debug.Log("setvolumeMusic: " + volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audio_mixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
        // PlayerPrefs.SetFloat("VolumeSFXKey", volume);
        // Debug.Log("setvolumeSfx: " + volume);
    }

    public void LoadData(GameData data)
    {
        if (data != null)
        {
            if (slider_volume_master != null)
            {
                // se os valores nao foram salvos antes, inicialize com o valor padrao de 0.75f
                float masterVolume = data.volumeMaster != 0 ? data.volumeMaster : 0.75f;
                slider_volume_master.value = masterVolume;
                SetVolumeMaster(data.volumeMaster);
            }
            if (slider_volume_musics != null)
            {
                float musicVolume = data.volumeMusics != 0 ? data.volumeMusics : 0.75f;
                slider_volume_musics.value = musicVolume;
                SetVolumeMusic(data.volumeMusics);
            }
            if (slider_volume_sfx != null)
            {
                float sfxVolume = data.volumeSfx != 0 ? data.volumeSfx : 0.75f;
                slider_volume_sfx.value = sfxVolume;
                SetVolumeSFX(data.volumeSfx);
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (slider_volume_master != null)
        {
            data.volumeMaster = slider_volume_master.value;
        }
        if (slider_volume_musics != null)
        {
            data.volumeMusics = slider_volume_musics.value;
        }
        if (slider_volume_sfx != null)
        {
            data.volumeSfx = slider_volume_sfx.value;
        }
    }

    void ConferirCena(int indice)
    {
        Debug.Log("indiceCena: " + indice);
        if (indice == 1) // significa que é o runner
        {
            boolMaster = false;
            boolMusic = false;
            boolSfx = false;
        }
    }
}

