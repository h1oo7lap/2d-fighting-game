using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapSelectionManager : MonoBehaviour
{
    [Header("Map Data")]
    public MapData[] availableMaps; // Assign trong Inspector

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI selectedMapText;
    public Button startBattleButton;
    public Button backButton;

    [Header("Map Buttons")]
    public Button[] mapButtons; // Buttons cho từng map

    [Header("Map Display")]
    public Image[] mapPreviews; // Images hiển thị map previews
    public TextMeshProUGUI[] mapNames; // Text hiển thị tên map

    private MapData selectedMap;

    void Start()
    {
        Debug.Log("=== MapSelectionManager Start ===");
        
        // Check GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL! Tạo GameManager GameObject trong scene!");
            return;
        }
        
        // Check available maps
        if (availableMaps == null || availableMaps.Length == 0)
        {
            Debug.LogError("availableMaps is empty! Assign MapData assets trong Inspector!");
            return;
        }
        
        Debug.Log("Available maps: " + availableMaps.Length);
        
        SetupUI();
        UpdateUI();
    }

    void SetupUI()
    {
        Debug.Log("=== SetupUI ===");
        
        // Check map buttons
        if (mapButtons == null || mapButtons.Length == 0)
        {
            Debug.LogError("mapButtons array is empty! Assign buttons trong Inspector!");
            return;
        }
        
        Debug.Log("Map buttons count: " + mapButtons.Length);
        // Setup map buttons
        for (int i = 0; i < mapButtons.Length && i < availableMaps.Length; i++)
        {
            int index = i; // Capture for closure
            mapButtons[i].onClick.AddListener(() => OnMapButtonClicked(index));

            // Display map info
            if (i < mapPreviews.Length)
                mapPreviews[i].sprite = availableMaps[i].previewSprite;
            
            if (i < mapNames.Length)
                mapNames[i].text = availableMaps[i].mapName;
        }

        // Setup navigation buttons
        startBattleButton.onClick.AddListener(OnStartBattleClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnMapButtonClicked(int mapIndex)
    {
        Debug.Log("=== Map button clicked: Index " + mapIndex + " ===");
        
        if (mapIndex < 0 || mapIndex >= availableMaps.Length)
        {
            Debug.LogError("Invalid map index: " + mapIndex);
            return;
        }
        
        selectedMap = availableMaps[mapIndex];
        Debug.Log("Selected map: " + selectedMap.mapName);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMap(selectedMap);
        }
        else
        {
            Debug.LogError("GameManager.Instance is NULL!");
        }
        
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update selection text
        if (selectedMap != null)
            selectedMapText.text = "Map: " + selectedMap.mapName;
        else
            selectedMapText.text = "Chưa chọn map";

        // Enable/disable start button
        startBattleButton.interactable = (selectedMap != null);
    }

    void OnStartBattleClicked()
    {
        Debug.Log("=== Start Battle button clicked ===");
        
        if (selectedMap != null)
        {
            Debug.Log("Selected map is: " + selectedMap.mapName);
            Debug.Log("Loading Battle scene...");
            SceneController.LoadBattle();
        }
        else
        {
            Debug.LogWarning("No map selected!");
        }
    }

    void OnBackButtonClicked()
    {
        SceneController.LoadCharacterSelection();
    }
}
