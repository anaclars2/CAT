using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audio_mixer;
    public Slider slider_volume_master;
    public Slider slider_volume_musics;
    public Slider slider_volume_sfx;

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
        Debug.Log("Inicializando VolumeController");

        IniciarSliders();
        CarregarVolumes();
    }

    void IniciarSliders()
    {
        if (slider_volume_master == null)
        {
            slider_volume_master = FindSliderByName("slider-master");
        }
        if (slider_volume_sfx == null)
        {
            slider_volume_sfx = FindSliderByName("slider-sfx");
        }
        if (slider_volume_musics == null)
        {
            slider_volume_musics = FindSliderByName("slider-musica");
        }
    }

    private Slider FindSliderByName(string name)
    {
        Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (Slider slider in allSliders)
        {
            if (slider.gameObject.name == name)
            {
                return slider;
            }
        }
        return null;
    }

    private void Update()
    {
        if (slider_volume_master == null || slider_volume_sfx == null || slider_volume_musics == null)
        {
            IniciarSliders();
            CarregarVolumes();
        }
    }

    void Start()
    {
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
        audio_mixer.SetFloat("VolumeMASTER", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeMASTERKey", volume);
        PlayerPrefs.Save();
    }

    public void SetVolumeMusic(float volume)
    {
        audio_mixer.SetFloat("VolumeMUSICS", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeMUSICSKey", volume);
        PlayerPrefs.Save();
    }

    public void SetVolumeSFX(float volume)
    {
        audio_mixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeSFXKey", volume);
        PlayerPrefs.Save();
    }

    void CarregarVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat("volumeMASTERKey", 0.75f);
        float musicVolume = PlayerPrefs.GetFloat("volumeMUSICSKey", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("volumeSFXKey", 0.75f);

        if (slider_volume_master != null)
        {
            slider_volume_master.value = masterVolume;
            SetVolumeMaster(masterVolume);
        }
        if (slider_volume_musics != null)
        {
            slider_volume_musics.value = musicVolume;
            SetVolumeMusic(musicVolume);
        }
        if (slider_volume_sfx != null)
        {
            slider_volume_sfx.value = sfxVolume;
            SetVolumeSFX(sfxVolume);
        }
    }
}
