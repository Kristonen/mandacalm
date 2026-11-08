using Mandacalm.Core;
using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace Mandacalm.Rendering;

public static class CanvasSampler
{
    private const int SamplesPerAxis = 2;
    private const float Margin = 0.15f;
    private const float MinCoverage = 0.5f;
    private const int MaxColorDistance = 40;

    public static int[] Sample(Image image, Mandala mandala, MandalaLayout layout, Palette palette)
    {
        float segmentAngle = 360f / mandala.Parameters.SegmentCount;
        int totalSamples = SamplesPerAxis * SamplesPerAxis;
        List<int> result = new();

        foreach (Cell cell in mandala.AllCells)
        {
            float innerR = layout.InnerRadius + cell.Ring * layout.RingWidth;
            float outerR = innerR + layout.RingWidth;
            float startA = cell.Segment * segmentAngle;
            float endA = startA + segmentAngle;

            int[] counts = new int[palette.Count];

            for (int r = 0; r < SamplesPerAxis; r++)
            {
                float tr = Margin + (1f - 2f * Margin) * (r + 0.5f) / SamplesPerAxis;
                float radius = innerR + (outerR - innerR) * tr;

                for (int a = 0; a < SamplesPerAxis; a++)
                {
                    float ta = Margin + (1f - 2f * Margin) * (a + 0.5f) / SamplesPerAxis;
                    float angle = (startA + (endA - startA) * ta) * MathF.PI / 180f;

                    int x = (int)(layout.Center.X + radius * MathF.Cos(angle));
                    int y = (int)(layout.Center.Y + radius * MathF.Sin(angle));
                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
                    {
                        continue;
                    }

                    int index = NearestPaletteIndex(RL.GetImageColor(image, x, y), palette);
                    if (index >= 0)
                    {
                        counts[index]++;
                    }
                }
            }

            int best = 0;
            for (int i = 1; i < counts.Length; i++)
            {
                if (counts[i] > counts[best])
                {
                    best = i;
                }
            }
            result.Add(counts[best] >= MinCoverage * totalSamples ? best : Cell.Unpainted);
        }

        return result.ToArray();
    }

    private static int NearestPaletteIndex(Color c, Palette palette)
    {
        int best = -1;
        int bestDistance = MaxColorDistance + 1;
        for (int i = 0; i < palette.Count; i++)
        {
            PaletteColor p = palette.Get(i);
            int distance = Math.Abs(c.R - p.R) + Math.Abs(c.G - p.G) + Math.Abs(c.B - p.B);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = i;
            }
        }
        return best;
    }
}