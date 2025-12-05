using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData[] availableCharacters; // Assign trong Inspector

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI player1SelectedText;
    public TextMeshProUGUI player2SelectedText;
    public Button nextButton;
    public Button backButton;

    [Header("Character Buttons")]
    public Button[] characterButtons; // Buttons cho từng character

    [Header("Character Display")]
    public Image[] characterPortraits; // Images hiển thị portraits
    public TextMeshProUGUI[] characterNames; // Text hiển thị tên

    private CharacterData player1Selection;
    private CharacterData player2Selection;

    void Start()
    {
        SetupUI();
        UpdateUI();
    }

    void SetupUI()
    {
        // Setup character buttons
        for (int i = 0; i < characterButtons.Length && i < availableCharacters.Length; i++)
        {
            int index = i; // Capture for closure
            characterButtons[i].onClick.AddListener(() => OnCharacterButtonClicked(index));

            // Display character info
            if (i < characterPortraits.Length)
                characterPortraits[i].sprite = availableCharacters[i].portraitSprite;
            
            if (i < characterNames.Length)
                characterNames[i].text = availableCharacters[i].characterName;
        }

        // Setup navigation buttons
        nextButton.onClick.AddListener(OnNextButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnCharacterButtonClicked(int characterIndex)
    {
        CharacterData selectedChar = availableCharacters[characterIndex];

        // Logic: Player 1 chọn trước, sau đó Player 2
        if (player1Selection == null)
        {
            player1Selection = selectedChar;
            GameManager.Instance.SetPlayer1Character(selectedChar);
        }
        else if (player2Selection == null)
        {
            player2Selection = selectedChar;
            GameManager.Instance.SetPlayer2Character(selectedChar);
        }
        else
        {
            // Cả 2 đã chọn rồi, reset và chọn lại
            player1Selection = selectedChar;
            player2Selection = null;
            GameManager.Instance.SetPlayer1Character(selectedChar);
            GameManager.Instance.SetPlayer2Character(null);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        // Update selection text
        if (player1Selection != null)
            player1SelectedText.text = "Player 1: " + player1Selection.characterName;
        else
            player1SelectedText.text = "Player 1: Chưa chọn";

        if (player2Selection != null)
            player2SelectedText.text = "Player 2: " + player2Selection.characterName;
        else
            player2SelectedText.text = "Player 2: Chưa chọn";

        // Enable/disable next button
        nextButton.interactable = (player1Selection != null && player2Selection != null);
    }

    void OnNextButtonClicked()
    {
        if (player1Selection != null && player2Selection != null)
        {
            SceneController.LoadMapSelection();
        }
    }

    void OnBackButtonClicked()
    {
        SceneController.LoadHome();
    }
}
