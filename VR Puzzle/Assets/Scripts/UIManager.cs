using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Text matchingText;
    [SerializeField] private ShadowChecker shadowChecker;
    
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else{
            instance = this;
        }
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.LevelCleared)
        {
            matchingText.text = "Level Cleared!";
        }
        else
        {
            matchingText.text = "Hit amount: " + shadowChecker.correctHits;
        }
    }
}

