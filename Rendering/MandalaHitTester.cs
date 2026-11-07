using System.Numerics;
using Mandacalm.Core;

namespace Mandacalm.Rendering;

public static class MandalaHitTester
{
    public static Cell? GetCellAt(Mandala mandala, MandalaLayout layout, Vector2 point)
    {
        Vector2 offset = point - layout.Center;
        float distance = offset.Length();

        int ringCount = mandala.Parameters.RingCount;
        float outerRadius = layout.InnerRadius + ringCount * layout.RingWidth;

        if (distance < layout.InnerRadius || distance >= outerRadius)
        {
            return null;
        }

        int ring = (int)((distance - layout.InnerRadius) / layout.RingWidth);
        ring = Math.Min(ring, ringCount - 1);

        float angle = MathF.Atan2(offset.Y, offset.X) * 180f / MathF.PI;
        if(angle < 0)
        {
            angle += 360f;
        }

        int segmentCount = mandala.Parameters.SegmentCount;
        int segment = (int)(angle / (360f / segmentCount));
        segment = Math.Min(segment, segmentCount - 1);
        return mandala.GetCell(ring, segment);
    }
}