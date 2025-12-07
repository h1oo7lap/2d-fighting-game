using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Scene names - cập nhật sau khi tạo scenes
    public const string SCENE_HOME = "Home";
    public const string SCENE_HOW_TO_PLAY = "HowToPlay";
    public const string SCENE_CHARACTER_SELECTION = "CharacterSelection";
    public const string SCENE_MAP_SELECTION = "MapSelection";
    public const string SCENE_BATTLE = "Battle 1"; // Đổi thành tên scene thực tế
    public const string SCENE_RESULT = "Result";

    // Navigation Methods
    public static void LoadHome()
    {
        Time.timeScale = 1; // Reset time scale nếu bị pause
        SceneManager.LoadScene(SCENE_HOME);
    }

    public static void LoadHowToPlay()
    {
        SceneManager.LoadScene(SCENE_HOW_TO_PLAY);
    }

    public static void LoadCharacterSelection()
    {
        SceneManager.LoadScene(SCENE_CHARACTER_SELECTION);
    }

    public static void LoadMapSelection()
    {
        // Kiểm tra đã chọn đủ characters chưa
        if (GameManager.Instance != null && GameManager.Instance.IsBothCharactersSelected())
        {
            SceneManager.LoadScene(SCENE_MAP_SELECTION);
        }
        else
        {
            Debug.LogWarning("Chưa chọn đủ 2 nhân vật!");
        }
    }

    public static void LoadBattle()
    {
        Debug.Log("=== SceneController.LoadBattle called ===");
        
        // Kiểm tra đã chọn map chưa
        if (GameManager.Instance != null && GameManager.Instance.IsMapSelected())
        {
            Debug.Log("Map is selected: " + GameManager.Instance.selectedMap.mapName);
            Debug.Log("Character 1: " + (GameManager.Instance.selectedCharacter1 != null ? GameManager.Instance.selectedCharacter1.characterName : "NULL"));
            Debug.Log("Character 2: " + (GameManager.Instance.selectedCharacter2 != null ? GameManager.Instance.selectedCharacter2.characterName : "NULL"));
            
            Time.timeScale = 1; // Đảm bảo game không bị pause
            
            // Kiểm tra xem map có battle scene riêng không
            string sceneToLoad = SCENE_BATTLE; // Default
            
            if (!string.IsNullOrEmpty(GameManager.Instance.selectedMap.battleSceneName))
            {
                sceneToLoad = GameManager.Instance.selectedMap.battleSceneName;
                Debug.Log("Map has custom battle scene: " + sceneToLoad);
            }
            else
            {
                Debug.Log("Using default battle scene: " + sceneToLoad);
            }
            
            Debug.Log("Attempting to load scene: " + sceneToLoad);
            
            try
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load Battle scene: " + e.Message);
                Debug.LogError("Make sure '" + sceneToLoad + "' scene is added to Build Settings!");
            }
        }
        else
        {
            if (GameManager.Instance == null)
                Debug.LogWarning("GameManager.Instance is NULL!");
            else
                Debug.LogWarning("Chưa chọn map!");
        }
    }

    public static void LoadResult()
    {
        SceneManager.LoadScene(SCENE_RESULT);
    }

    // Utility
    public static void QuitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit();
    }
}
