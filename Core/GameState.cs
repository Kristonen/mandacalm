using System.Collections.Immutable;
using Mandacalm.Screen;

namespace Mandacalm.Core;

public sealed class GameState
{
    public MandalaParameter Parameter {get; private set;}
    public Mandala Mandala {get; private set;}
    public Wallet Wallet {get; init;} = new();
    public Palette Palette {get; init;} = new();
    public Shop Shop {get; init;} = new();
    public ScoreResult? LastResult {get; private set;}
    public MandalaLimits Limits { get; private set; }
    public GameState()
    {
        Parameter = new(42, 4, 2, [ShapeType.Circle, ShapeType.Rec]);
        Mandala = MandalaGenerator.Generate(Parameter);
        Limits = new(2, 4);
    }

    public void NextMandala()
    {
        Parameter = Parameter with { Seed = Parameter.Seed + 1};
        Mandala = MandalaGenerator.Generate(Parameter);
    }

    public bool TrySubmit()
    {
        if (Mandala.IsComplete())
        {
            ScoreResult result = ScoreCalculator.Calculate(Mandala);
            LastResult = result;
            Wallet.Add(LastResult.Total);
            NextMandala();
            return true;
        }
        return false;
    }

    public void StartMandala(int rings, int segments, int seed)
    {
        rings = Math.Clamp(rings, 1, Limits.MaxRings);
        segments = Math.Clamp(segments, 1, Limits.MaxSegments);
        segments -= segments % 2; //always even

        Parameter = Parameter with {Seed = seed, RingCount = rings, SegmentCount = segments};
        Mandala = MandalaGenerator.Generate(Parameter);
    }

    public void AddRings(int amount) => Limits = Limits with {MaxRings = Limits.MaxRings + amount};
    public void AddSegments(int amount) => Limits = Limits with {MaxSegments = Limits.MaxSegments + amount};

    public GameSnapShot CreateSnapshot()
    {
        GameSnapShot shot = new()
        {
            Points = Wallet.Points,
            Seed = Parameter.Seed,
            RingCount = Parameter.RingCount,
            MaxRings = Limits.MaxRings,
            SegmentCount = Parameter.SegmentCount,
            MaxSegments = Limits.MaxSegments,
            AllowedTypes = Parameter.AllowedShapes.ToList(),
        };
        
        for(int i = 0; i < Palette.Count; i++)
        {
            shot.Colors.Add(Palette.Get(i));
        }

        foreach(var cell in Mandala.AllCells)
        {
            shot.CellColors.Add(cell.ColorIndex);
        }
        shot.ShopPurchases = Shop.GetPurchases();
        return shot;
    }

    public bool Restore(GameSnapShot shot)
    {
        if(shot.Version != 1) return false;
        if(shot.RingCount <= 0 || shot.RingCount > 20) return false;
        if(shot.SegmentCount <= 0 || shot.SegmentCount > 100 || shot.SegmentCount % 2 != 0) return false;
        if(shot.AllowedTypes == null || shot.AllowedTypes.Count == 0) return false;
        if(shot.CellColors == null || shot.Colors == null || shot.ShopPurchases == null) return false;
        Limits = new MandalaLimits(
            Math.Clamp(Math.Max(shot.MaxRings, shot.RingCount), 1, 20),
            Math.Clamp(Math.Max(shot.MaxSegments, shot.SegmentCount), 2, 100)
        );

        MandalaParameter para = new(shot.Seed, shot.SegmentCount, shot.RingCount, shot.AllowedTypes.ToImmutableArray());
        Mandala newMandala = MandalaGenerator.Generate(para);
        if(shot.CellColors.Count == shot.RingCount * shot.SegmentCount)
        {
            int i = 0;
            foreach(var cell in newMandala.AllCells)
            {
                cell.ColorIndex = shot.CellColors[i];
                i++;
            }
        }
        
        Wallet.Restore(shot.Points);
        Palette.Restore(shot.Colors);
        Shop.RestorePurchases(shot.ShopPurchases);
        Parameter = para;
        Mandala = newMandala;
        return true;
    }
}