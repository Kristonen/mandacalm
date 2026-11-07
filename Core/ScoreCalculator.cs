namespace Mandacalm.Core;

public static class ScoreCalculator
{
    private const int PointsPerCell = 10;
    private const int PointsPerUniformingRing = 30;
    private const int PointsPerMirrorPair = 15;

    public static ScoreResult Calculate(Mandala mandala)
    {
        int sum = 0;
        foreach(var cell in mandala.AllCells)
        {
            if (cell.IsPainted)
            {
                sum += PointsPerCell;
            }
        }

        int uniformCount = CountUniformRings(mandala);
        int mirrorPairCount = CountMirrorPairs(mandala);

        return new(sum, PointsPerUniformingRing * uniformCount, PointsPerMirrorPair * mirrorPairCount);
    }

    private static int CountUniformRings(Mandala mandala)
    {
        int count = 0;
        int rings = mandala.Parameters.RingCount;

        for(int ring = 0; ring < rings; ring++)
        {
            if (IsRingUniform(mandala, ring))
            {
                count++;
            }
        }

        return count;
    }

    private static int CountMirrorPairs(Mandala mandala)
    {
        int segments = mandala.Parameters.SegmentCount;
        if(segments % 2 != 0)
        {
            return 0;
        }

        int half = segments / 2;
        int count = 0;

        for(int ring = 0; ring < mandala.Parameters.RingCount; ring++)
        {
            if(IsRingUniform(mandala, ring))
            {
                continue;
            }
            for(int segment = 0; segment < half; segment++)
            {
                Cell a = mandala.GetCell(ring, segment);
                Cell b = mandala.GetCell(ring, segment + half);

                if(a.IsPainted && a.ColorIndex == b.ColorIndex)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static bool IsRingUniform(Mandala mandala, int ring)
    {
        int segments = mandala.Parameters.SegmentCount;
        int firstColor = mandala.GetCell(ring, 0).ColorIndex;
        bool uniform = firstColor != Cell.Unpainted;
        for(int segment = 0; segment < segments; segment++)
        {
            if(mandala.GetCell(ring, segment).ColorIndex != firstColor)
            {
                uniform = false;
                break;
            }
        }
        return uniform;
    }
}

