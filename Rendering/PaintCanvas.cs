using Raylib_cs;
using RL = Raylib_cs.Raylib;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Mandacalm.Rendering;

public sealed class PaintCanvas : IDisposable
{
    private static readonly Color Paper = Color.LightGray;
    private RenderTexture2D _texture;
    private readonly int _width;
    private readonly int _height;

    public PaintCanvas(int width, int height)
    {
        _width = width;
        _height = height;
        _texture = RL.LoadRenderTexture(width, height);
        Clear();
    }

    public void Clear()
    {
        RL.BeginTextureMode(_texture);
        RL.ClearBackground(Paper);
        RL.EndTextureMode();
    }

    public void Stroke(Vector2 from, Vector2 to, float radius, Color color)
    {
        RL.BeginTextureMode(_texture);
        RL.DrawLineEx(from, to, radius * 2f, color);
        RL.DrawCircleV(from, radius, color);
        RL.DrawCircleV(to, radius, color);
        RL.EndTextureMode();
    }

    public void Draw()
    {
        Rectangle source = new(0, 0, _width, -_height);
        RL.DrawTextureRec(_texture.Texture, source, Vector2.Zero, Color.White);
    }

    public void Dispose()
    {
        RL.UnloadRenderTexture(_texture);
    }
}