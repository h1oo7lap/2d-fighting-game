using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Selected Data")]
    public CharacterData selectedCharacter1;
    public CharacterData selectedCharacter2;
    public MapData selectedMap;

    [Header("Game State")]
    public string winnerName; // "Player 1" hoặc "Player 2"

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Character Selection Methods
    public void SetPlayer1Character(CharacterData character)
    {
        selectedCharacter1 = character;
        Debug.Log("Player 1 chọn: " + character.characterName);
    }

    public void SetPlayer2Character(CharacterData character)
    {
        selectedCharacter2 = character;
        Debug.Log("Player 2 chọn: " + character.characterName);
    }

    // Map Selection Method
    public void SetMap(MapData map)
    {
        selectedMap = map;
        Debug.Log("Map được chọn: " + map.mapName);
    }

    // Winner Method
    public void SetWinner(string winner)
    {
        winnerName = winner;
        Debug.Log("Người thắng: " + winner);
    }

    // Validation
    public bool IsBothCharactersSelected()
    {
        return selectedCharacter1 != null && selectedCharacter2 != null;
    }

    public bool IsMapSelected()
    {
        return selectedMap != null;
    }

    // Reset (cho rematch hoặc new game)
    public void ResetSelections()
    {
        selectedCharacter1 = null;
        selectedCharacter2 = null;
        selectedMap = null;
        winnerName = "";
    }
}
