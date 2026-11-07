namespace Mandacalm.Core;

public record RingUnlock(int Amount) : IUnlock
{
    public string Desc => $"+ {Amount} maximum rings";
    public void Apply(GameState state) => state.AddRings(Amount);
}