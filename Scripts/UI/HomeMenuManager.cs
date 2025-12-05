using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public Button howToPlayButton;
    public Button quitButton;

    void Start()
    {
        SetupButtons();
    }

    void SetupButtons()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        howToPlayButton.onClick.AddListener(OnHowToPlayClicked);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    void OnPlayClicked()
    {
        // Reset selections khi bắt đầu game mới
        if (GameManager.Instance != null)
            GameManager.Instance.ResetSelections();
        
        SceneController.LoadCharacterSelection();
    }

    void OnHowToPlayClicked()
    {
        SceneController.LoadHowToPlay();
    }

    void OnQuitClicked()
    {
        SceneController.QuitGame();
    }
}
