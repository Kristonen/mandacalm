using Mandacalm.Core;
using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace Mandacalm.Screen;

public sealed class MenuScreen : IScreen
{
    private readonly Rectangle _paintButton;
    private readonly Rectangle _shopButton;
    private readonly Rectangle _newButton;

    public MenuScreen()
    {
        float x = RL.GetScreenWidth()/2f - 160f;
        float y = RL.GetScreenHeight()/2f - 130f;
        _paintButton = new(x, y, 320, 64);
        _shopButton = new(x, y + 90, 320, 64);
        _newButton = new(x, y + 180, 320, 64);
    }

    public ScreenState? Update()
    {
        if(UiButton.IsClicked(_paintButton)) return ScreenState.Painting;
        if(UiButton.IsClicked(_shopButton)) return ScreenState.Shop;
        if(UiButton.IsClicked(_newButton)) return ScreenState.Generator;
        return null;
    }

    public void Draw()
    {
        UiButton.Draw(_paintButton, "Draw");
        UiButton.Draw(_shopButton, "Shop");
        UiButton.Draw(_newButton, "New Mandala");
    }
}