using RL = Raylib_cs.Raylib;
using Raylib_cs;
using Mandacalm.Core;
using Mandacalm.Rendering;
using System.Numerics;
using System.Data;

namespace Mandacalm.Screen;

public sealed class PaintScreen : IScreen
{
    private const float MinBrushRadius = 4f;
    private const float MaxBrushRadius = 40f;
    private float _brushRadius = 12f;
    private readonly GameState _state;
    private readonly PaintCanvas _canvas; 
    private readonly MandalaLayout _layout = new(new(RL.GetScreenWidth()/2, RL.GetScreenHeight()/2), 50f, 60f);
    private readonly Vector2 _barPosition = new(50, 50);
    private int _currentColor;
    private Vector2 _lastMouse;
    private bool _painting;
    private bool _erasing;
    private Mandala? _lastMandala;
    public PaintScreen(GameState state)
    {
        _state = state;
        _canvas = new(RL.GetScreenWidth(), RL.GetScreenHeight());
    }
    public ScreenState? Update()
    {
        var mousePos = RL.GetMousePosition();
        if (RL.IsKeyPressed(KeyboardKey.Tab))
        {
            return ScreenState.Menu;
        }
        float wheel = RL.GetMouseWheelMove();
        if(wheel != 0)
        {
            _brushRadius = Math.Clamp(_brushRadius + wheel * 2f, MinBrushRadius, MaxBrushRadius);
        }
        if(!ReferenceEquals(_state.Mandala, _lastMandala))
        {
            _canvas.Clear();
            _lastMandala = _state.Mandala;
        }

        Color brush = ColorConvert.ToRaylib(_state.Palette.Get(_currentColor));


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
        if (RL.IsMouseButtonPressed(MouseButton.Left))
        {
            int clickedColor = PaletteBar.GetIndexAt(_state.Palette, _barPosition, mousePos);
            if(clickedColor >= 0)
            {
                _currentColor = clickedColor;
            }
            else
            {
                _painting = true;
                _canvas.Stroke(mousePos, mousePos, _brushRadius, brush);
            }
        }
        else if(RL.IsMouseButtonDown(MouseButton.Left) && _painting)
        {
            _canvas.Stroke(_lastMouse, mousePos, _brushRadius, brush);
        }

        if (RL.IsMouseButtonReleased(MouseButton.Left))
        {
            _painting = false;
        }

        // Radieren: solange die rechte Taste gehalten wird
            if (RL.IsMouseButtonPressed(MouseButton.Right))
        {
            _erasing = true;
            _canvas.Stroke(mousePos, mousePos, _brushRadius, PaintCanvas.Paper);
        }
        else if (RL.IsMouseButtonDown(MouseButton.Right) && _erasing)
        {
            _canvas.Stroke(_lastMouse, mousePos, _brushRadius, PaintCanvas.Paper);
        }
        if (RL.IsMouseButtonReleased(MouseButton.Right))
        {
            _erasing = false;
        }

        _lastMouse = mousePos;

        if (RL.IsKeyPressed(KeyboardKey.Enter))
        {
            Image image = _canvas.ReadImage();
            int[] indicies = CanvasSampler.Sample(image, _state.Mandala, _layout, _state.Palette);
            RL.UnloadImage(image);

            int i = 0;
            foreach(var cell in _state.Mandala.AllCells)
            {
                cell.ColorIndex = indicies[i];
                i++;
            }

            if (_state.TrySubmit())
            {
                _canvas.Clear();
                _lastMandala = _state.Mandala;
                return ScreenState.Result;
            }

            foreach(var cell in _state.Mandala.AllCells)
            {
                cell.ColorIndex = Cell.Unpainted;
            }
        }
        return null;
    }

    public void Draw()
    {
        _canvas.Draw();
        float outerRadius = _layout.InnerRadius + _state.Mandala.Parameters.RingCount * _layout.RingWidth;
        RL.DrawCircleV(_layout.Center, _layout.InnerRadius, Color.SkyBlue);
        RL.DrawRing(_layout.Center, outerRadius, 2000f, 0, 360, 128, Color.SkyBlue);

        MandalaRenderer.DrawOutlines(_state.Mandala, _layout);
        PaletteBar.Draw(_state.Palette, _currentColor, _barPosition);
        RL.DrawText($"Points: {_state.Wallet.Points}", 50, 780 - 50, 20, Color.Black);

        Vector2 mouse = RL.GetMousePosition();
        bool overPaletteBar = PaletteBar.GetIndexAt(_state.Palette, _barPosition, mouse) >= 0;
        if (!overPaletteBar)
        {
            RL.DrawCircleLines((int)mouse.X, (int)mouse.Y, _brushRadius, Color.Black);
            RL.DrawCircleLines((int)mouse.X, (int)mouse.Y, _brushRadius + 1f, Color.White);
        }
    }

    public void OnEnter()
    {
        if(!ReferenceEquals(_state.Mandala, _lastMandala))
        {
            _canvas.Clear();
            _lastMandala = _state.Mandala;
        }
    }

    public void SaveCanvas(string path) => _canvas.Export(path);

    public void LoadCanvas(string path)
    {
        _canvas.Import(path);
        _lastMandala = _state.Mandala;
    }
}