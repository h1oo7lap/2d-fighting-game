using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "Game/Map Data")]
public class MapData : ScriptableObject
{
    [Header("Map Info")]
    public string mapName;
    [TextArea(2, 4)]
    public string description;
    
    [Header("Visuals")]
    public Sprite previewSprite; // Sprite hiển thị trong UI selection
    public Sprite backgroundSprite; // Background cho battle scene (optional)
    
    [Header("Map ID")]
    public int mapID; // 0 = Map1, 1 = Map2, etc.
    
    // Có thể thêm sau:
    // public AudioClip backgroundMusic;
    // public GameObject[] environmentPrefabs;
}
