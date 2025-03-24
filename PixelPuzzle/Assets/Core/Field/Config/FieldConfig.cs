using UnityEngine;

namespace PixelPuzzle
{
    [CreateAssetMenu(fileName = "FieldConfig", menuName = "Scriptable Objects/FieldConfig")]
    public class FieldConfig : ScriptableObject
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}
