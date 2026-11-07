namespace Mandacalm.Core;

public interface IUnlock
{
    string Desc { get; }
    void Apply(GameState state);
}