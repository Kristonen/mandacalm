using System.Numerics;
using Mandacalm.Core;
using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace Mandacalm.Rendering;

public static class PaletteBar
{
    private const float Swatch = 48f;
    private const float Gap = 12f;

    public static void Draw(Palette palette, int selectedIndex, Vector2 topLeft)
    {
        int hovered = GetIndexAt(palette, topLeft, RL.GetMousePosition());
        for(int i = 0; i < palette.Count; i++)
        {
            PaletteColor c = palette.Get(i);
            Rectangle rec = GetSwatchRec(i, topLeft);
            RL.DrawRectangleRec(rec, new(c.R, c.G, c.B, (byte)255));
            if (selectedIndex == i)
            {
                RL.DrawRectangleLinesEx(rec, 5, Color.Orange);
            }
            else if(hovered == i)
            {
                RL.DrawRectangleLinesEx(rec, 3, Color.Beige);
            }
        }
    }

    public static int GetIndexAt(Palette palette, Vector2 topLeft, Vector2 point)
    {
        for(int i = 0; i < palette.Count; i++)
        {
            Rectangle rec = GetSwatchRec(i, topLeft);
            if(RL.CheckCollisionPointRec(point, rec))
            {
                return i;
            }
        }
        return -1;
    }

    private static Rectangle GetSwatchRec(int index, Vector2 topLeft)
    {
        float x = topLeft.X + index * (Swatch + Gap);
        return new(x, topLeft.Y, Swatch, Swatch);
    }
}
