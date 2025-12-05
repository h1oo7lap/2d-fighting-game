using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlayManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI instructionsText;
    public Button backButton;

    void Start()
    {
        SetupUI();
    }

    void SetupUI()
    {
        // Setup instructions
        if (instructionsText != null)
        {
            instructionsText.text = GetInstructions();
        }

        // Setup back button
        backButton.onClick.AddListener(OnBackClicked);
    }

    string GetInstructions()
    {
        return @"CÁCH CHƠI

PLAYER 1:
• Di chuyển: A (trái), D (phải)
• Nhảy: W
• Tấn công cơ bản (J): 10 damage, 20 mana
• Skill mạnh (K): 30 damage, 50 mana
• Bắn chưởng (L): 20 damage, 30 mana

PLAYER 2:
• Di chuyển: ← (trái), → (phải)
• Nhảy: ↑
• Tấn công cơ bản (1): 10 damage, 20 mana
• Skill mạnh (2): 30 damage, 50 mana
• Bắn chưởng (3): 20 damage, 30 mana

MỤC TIÊU:
Đánh bại đối thủ bằng cách giảm HP của họ về 0!

LƯU Ý:
• Mana sẽ tự động hồi phục theo thời gian
• Sau khi bị đánh, bạn có 0.5 giây bất tử
";
    }

    void OnBackClicked()
    {
        SceneController.LoadHome();
    }
}
