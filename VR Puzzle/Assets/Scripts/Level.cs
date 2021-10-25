using OVR;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Text buttonText;
    [SerializeField] private Color lockedColor;
    [SerializeField] private int level;
    [SerializeField] private SoundFXRef levelSelectionSound;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = "Level " + level;
        if (PlayerPrefs.GetInt("level" + level) != 1 && level != 1)
        {
            gameObject.SetActive(false);
            // button.interactable = false;
            // buttonImage.color = lockedColor;
            // buttonImage.raycastTarget = false;
        }
    }

    public void LevelSelected()
    {
        levelSelectionSound.PlaySound();
        GameManager.instance.LoadLevel(level);
    }

}
