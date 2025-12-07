using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;
    public Image player1Portrait;
    public Image player2Portrait;
    
    public Button rematchButton;
    public Button changeCharactersButton;
    public Button mainMenuButton;

    void Start()
    {
        DisplayResults();
        SetupButtons();
        
        // Phát nhạc nền Result
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayResultBGM();
    }

    void DisplayResults()
    {
        if (GameManager.Instance == null)
            return;

        // Display winner
        if (winnerText != null)
        {
            winnerText.text = GameManager.Instance.winnerName + " THẮNG!";
        }

        // Display character info
        if (GameManager.Instance.selectedCharacter1 != null)
        {
            if (player1NameText != null)
                player1NameText.text = "P1: " + GameManager.Instance.selectedCharacter1.characterName;
            
            if (player1Portrait != null)
                player1Portrait.sprite = GameManager.Instance.selectedCharacter1.portraitSprite;
        }

        if (GameManager.Instance.selectedCharacter2 != null)
        {
            if (player2NameText != null)
                player2NameText.text = "P2: " + GameManager.Instance.selectedCharacter2.characterName;
            
            if (player2Portrait != null)
                player2Portrait.sprite = GameManager.Instance.selectedCharacter2.portraitSprite;
        }
    }

    void SetupButtons()
    {
        rematchButton.onClick.AddListener(OnRematchClicked);
        changeCharactersButton.onClick.AddListener(OnChangeCharactersClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    void OnRematchClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        // Giữ nguyên characters và map, chơi lại
        SceneController.LoadBattle();
    }

    void OnChangeCharactersClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        // Reset selections và quay về character selection
        if (GameManager.Instance != null)
            GameManager.Instance.ResetSelections();
        
        SceneController.LoadCharacterSelection();
    }

    void OnMainMenuClicked()
    {
        // Play button click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        // Reset và về home
        if (GameManager.Instance != null)
            GameManager.Instance.ResetSelections();
        
        SceneController.LoadHome();
    }
}
