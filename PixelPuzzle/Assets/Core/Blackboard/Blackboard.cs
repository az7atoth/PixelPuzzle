
using UniRx;

namespace PixelPuzzle
{
    public static class Blackboard
    {
        public static bool GameInProgress { get; set; }
        public static bool SomeAnimationInProgress { get; set; }

        public static ReactiveProperty<int> AvailableHints { get; } = new();
    }
}
