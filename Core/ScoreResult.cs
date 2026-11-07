namespace Mandacalm.Core;

public record ScoreResult(int Base, int RingBonus, int MirrorBonus)
{
    public int Total => Base + RingBonus + MirrorBonus;
}