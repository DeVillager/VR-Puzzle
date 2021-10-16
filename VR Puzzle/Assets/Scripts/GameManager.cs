using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public GameObject player;
    public bool initPlayerOnStart;
    [SerializeField] private Transform playerStartTransform;
    
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
        if (initPlayerOnStart) InitGame();
    }

    private void InitGame()
    {
        Instantiate(player, playerStartTransform.position, playerStartTransform.rotation);
    }

    public void LevelClear()
    {
        gameState = GameState.LevelCleared;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
