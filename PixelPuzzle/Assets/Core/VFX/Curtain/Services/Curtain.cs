using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Threading;
using UnityEngine;

namespace PixelPuzzle
{
    public class Curtain : MonoBehaviour, ICurtain
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _leftCurtain;
        [SerializeField] private RectTransform _rightCurtain;
        [SerializeField] private Transform _loadingIcon;
        [SerializeField] private float _animationTime = 2f;
        [SerializeField] private bool _showOnAwake;

        private TweenerCore<Vector2, Vector2, VectorOptions> _leftCurtainTween;
        private TweenerCore<Vector2, Vector2, VectorOptions> _rightCurtainTween;

        public void ShowImmidiate(bool showLoadingIcon = true)
        {
            _leftCurtain.anchoredPosition = Vector2.zero;
            _rightCurtain.anchoredPosition = Vector2.zero;

            if (showLoadingIcon)
            {
                _loadingIcon.gameObject.SetActive(true);
            }

            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void HideImmidiate()
        {
            _leftCurtain.anchoredPosition = new Vector2(-_leftCurtain.rect.width, 0f);
            _rightCurtain.anchoredPosition = new Vector2(_rightCurtain.rect.width, 0f);

            _loadingIcon.gameObject.SetActive(false);

            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public UniTask ShowAsync(CancellationToken token, bool showLoadingIcon = true)
        {
            return AnimateAsync(true, showLoadingIcon, token);
        }

        public UniTask HideAsync(CancellationToken token)
        {
            return AnimateAsync(false, false, token);
        }

        private async UniTask AnimateAsync(bool show, bool showLoadingIcon, CancellationToken token)
        {
            token.Register(() =>
            {
                _leftCurtainTween.Pause();
                _rightCurtainTween.Pause();

                if (show)
                {
                    ShowImmidiate(showLoadingIcon);
                }
                else
                {
                    HideImmidiate();
                }

                Blackboard.SomeAnimationInProgress = false;
                return;
            });

            Blackboard.SomeAnimationInProgress = true;

            if (!show)
            {
                _loadingIcon.gameObject.SetActive(false);
            }

            var leftStart = new Vector2(-_leftCurtain.rect.width, 0f);
            var leftEnd = Vector2.zero;

            var rightStart = new Vector2(_rightCurtain.rect.width, 0f);
            var rightEnd = Vector2.zero;

            if (show)
            {
                _leftCurtainTween.ChangeStartValue(leftStart);
                _leftCurtainTween.ChangeEndValue(leftEnd);

                _rightCurtainTween.ChangeStartValue(rightStart);
                _rightCurtainTween.ChangeEndValue(rightEnd);

                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _leftCurtainTween.ChangeStartValue(leftEnd);
                _leftCurtainTween.ChangeEndValue(leftStart);

                _rightCurtainTween.ChangeStartValue(rightEnd);
                _rightCurtainTween.ChangeEndValue(rightStart);
            }

            _leftCurtainTween.Rewind();
            _rightCurtainTween.Rewind();

            await UniTask.WhenAll
                (
                _leftCurtainTween.Play().AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(token),
                _rightCurtainTween.Play().AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(token)
                );

            if (show && showLoadingIcon)
            {
                _loadingIcon.gameObject.SetActive(true);
            }

            if (!show)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
            }

            Blackboard.SomeAnimationInProgress = false;
        }

        private void MakeTweens()
        {
            _leftCurtainTween = _leftCurtain
                .DOAnchorPos(Vector2.zero, _animationTime)
                .SetAutoKill(false)
                .SetEase(Ease.InOutCubic)
                .Pause();

            _rightCurtainTween = _rightCurtain
                .DOAnchorPos(Vector2.zero, _animationTime)
                .SetAutoKill(false)
                .SetEase(Ease.InOutCubic)
                .Pause();
        }

        private void Awake()
        {
            _leftCurtain.anchorMin = new Vector2(0f, 0f);
            _rightCurtain.anchorMax = new Vector2(1f, 1f);

            MakeTweens();

            if (_showOnAwake)
            {
                ShowImmidiate(false);
            }
        }

    }
}
