namespace Mandacalm.Core;

public sealed class Cell{
    public const int Unpainted = -1;

    public int Ring { get; }
    public int Segment { get; }
    public ShapeType Shape { get; }
    public int ColorIndex { get; set; } = Unpainted;
    public bool IsPainted => ColorIndex != Unpainted;

    public Cell(int ring, int segment, ShapeType shape){
        Ring = ring;
        Segment = segment;
        Shape = shape;
    }
}
