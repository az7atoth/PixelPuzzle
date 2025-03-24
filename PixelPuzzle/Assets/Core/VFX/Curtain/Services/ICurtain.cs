using Cysharp.Threading.Tasks;
using System.Threading;

namespace PixelPuzzle
{
    public interface ICurtain
    {
        public void ShowImmidiate(bool showLoadingIcon = true);
        public void HideImmidiate();

        public UniTask ShowAsync(CancellationToken token, bool showLoadingIcon = true);
        public UniTask HideAsync(CancellationToken token);
    }
}
