namespace Mandacalm.Core;

public sealed class GameSnapShot
{
    public int Version { get; set; } = 1;
    public int Points { get; set; }
    public List<PaletteColor> Colors { get; set; } = new();
    public int Seed { get; set; }
    public int RingCount { get; set; }
    public int MaxRings {get; set; }
    public int SegmentCount { get; set; }
    public int MaxSegments { get; set; }
    public List<ShapeType> AllowedTypes { get; set; } = new();
    public List<int> CellColors { get; set; } = new();
    public Dictionary<string, int> ShopPurchases { get; set; } = new();
}