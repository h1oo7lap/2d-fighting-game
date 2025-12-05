using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Scene names - cập nhật sau khi tạo scenes
    public const string SCENE_HOME = "Home";
    public const string SCENE_HOW_TO_PLAY = "HowToPlay";
    public const string SCENE_CHARACTER_SELECTION = "CharacterSelection";
    public const string SCENE_MAP_SELECTION = "MapSelection";
    public const string SCENE_BATTLE = "Battle";
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
        // Kiểm tra đã chọn map chưa
        if (GameManager.Instance != null && GameManager.Instance.IsMapSelected())
        {
            Time.timeScale = 1; // Đảm bảo game không bị pause
            SceneManager.LoadScene(SCENE_BATTLE);
        }
        else
        {
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
