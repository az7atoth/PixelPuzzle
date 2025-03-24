using UnityEngine;

namespace PixelPuzzle
{
    public class BackgroundColorChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _changeTime;
        [SerializeField] private Color[] _colors;

        private float _timer;
        private int _colorIndex;
        private Color _currentColor;
        private Color _nextColor;

        private void Awake()
        {
            _colorIndex = Random.Range(0, _colors.Length);
            _spriteRenderer.color = _colors[_colorIndex];
            _currentColor = _colors[_colorIndex];
            _timer = _changeTime;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _changeTime)
            {
                _timer = 0f;
                _colorIndex = Random.Range(0, _colors.Length);
                _currentColor = _nextColor;
                _nextColor = _colors[_colorIndex];
            }

            var t = _timer / _changeTime;
            _spriteRenderer.color = Color.Lerp(_currentColor, _nextColor, t);
        }

    }
}
