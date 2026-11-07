namespace Mandacalm.Core;

public record ColorUnlock(PaletteColor Color) : IUnlock
{
    public string Desc => "New Color";

    public void Apply(GameState state) => state.Palette.Add(Color);
}