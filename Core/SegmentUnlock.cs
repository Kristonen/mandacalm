namespace Mandacalm.Core;

public record SegmentUnlock(int Amount) : IUnlock
{
    public string Desc => $"+ {Amount} maximum segments";

    public void Apply(GameState state) => state.AddSegments(Amount);
}