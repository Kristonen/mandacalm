using Raylib_cs;

namespace Mandacalm.Core;

public static class ColorConvert
{
    public static Color ToRaylib(PaletteColor c) => new(c.R, c.G, c.B, (byte)255);
}