using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    [TextArea(2, 4)]
    public string description;
    
    [Header("Prefab & Visuals")]
    public GameObject characterPrefab;
    public Sprite portraitSprite; // Sprite hiển thị trong UI selection
    
    [Header("Stats Preview")]
    public int maxHP = 100;
    public int attackDamage = 10;
    public int skillKDamage = 30;
    public int projectileDamage = 20;
    
    [Header("Character ID")]
    public int characterID; // 0 = Character1, 1 = Character2, etc.
}
