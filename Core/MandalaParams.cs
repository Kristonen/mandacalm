using System.Collections.Immutable;

namespace Mandacalm.Core;

public enum ShapeType
{
    Circle, Rec
}

public record MandalaParameter(int Seed, int SegmentCount, int RingCount, ImmutableArray<ShapeType> AllowedShapes);
