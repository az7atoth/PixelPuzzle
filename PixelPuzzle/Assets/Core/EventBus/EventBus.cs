using UniRx;

namespace PixelPuzzle
{
    public static class EventBus
    {
        //GAME
        public static ReactiveCommand OnPuzzleSolved { get; } = new();

        //BLOCKS
        public static ReactiveCommand<Block> OnBlockPlaced { get; } = new();
        public static ReactiveCommand<Block> OnBlockReturned { get; } = new();

        //UI
        public static ReactiveCommand UI_OnStartGamePressed { get; } = new();
        public static ReactiveCommand UI_OnMainMenuPressed { get; } = new();
        public static ReactiveCommand UI_OnLevelMenuPressed { get; } = new();
        public static ReactiveCommand UI_OnAcceptionButtonPressed { get; } = new();
        public static ReactiveCommand UI_OnCancellationButtonPressed { get; } = new();
        public static ReactiveCommand UI_OnToggleSoundsPressed { get; } = new();
        public static ReactiveCommand<int> UI_OnLevelChoose { get; } = new();
    }
}
