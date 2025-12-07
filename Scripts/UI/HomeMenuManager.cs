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
        
        // Phát nhạc nền Home
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayHomeBGM();
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
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        // Reset selections khi bắt đầu game mới
        if (GameManager.Instance != null)
            GameManager.Instance.ResetSelections();
        
        SceneController.LoadCharacterSelection();
    }

    void OnHowToPlayClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        SceneController.LoadHowToPlay();
    }

    void OnQuitClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        SceneController.QuitGame();
    }
}
