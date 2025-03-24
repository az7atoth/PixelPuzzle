using UnityEngine;

namespace PixelPuzzle
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public int OnStartRevealedBlocks { get; private set; } = 5;
        [field: SerializeField] public int MaxAvailableHints { get; private set; } = 3;
    }
}
