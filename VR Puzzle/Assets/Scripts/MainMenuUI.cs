using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance;
    
    [SerializeField] private Animator creditsAnimator;
    [SerializeField] private Animator rulesAnimator;
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private string description1;
    [SerializeField] private string description2;

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

    public void ShowMenu()
    {
        menu.SetActive(true);
        description.text = description1;
    }
    
    void HideMenu()
    {
        menu.SetActive(false);
        description.text = description2;
    }
    
    public void ShowCredits()
    {
        bool creditsOn = creditsAnimator.GetBool("Credits");
        Debug.Log("creditsOn: " + creditsOn);
        creditsAnimator.SetBool("Credits", !creditsOn);
        // creditsAnimator.SetTrigger("Credits");
    }

    public void Play()
    {
        Debug.Log("Game started");
        // GameManager.instance.StartCoroutine("RestartLevel");
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    
    public void ShowRules()
    {
        bool rulesOn = rulesAnimator.GetBool("Rules");
        Debug.Log("rules on: " + rulesOn);
        rulesAnimator.SetBool("Rules", !rulesOn);
        // rulesAnimator.SetTrigger("Rules");
    }
    
}