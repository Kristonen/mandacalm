using RL = Raylib_cs.Raylib;
using Raylib_cs;
using Mandacalm.Core;
using Mandacalm.Rendering;
using System.Numerics;

namespace Mandacalm.Screen;

public sealed class ShopScreen : IScreen
{
    public GameState _state;
    private Vector2 _shopPosition = new(50, 50);
    private readonly Rectangle _back = new (RL.GetScreenWidth() - 270, RL.GetScreenHeight() - 90, 220, 56);

    public ShopScreen(GameState gameState)
    {
        _state = gameState;
    }

    public ScreenState? Update()
    {
        var mousePos = RL.GetMousePosition();
        if (RL.IsMouseButtonPressed(MouseButton.Left))
        {
            var clickedOffer = ShopPanel.GetIndexAt(_state.Shop, _shopPosition, mousePos);
            if(clickedOffer != -1)
            {
                _state.Shop.TryBuy(clickedOffer, _state);
            }
        }

        if(UiButton.IsClicked(_back)) return ScreenState.Menu;

        return null;
    }

    public void Draw()
    {
        ShopPanel.Draw(_state.Shop, _state.Wallet, _shopPosition);
        UiButton.Draw(_back, "x");
    }
}