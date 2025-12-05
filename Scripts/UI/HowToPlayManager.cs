using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlayManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI player1InstructionsText;
    public TextMeshProUGUI player2InstructionsText;
    public Button backButton;

    void Start()
    {
        SetupUI();
    }

    void SetupUI()
    {
        // Setup instructions cho Player 1
        if (player1InstructionsText != null)
        {
            player1InstructionsText.text = GetPlayer1Instructions();
        }

        // Setup instructions cho Player 2
        if (player2InstructionsText != null)
        {
            player2InstructionsText.text = GetPlayer2Instructions();
        }

        // Setup back button
        backButton.onClick.AddListener(OnBackClicked);
    }

    string GetPlayer1Instructions()
    {
        return @"<b><size=24>PLAYER 1</size></b>

<b>Di chuyển:</b>
• A - Trái
• D - Phải
• W - Nhảy

<b>Tấn công:</b>
• J - Tấn công cơ bản
• K - Skill 1
• L - Skill 2";
    }

    string GetPlayer2Instructions()
    {
        return @"<b><size=24>PLAYER 2</size></b>

<b>Di chuyển:</b>
• ← - Trái
• → - Phải
• ↑ - Nhảy

<b>Tấn công:</b>
• 1 - Tấn công cơ bản
• 2 - Skill 1
• 3 - Skill 2";
    }

    void OnBackClicked()
    {
        SceneController.LoadHome();
    }
}

