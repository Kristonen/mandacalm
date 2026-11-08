using System.Numerics;
using Mandacalm.Core;
using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace Mandacalm.Rendering;

public static class MandalaRenderer{
    public static void Draw(Mandala mandala, MandalaLayout layout, Palette palette){
        float segmentAngle = 360f / mandala.Parameters.SegmentCount;

        foreach(Cell cell in mandala.AllCells)
        {
            float cellInner = layout.InnerRadius + cell.Ring * layout.RingWidth;
            float cellOuter = cellInner + layout.RingWidth;
            float startAngle = cell.Segment * segmentAngle;
            float endAngle = startAngle + segmentAngle;

            

            Color fill = Color.LightGray;
            if (cell.IsPainted)
            {
                PaletteColor c = palette.Get(cell.ColorIndex);
                fill = new Color(c.R, c.G, c.B, (byte)255);
            }

            RL.DrawRing(layout.Center, cellInner, cellOuter, startAngle, endAngle, 32, fill);
            RL.DrawRingLines(layout.Center, cellInner, cellOuter, startAngle, endAngle, 32, Color.Black);
        }
    }

    public static void DrawOutlines(Mandala mandala, MandalaLayout layout)
    {
        float segmentAngle = 360f / mandala.Parameters.SegmentCount;
        foreach(var cell in mandala.AllCells)
        {
            float cellInner = layout.InnerRadius + cell.Ring * layout.RingWidth;
            float cellOuter = cellInner + layout.RingWidth;
            float startAngle = cell.Segment * segmentAngle;
            float endAngle = startAngle + segmentAngle;

            RL.DrawRingLines(layout.Center, cellInner, cellOuter, startAngle, endAngle, 32, Color.Black);
        }
    }
}
