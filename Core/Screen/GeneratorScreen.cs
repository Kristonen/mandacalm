using System.Numerics;
using Raylib_cs;
using Mandacalm.Core;
using Mandacalm.Rendering;
using RL = Raylib_cs.Raylib;

namespace Mandacalm.Screen;

public sealed class GeneratorScreen : IScreen
{
    private readonly GameState _state;
        private readonly Rectangle _ringsMinus = new(80, 150, 56, 56);
    private readonly Rectangle _ringsPlus = new(260, 150, 56, 56);
    private readonly Rectangle _segmentsMinus = new(80, 240, 56, 56);
    private readonly Rectangle _segmentsPlus = new(260, 240, 56, 56);
    private readonly Rectangle _start = new(80, 400, 236, 64);
    private readonly Rectangle _back = new(80, 480, 236, 56);

    private int _rings;
    private int _segments;
    private Mandala? _preview;


    public GeneratorScreen(GameState state)
    {
        _state = state;
    }

    public void OnEnter()
    {
        _rings = _state.Parameter.RingCount;
        _segments = _state.Parameter.SegmentCount;
        RebuildPreview();
    }

    public ScreenState? Update()
    {
        int oldRings = _rings;
        int oldSegments = _segments;

        if(UiButton.IsClicked(_ringsMinus, _rings > 1)) _rings--;
        if(UiButton.IsClicked(_ringsPlus, _rings < _state.Limits.MaxRings)) _rings++;
        if(UiButton.IsClicked(_segmentsMinus, _segments > 1)) _segments--;
        if(UiButton.IsClicked(_segmentsPlus, _segments < _state.Limits.MaxSegments)) _segments++;

        if(_rings != oldRings || _segments != oldSegments)
        {
            RebuildPreview();
        }

        if(UiButton.IsClicked(_start))
        {
            _state.StartMandala(_rings, _segments, _state.Parameter.Seed + 1);
            return ScreenState.Painting;
        }

        if(UiButton.IsClicked(_back)) return ScreenState.Menu;
        return null;
    }

    public void Draw()
    {
                RL.DrawText("New mandala", 80, 60, 40, Color.Black);

        UiButton.Draw(_ringsMinus, "-", _rings > 1);
        UiButton.Draw(_ringsPlus, "+", _rings < _state.Limits.MaxRings);
        DrawCentered($"Rings {_rings}/{_state.Limits.MaxRings}", 198, 165);

        UiButton.Draw(_segmentsMinus, "-", _segments > 2);
        UiButton.Draw(_segmentsPlus, "+", _segments < _state.Limits.MaxSegments);
        DrawCentered($"Segments {_segments}/{_state.Limits.MaxSegments}", 198, 255);

        UiButton.Draw(_start, "Start");
        UiButton.Draw(_back, "Back");

        if (_preview != null)
        {
            // Vorschau verkleinern, wenn sie sonst nicht ins Fenster passt
            float inner = 50f;
            float ringWidth = 60f;
            float outer = inner + _rings * ringWidth;
            float maxRadius = RL.GetScreenHeight() / 2f - 60f;
            float scale = Math.Min(1f, maxRadius / outer);

            Vector2 center = new(RL.GetScreenWidth() * 0.68f, RL.GetScreenHeight() / 2f);
            MandalaLayout layout = new(center, inner * scale, ringWidth * scale);
            MandalaRenderer.Draw(_preview, layout, _state.Palette);
        }
    }

    private void DrawCentered(string text, int centerX, int y)
    {
        const int fontsize = 22;
        int width = RL.MeasureText(text, fontsize);
        RL.DrawText(text, centerX - width/2, y, fontsize, Color.Black);
    }

    private void RebuildPreview()
    {
        MandalaParameter parameter = _state.Parameter with {RingCount = _rings, SegmentCount = _segments};
        _preview = MandalaGenerator.Generate(parameter);
    }
}