using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using OVR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public GameObject player;
    // public Player player;
    public ScreenFader screenFader;
    public bool initPlayerOnStart;
    [SerializeField] private Transform playerStartTransform;
    [SerializeField] private float levelLoadTime = 1f;
    [SerializeField] private int level = 1;

    public enum GameState
    {
        MainMenu,
        Puzzle,
        LevelCleared
    }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else{
            instance = this;
        }
        gameState = gameState != GameState.MainMenu ? GameState.Puzzle : gameState;
    }

    private void Start()
    {
        if (initPlayerOnStart)
        {
            InitGame();
        }
        screenFader = screenFader != null ? screenFader : FindObjectOfType<ScreenFader>();
        level = SceneManager.GetActiveScene().buildIndex;
    }

    private void InitGame()
    {
        Instantiate(player, playerStartTransform.position, playerStartTransform.rotation);
    }

    public void LevelClear()
    {
        if (gameState  == GameState.LevelCleared)
        {
            return;
        }
        PlayerPrefs.SetInt("level" + (level + 1), 1); // Next level visible in main menu
        GameState previousState = gameState;
        gameState = GameState.LevelCleared;
        if (previousState == GameState.Puzzle)
        {
            LoadNextScene();
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
        StartCoroutine("HandleSceneLoading", SceneManager.GetActiveScene().buildIndex);
    }
    
    public void MainMenu()
    {
        StartCoroutine("HandleSceneLoading", 0);
    }
    
    IEnumerator HandleSceneLoading(int index)
    {
        screenFader.DoFadeIn();
        yield return new WaitForSeconds(levelLoadTime);
        SceneManager.LoadScene(index);
    }

}
