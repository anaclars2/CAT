using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
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
        DontDestroyOnLoad(gameObject);

        if (GameObject.Find("slider-master") == true && boolMaster == false)
        {
            slider_volume_master = GameObject.Find("slider-master").GetComponent<Slider>();
            slider_volume_master.value = PlayerPrefs.GetFloat("volumeMASTERKey", 0.75f);
            boolMaster = true;
        }
        if (GameObject.Find("slider-sfx") == true && boolSfx == false)
        {
            slider_volume_sfx = GameObject.Find("slider-sfx").GetComponent<Slider>();
            slider_volume_sfx.value = PlayerPrefs.GetFloat("VolumeSFXKey", 0.75f);
            boolSfx = true;
        }

        if (GameObject.Find("slider-musica") == true && boolMusic == false)
        {
            slider_volume_musics = GameObject.Find("slider-musica").GetComponent<Slider>();
            slider_volume_musics.value = PlayerPrefs.GetFloat("volumeMUSICSKey", 0.75f);
            boolMusic = true;
        }
    }

    private void Update()
    {
        // Debug.Log(boolMaster);
        if (GameObject.Find("slider-master") == true && boolMaster == false)
        {
            slider_volume_master = GameObject.Find("slider-master").GetComponent<Slider>();
            slider_volume_master.value = PlayerPrefs.GetFloat("volumeMASTERKey", 0.75f);
            boolMaster = true;
        }
        if (GameObject.Find("slider-sfx") == true && boolSfx == false)
        {
            slider_volume_sfx = GameObject.Find("slider-sfx").GetComponent<Slider>();
            slider_volume_sfx.value = PlayerPrefs.GetFloat("VolumeSFXKey", 0.75f);
            boolSfx = true;
        }

        if (GameObject.Find("slider-musica") == true && boolMusic == false)
        {
            slider_volume_musics = GameObject.Find("slider-musica").GetComponent<Slider>();
            slider_volume_musics.value = PlayerPrefs.GetFloat("volumeMUSICSKey", 0.75f);
            boolMusic = true;
        }
    }
    void Start()
    {
        // carregando os valores salvos ou definindo os valores padrao
        if (slider_volume_master != null)
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
        }

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
        PlayerPrefs.SetFloat("volumeMASTERKey", volume);
        Debug.Log("setvolumeMaster: " + volume);
    }

    public void SetVolumeMusic(float volume)
    {
        audio_mixer.SetFloat("VolumeMUSICS", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeMUSICSKey", volume);
        Debug.Log("setvolumeMusic: " + volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audio_mixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VolumeSFXKey", volume);
        Debug.Log("setvolumeSfx: " + volume);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}

