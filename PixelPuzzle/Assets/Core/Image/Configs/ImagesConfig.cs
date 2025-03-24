using UnityEngine;

namespace PixelPuzzle
{
    [CreateAssetMenu(fileName = "ImagesConfig", menuName = "Scriptable Objects/ImagesConfig")]
    public class ImagesConfig : ScriptableObject
    {
        [field: SerializeField] public string ImagesFolderPath { get; private set; }
        [field: SerializeField] public int Width {  get; private set; }
        [field: SerializeField] public int Height {  get; private set; }
    }
}
