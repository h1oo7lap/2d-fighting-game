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
        
        // Phát nhạc nền Battle
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBattleBGM();
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
            
            // Tìm PlayerHealth (thử tất cả các loại)
            player1Health = player1Instance.GetComponent<PlayerHealth>();
            PlayerHealth3 p1Health3 = player1Instance.GetComponent<PlayerHealth3>();
            PlayerHealth4 p1Health4 = player1Instance.GetComponent<PlayerHealth4>();
            
            if (player1Health != null)
            {
                player1Health.OnDeathComplete += OnPlayer1DeathComplete;
                if (player1HPSlider != null)
                    player1Health.hpSlider = player1HPSlider;
            }
            else if (p1Health3 != null)
            {
                p1Health3.OnDeathComplete += OnPlayer1DeathComplete;
                if (player1HPSlider != null)
                    p1Health3.hpSlider = player1HPSlider;
            }
            else if (p1Health4 != null)
            {
                p1Health4.OnDeathComplete += OnPlayer1DeathComplete;
                if (player1HPSlider != null)
                    p1Health4.hpSlider = player1HPSlider;
            }

            // Tìm PlayerMana (thử tất cả các loại)
            PlayerMana p1Mana = player1Instance.GetComponent<PlayerMana>();
            PlayerMana3 p1Mana3 = player1Instance.GetComponent<PlayerMana3>();
            PlayerMana4 p1Mana4 = player1Instance.GetComponent<PlayerMana4>();
            
            if (p1Mana != null && player1ManaSlider != null)
                p1Mana.manaSlider = player1ManaSlider;
            else if (p1Mana3 != null && player1ManaSlider != null)
                p1Mana3.manaSlider = player1ManaSlider;
            else if (p1Mana4 != null && player1ManaSlider != null)
                p1Mana4.manaSlider = player1ManaSlider;

            // Set isPlayer1 cho tất cả các loại PlayerController
            PlayerController p1Controller = player1Instance.GetComponent<PlayerController>();
            PlayerController3 p1Controller3 = player1Instance.GetComponent<PlayerController3>();
            PlayerController4 p1Controller4 = player1Instance.GetComponent<PlayerController4>();
            
            if (p1Controller != null)
                p1Controller.isPlayer1 = true;
            if (p1Controller3 != null)
                p1Controller3.isPlayer1 = true;
            if (p1Controller4 != null)
                p1Controller4.isPlayer1 = true;

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
            
            // Tìm PlayerHealth (thử tất cả các loại)
            player2Health = player2Instance.GetComponent<PlayerHealth>();
            PlayerHealth3 p2Health3 = player2Instance.GetComponent<PlayerHealth3>();
            PlayerHealth4 p2Health4 = player2Instance.GetComponent<PlayerHealth4>();
            
            if (player2Health != null)
            {
                player2Health.OnDeathComplete += OnPlayer2DeathComplete;
                if (player2HPSlider != null)
                    player2Health.hpSlider = player2HPSlider;
            }
            else if (p2Health3 != null)
            {
                p2Health3.OnDeathComplete += OnPlayer2DeathComplete;
                if (player2HPSlider != null)
                    p2Health3.hpSlider = player2HPSlider;
            }
            else if (p2Health4 != null)
            {
                p2Health4.OnDeathComplete += OnPlayer2DeathComplete;
                if (player2HPSlider != null)
                    p2Health4.hpSlider = player2HPSlider;
            }

            // Tìm PlayerMana (thử tất cả các loại)
            PlayerMana p2Mana = player2Instance.GetComponent<PlayerMana>();
            PlayerMana3 p2Mana3 = player2Instance.GetComponent<PlayerMana3>();
            PlayerMana4 p2Mana4 = player2Instance.GetComponent<PlayerMana4>();
            
            if (p2Mana != null && player2ManaSlider != null)
                p2Mana.manaSlider = player2ManaSlider;
            else if (p2Mana3 != null && player2ManaSlider != null)
                p2Mana3.manaSlider = player2ManaSlider;
            else if (p2Mana4 != null && player2ManaSlider != null)
                p2Mana4.manaSlider = player2ManaSlider;

            // Set isPlayer1 cho tất cả các loại PlayerController
            PlayerController p2Controller = player2Instance.GetComponent<PlayerController>();
            PlayerController3 p2Controller3 = player2Instance.GetComponent<PlayerController3>();
            PlayerController4 p2Controller4 = player2Instance.GetComponent<PlayerController4>();
            
            if (p2Controller != null)
                p2Controller.isPlayer1 = false;
            if (p2Controller3 != null)
                p2Controller3.isPlayer1 = false;
            if (p2Controller4 != null)
                p2Controller4.isPlayer1 = false;

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
        
        // Play victory sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayVictorySound();

        StartCoroutine(LoadResultSceneAfterDelay(2f));
    }

    IEnumerator LoadResultSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneController.LoadResult();
    }
}