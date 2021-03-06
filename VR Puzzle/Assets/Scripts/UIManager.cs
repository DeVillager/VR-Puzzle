using System.Collections;
using System.Collections.Generic;
using OVR;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI matchingText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private ShadowChecker shadowChecker;
    [SerializeField] private RectMask2D optionsMask;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SoundFXRef optionsSound;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

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
        optionsMask.enabled = true;
        shadowChecker = shadowChecker != null ? shadowChecker : FindObjectOfType<ShadowChecker>();
        SetLevelText();
        musicSlider.value = MusicManager.instance.musicLevel;
        soundSlider.value = MusicManager.instance.soundLevel;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.LevelCleared)
        {
            matchingText.text = "Level Cleared!";
        }
        else if (matchingText != null)
        {
            int percentage = Mathf.RoundToInt((float)shadowChecker.correctHits / shadowChecker.winAmount * 100);
            matchingText.text = Mathf.Max(0, percentage) + "%";
        }
    }
    
    public void Options()
    {
        optionsSound.PlaySound();
        Debug.Log("Options opened");
        optionsMask.enabled = !optionsMask.enabled;
    }

    public void Restart()
    {
        GameManager.instance.RestartLevel();
    }

    public void Quit()
    {
        GameManager.instance.MainMenu();
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
    
    public void SetLevelText()
    {
        levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
    }
}