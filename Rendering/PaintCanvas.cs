using Raylib_cs;
using RL = Raylib_cs.Raylib;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Reflection.Metadata.Ecma335;

namespace Mandacalm.Rendering;

public sealed class PaintCanvas : IDisposable
{
    public static readonly Color Paper = Color.LightGray;
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

    public Image ReadImage()
    {
        Image image = RL.LoadImageFromTexture(_texture.Texture); 
        RL.ImageFlipVertical(ref image);
        return image;
    }

    public bool Export(string path)
    {
        string temp = Path.ChangeExtension(path, ".tmp.png");
        Image image = ReadImage();
        bool ok = RL.ExportImage(image, temp);
        RL.UnloadImage(image);
        if (!ok)
        {
            return false;
        }

        try
        {
            File.Move(temp, path, true);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }

    public bool Import(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        Image image = RL.LoadImage(path);
        if(image.Width != _width || image.Height != _height)
        {
            RL.UnloadImage(image);
            return false;
        }

        Texture2D texture = RL.LoadTextureFromImage(image);
        RL.UnloadImage(image);

        RL.BeginTextureMode(_texture);
        RL.ClearBackground(Paper);
        RL.DrawTexture(texture, 0, 0, Color.White);
        RL.EndTextureMode();

        RL.UnloadTexture(texture);
        return true;
    }
}