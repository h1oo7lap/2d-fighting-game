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
        SetupUI();
        UpdateUI();
    }

    void SetupUI()
    {
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
        selectedMap = availableMaps[mapIndex];
        GameManager.Instance.SetMap(selectedMap);
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
        if (selectedMap != null)
        {
            SceneController.LoadBattle();
        }
    }

    void OnBackButtonClicked()
    {
        SceneController.LoadCharacterSelection();
    }
}
