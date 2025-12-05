using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Map Background")]
    public SpriteRenderer mapBackground; // Optional - để set background từ MapData

    [Header("UI (Optional - có thể xóa sau)")]
    public TextMeshProUGUI resultText; // Có thể xóa vì giờ dùng Result scene

    private GameObject player1Instance;
    private GameObject player2Instance;
    private PlayerHealth player1Health;
    private PlayerHealth player2Health;

    private bool gameEnded = false;

    void Start()
    {
        SpawnCharacters();
        LoadMapBackground();
        
        // Ẩn text kết quả nếu còn dùng
        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }

    void SpawnCharacters()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager không tồn tại! Không thể spawn characters.");
            return;
        }

        // Spawn Player 1
        if (GameManager.Instance.selectedCharacter1 != null && player1SpawnPoint != null)
        {
            player1Instance = Instantiate(
                GameManager.Instance.selectedCharacter1.characterPrefab,
                player1SpawnPoint.position,
                Quaternion.identity
            );
            player1Instance.name = "Player1";
            
            // Get PlayerHealth component
            player1Health = player1Instance.GetComponent<PlayerHealth>();
            if (player1Health != null)
            {
                player1Health.OnDeathComplete += OnPlayer1DeathComplete;
            }

            // Ensure PlayerController knows it's player 1
            PlayerController p1Controller = player1Instance.GetComponent<PlayerController>();
            if (p1Controller != null)
            {
                p1Controller.isPlayer1 = true;
            }

            Debug.Log("Spawned Player 1: " + GameManager.Instance.selectedCharacter1.characterName);
        }

        // Spawn Player 2
        if (GameManager.Instance.selectedCharacter2 != null && player2SpawnPoint != null)
        {
            player2Instance = Instantiate(
                GameManager.Instance.selectedCharacter2.characterPrefab,
                player2SpawnPoint.position,
                Quaternion.identity
            );
            player2Instance.name = "Player2";
            
            // Get PlayerHealth component
            player2Health = player2Instance.GetComponent<PlayerHealth>();
            if (player2Health != null)
            {
                player2Health.OnDeathComplete += OnPlayer2DeathComplete;
            }

            // Ensure PlayerController knows it's player 2
            PlayerController p2Controller = player2Instance.GetComponent<PlayerController>();
            if (p2Controller != null)
            {
                p2Controller.isPlayer1 = false;
            }

            Debug.Log("Spawned Player 2: " + GameManager.Instance.selectedCharacter2.characterName);
        }
    }

    void LoadMapBackground()
    {
        if (GameManager.Instance == null || GameManager.Instance.selectedMap == null)
            return;

        // Set background sprite nếu có
        if (mapBackground != null && GameManager.Instance.selectedMap.backgroundSprite != null)
        {
            mapBackground.sprite = GameManager.Instance.selectedMap.backgroundSprite;
            Debug.Log("Loaded map background: " + GameManager.Instance.selectedMap.mapName);
        }
    }

    void OnDestroy()
    {
        // Hủy đăng ký events
        if (player1Health != null)
            player1Health.OnDeathComplete -= OnPlayer1DeathComplete;
        
        if (player2Health != null)
            player2Health.OnDeathComplete -= OnPlayer2DeathComplete;
    }

    void OnPlayer1DeathComplete()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            EndGame("Player 2");
        }
    }

    void OnPlayer2DeathComplete()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            EndGame("Player 1");
        }
    }

    void EndGame(string winner)
    {
        Debug.Log("Trận đấu kết thúc: " + winner + " thắng!");

        // Lưu winner vào GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetWinner(winner);
        }

        // Hiện text tạm thời (optional)
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = winner + " thắng!";
        }

        // Đợi 2 giây rồi chuyển sang Result scene
        StartCoroutine(LoadResultSceneAfterDelay(2f));
    }

    IEnumerator LoadResultSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneController.LoadResult();
    }
}