using UnityEngine;

namespace PixelPuzzle
{
    public class FieldCell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _imagePart;
        [SerializeField] private SpriteRenderer _shadow;
        public int ID { get; set; }

        public void SetPart(Sprite sprite)
        {
            _imagePart.sprite = sprite;
        }

        public void SetEnabled(bool enabled)
        {
            _imagePart.gameObject.SetActive(enabled);
            _shadow.gameObject.SetActive(enabled);
        }

        public void SetSortingOrder(int order)
        {
            _imagePart.sortingOrder = order;
            _shadow.sortingOrder = order - 1;
        }
    } 
}
