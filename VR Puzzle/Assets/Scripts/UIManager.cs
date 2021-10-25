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
    [SerializeField] private GameObject menu;

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
        shadowChecker = shadowChecker != null ? shadowChecker : FindObjectOfType<ShadowChecker>();
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.LevelCleared)
        {
            matchingText.text = "Level Cleared!";
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
        if (menu.activeInHierarchy)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }
    
    void ShowMenu()
    {
        menu.SetActive(true);
    }
    
    void HideMenu()
    {
        menu.SetActive(false);
    }
    
    public void Restart()
    {
        GameManager.instance.RestartLevel();
    }
    
    public void MainMenu()
    {
        GameManager.instance.MainMenu();
    }
    
    public void Quit()
    {
    }
    
}