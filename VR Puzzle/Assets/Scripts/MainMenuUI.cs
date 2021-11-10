using OVR;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance;

    [SerializeField] private Animator creditsAnimator;
    [SerializeField] private Animator rulesAnimator;
    [SerializeField] private Animator playButtonAnimator;
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private string description1;
    [SerializeField] private string description2;
    [SerializeField] private RectMask2D hideMask;

    [SerializeField] private ShadowChecker shadowChecker;
    [SerializeField] private TextMeshProUGUI matchingText;
    [SerializeField] private GameObject levels;
    [SerializeField] private SoundFXRef buttonSound;
    [SerializeField] private SoundFXRef optionsSound;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI playButtonText;
    [SerializeField] private int tutorialCleared;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        musicSlider.value = MusicManager.instance.musicLevel;
        soundSlider.value = MusicManager.instance.soundLevel;
        hideMask.enabled = true;
        shadowChecker = shadowChecker != null ? shadowChecker : FindObjectOfType<ShadowChecker>();
        matchingText.raycastTarget = false;
        tutorialCleared = PlayerPrefs.GetInt("tutorial");
        if (tutorialCleared == 1)
        {
            ShowLevels();
        }
        else
        {
            ShowTutorial();
        }
    }

    private void ShowLevels()
    {
        levels.SetActive(true);
        playButtonText.text = "Levels";
        playButtonAnimator.enabled = false;
        shadowChecker.gameObject.SetActive(false);
    }
    
    private void ShowTutorial()
    {
        levels.SetActive(false);
        playButtonText.text = "Play";
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.LevelCleared || tutorialCleared == 1)
        {
            matchingText.text = tutorialCleared == 1 ? "Level selection" : "Play";
            matchingText.raycastTarget = tutorialCleared != 1;
        }
        else
        {
            int percentage = Mathf.RoundToInt((float)shadowChecker.correctHits / shadowChecker.winAmount * 100);
            matchingText.text = Mathf.Max(0, percentage) + "%";
        }
    }

    public void Options()
    {
        Debug.Log("Options opened");
        buttonSound.PlaySound();
        if (!hideMask.enabled)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    public void ShowMenu()
    {
        hideMask.enabled = false;
        description.text = description1;
    }

    void HideMenu()
    {
        hideMask.enabled = true;
        description.text = description2;
    }

    public void ShowCredits()
    {
        buttonSound.PlaySound();
        bool creditsOn = creditsAnimator.GetBool("Credits");
        Debug.Log("creditsOn: " + creditsOn);
        creditsAnimator.SetBool("Credits", !creditsOn);
    }

    public void Play()
    {
        tutorialCleared = 1;
        PlayerPrefs.SetInt("tutorial", tutorialCleared); // Next level visible in main menu
        ShowLevels();
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        buttonSound.PlaySound();
        Application.Quit();
    }

    public void ShowRules()
    {
        buttonSound.PlaySound();
        bool rulesOn = rulesAnimator.GetBool("Rules");
        Debug.Log("rules on: " + rulesOn);
        rulesAnimator.SetBool("Rules", !rulesOn);
    }

    public void ShowPlayButton()
    {
        playButtonAnimator.SetTrigger("Play");
    }

    public void SetMusicVolume(float volume)
    {
        MusicManager.instance.musicLevel = volume;
        optionsSound.PlaySound();
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.0001f + (volume / 10)) * 20);
    }

    public void SetSoundsVolume(float volume)
    {
        MusicManager.instance.soundLevel = volume;
        optionsSound.PlaySound();
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(0.0001f + (volume / 10)) * 20);
    }
}