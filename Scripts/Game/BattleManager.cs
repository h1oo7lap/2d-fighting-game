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
    public SpriteRenderer mapBackground;

    [Header("UI Sliders")]
    public Slider player1HPSlider;
    public Slider player1ManaSlider;
    public Slider player2HPSlider;
    public Slider player2ManaSlider;

    [Header("UI (Optional)")]
    public TextMeshProUGUI resultText;

    private GameObject player1Instance;
    private GameObject player2Instance;
    private PlayerHealth player1Health;
    private PlayerHealth player2Health;
    private bool gameEnded = false;

    void Start()
    {
        SpawnCharacters();
        LoadMapBackground();
        
        if (resultText != null)
            resultText.gameObject.SetActive(false);
    }

    void SpawnCharacters()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager không tồn tại!");
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
            
            player1Health = player1Instance.GetComponent<PlayerHealth>();
            if (player1Health != null)
            {
                player1Health.OnDeathComplete += OnPlayer1DeathComplete;
                if (player1HPSlider != null)
                    player1Health.hpSlider = player1HPSlider;
            }

            PlayerMana p1Mana = player1Instance.GetComponent<PlayerMana>();
            if (p1Mana != null && player1ManaSlider != null)
                p1Mana.manaSlider = player1ManaSlider;

            PlayerController p1Controller = player1Instance.GetComponent<PlayerController>();
            if (p1Controller != null)
                p1Controller.isPlayer1 = true;

            Debug.Log("Spawned Player 1");
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
            
            player2Health = player2Instance.GetComponent<PlayerHealth>();
            if (player2Health != null)
            {
                player2Health.OnDeathComplete += OnPlayer2DeathComplete;
                if (player2HPSlider != null)
                    player2Health.hpSlider = player2HPSlider;
            }

            PlayerMana p2Mana = player2Instance.GetComponent<PlayerMana>();
            if (p2Mana != null && player2ManaSlider != null)
                p2Mana.manaSlider = player2ManaSlider;

            PlayerController p2Controller = player2Instance.GetComponent<PlayerController>();
            if (p2Controller != null)
                p2Controller.isPlayer1 = false;

            Debug.Log("Spawned Player 2");
        }
    }

    void LoadMapBackground()
    {
        if (GameManager.Instance == null || GameManager.Instance.selectedMap == null)
            return;

        if (mapBackground != null && GameManager.Instance.selectedMap.backgroundSprite != null)
        {
            mapBackground.sprite = GameManager.Instance.selectedMap.backgroundSprite;
        }
    }

    void OnDestroy()
    {
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

        if (GameManager.Instance != null)
            GameManager.Instance.SetWinner(winner);

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = winner + " thắng!";
        }

        StartCoroutine(LoadResultSceneAfterDelay(2f));
    }

    IEnumerator LoadResultSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneController.LoadResult();
    }
}