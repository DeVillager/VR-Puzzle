using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Oculus.Platform.Samples.VrHoops;
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

    public enum GameState
    {
        Puzzle,
        LevelCleared
    }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else{
            instance = this;
        }
        gameState = GameState.Puzzle;
    }

    private void Start()
    {
        if (initPlayerOnStart)
        {
            InitGame();
        }

        screenFader = screenFader != null ? screenFader : FindObjectOfType<ScreenFader>();
    }

    private void InitGame()
    {
        Instantiate(player, playerStartTransform.position, playerStartTransform.rotation);
    }

    public void LevelClear()
    {
        gameState = GameState.LevelCleared;
        screenFader.DoFadeIn();
        StartCoroutine("LoadNextScene");
        // screenFader.StartCoroutine("fadeOutWithDelay", 1f);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(levelLoadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(levelLoadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(levelLoadTime);
        SceneManager.LoadScene("MainMenu");
    }
}
