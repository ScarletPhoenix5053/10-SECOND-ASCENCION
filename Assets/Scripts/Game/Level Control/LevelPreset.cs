using UnityEngine;
using UnityEditor;

namespace Sierra.AGPW.TenSecondAscencion
{
    [CreateAssetMenu(fileName = "LevelPreset", menuName = "LevelPreset")]
    public class LevelPreset : ScriptableObject
    {
        public TileType[,] Tiles;
        public GameObject ExtrasPrefab;
    }
    
}