using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        [SerializeField] private FieldCell _fieldCellPrefab;
        [SerializeField] private Transform _cellsParent;
        [SerializeField] private Transform _field;

        private IImageProvider _imageProvider;
        private FieldConfig _fieldConfig;

        private FieldCell[] _cells;

        [Inject]
        public void Construct(IImageProvider imageProvider, FieldConfig fieldConfig)
        {
            _imageProvider = imageProvider;
            _fieldConfig = fieldConfig;
        }

        public void Initialize()
        {
            MakeCells();
        }

        public void PrepareNewField()
        {
            ClearAll();

            var data = _imageProvider.GetImageData();

            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].SetPart(data.GetPart(i));
            }

        }

        public void OpenCell(int id)
        {
            if (id < 0 || id >= _cells.Length) return;

            _cells[id].SetEnabled(true);
        }

        public void ClearAll()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].SetEnabled(false);
            }
        }

        public void Show()
        {
            _cellsParent.gameObject.SetActive(true);
            _field.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _cellsParent.gameObject.SetActive(false);
            _field.gameObject.SetActive(false);
        }

        private void MakeCells()
        {
            _cells = new FieldCell[_fieldConfig.Width * _fieldConfig.Height];

            for (int i = 0; i < _cells.Length; i++)
            {
                var cell = Instantiate(_fieldCellPrefab, _cellsParent);
                cell.name = i.ToString();
                cell.ID = i;

                var position = Utilities.GetGridPosition(i, _fieldConfig.Width);
                cell.transform.position = new Vector3(position.x, position.y, 0f);

                cell.SetSortingOrder(0);

                _cells[i] = cell;
            }
        }

    }
}
