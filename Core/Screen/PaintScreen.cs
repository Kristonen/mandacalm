using RL = Raylib_cs.Raylib;
using Raylib_cs;
using Mandacalm.Core;
using Mandacalm.Rendering;
using System.Numerics;

namespace Mandacalm.Screen;

public sealed class PaintScreen : IScreen
{
    public readonly GameState _state;
    private readonly MandalaLayout _layout = new(new(RL.GetScreenWidth()/2, RL.GetScreenHeight()/2), 50f, 60f);
    private readonly Vector2 _barPosition = new(50, 50);
    private int _currentColor;
    public PaintScreen(GameState state)
    {
        _state = state;
    }
    public ScreenState? Update()
    {
        var mousePos = RL.GetMousePosition();

        // Farbe wählen: ein Klick genügt
        if (RL.IsMouseButtonPressed(MouseButton.Left))
        {
            int clickedColor = PaletteBar.GetIndexAt(_state.Palette, _barPosition, mousePos);
            if (clickedColor >= 0)
            {
                _currentColor = clickedColor;
            }
        }

        // Malen: solange die linke Taste gehalten wird
        if (RL.IsMouseButtonDown(MouseButton.Left))
        {
            Cell? cell = MandalaHitTester.GetCellAt(_state.Mandala, _layout, mousePos);
            if (cell != null)
            {
                cell.ColorIndex = _currentColor;
            }
        }

        // Radieren: solange die rechte Taste gehalten wird
        if (RL.IsMouseButtonDown(MouseButton.Right))
        {
            Cell? cell = MandalaHitTester.GetCellAt(_state.Mandala, _layout, mousePos);
            if (cell != null)
            {
                cell.ColorIndex = Cell.Unpainted;
            }
        }

        if (RL.IsKeyPressed(KeyboardKey.Enter) && _state.TrySubmit())
        {
            return ScreenState.Result;
        }
        return null;
    }

    public void Draw()
    {
        MandalaRenderer.Draw(_state.Mandala, _layout, _state.Palette);
        PaletteBar.Draw(_state.Palette, _currentColor, _barPosition);
        RL.DrawText($"Points: {_state.Wallet.Points}", 50, 780 - 50, 20, Color.Black);
    }
}