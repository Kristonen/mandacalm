namespace Mandacalm.Core;

public static class MandalaGenerator
{
    public static Mandala Generate(MandalaParameter parameter)
    {
        var random = new Random(parameter.Seed);
        var cells = new Cell[parameter.RingCount, parameter.SegmentCount];

        for (int ring = 0; ring < parameter.RingCount; ring++)
        {
            var shape = parameter.AllowedShapes[random.Next(parameter.AllowedShapes.Length)];
            for (int segment = 0; segment < parameter.SegmentCount; segment++)
            {
                cells[ring, segment] = new Cell(ring, segment, shape);
            }
        }
        return new Mandala(parameter, cells);
    }
}
