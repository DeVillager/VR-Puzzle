using System.Collections;
using BNG;
using OVR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public ScreenFader screenFader;
    [SerializeField] private float levelLoadTime = 1f;
    [SerializeField] private int level = 1;
    [SerializeField] private SoundFXRef levelClearSound;
    [SerializeField] private SoundFXRef quitSound;

    public enum GameState
    {
        MainMenu,
        Puzzle,
        LevelCleared
    }

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
        gameState = gameState != GameState.MainMenu ? GameState.Puzzle : gameState;
    }

    private void Start()
    {
        screenFader = screenFader != null ? screenFader : FindObjectOfType<ScreenFader>();
        level = SceneManager.GetActiveScene().buildIndex;
    }

    public void LevelClear()
    {
        if (gameState == GameState.LevelCleared)
        {
            return;
        }

        PlayerPrefs.SetInt("level" + (level + 1), 1); // Next level visible in main menu
        GameState previousState = gameState;
        gameState = GameState.LevelCleared;
        levelClearSound.PlaySound();
        if (previousState == GameState.Puzzle)
        {
            if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
            {
                MainMenu();
            }
            else
            {
                LoadNextScene();
            }
        }
        else // MainMenu
        {
            MainMenuUI.instance.ShowPlayButton();
        }
    }

    public void LoadLevel(int index)
    {
        StartCoroutine("HandleSceneLoading", index);
    }

    public void LoadNextScene()
    {
        StartCoroutine("HandleSceneLoading", SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        quitSound.PlaySound();
        StartCoroutine("HandleSceneLoading", SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        quitSound.PlaySound();
        StartCoroutine("HandleSceneLoading", 0);
    }

    IEnumerator HandleSceneLoading(int index)
    {
        screenFader.DoFadeIn();
        yield return new WaitForSeconds(levelLoadTime);
        SceneManager.LoadScene(index);
    }
}