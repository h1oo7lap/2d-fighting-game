using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public int maxHP = 100;

    public PlayerHealth player1Health;
    public PlayerHealth player2Health;

    public TextMeshProUGUI resultText; // dùng TMP thay vì Text

    void Update()
    {
        // Kiểm tra HP
        if (player1Health.currentHP <= 0)
            ShowResult("Player 2 thắng!");
        else if (player2Health.currentHP <= 0)
            ShowResult("Player 1 thắng!");
    }

    void ShowResult(string message)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = message;
        Time.timeScale = 0; // tạm dừng game
    }
}