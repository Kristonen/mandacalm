namespace Mandacalm.Core;

public readonly record struct PaletteColor(byte R, byte G, byte B);

public sealed class Palette
{
    private readonly List<PaletteColor> _colors = new();

    public Palette(){
        _colors.Add(new(0, 0, 0));
        _colors.Add(new(255, 255, 255));
        _colors.Add(new(127, 127, 127));
    }

    public int Count => _colors.Count;
    public PaletteColor Get(int index) => index >= 0 && index < _colors.Count ? _colors[index] : _colors[0];
    public void Add(PaletteColor color) => _colors.Add(color);

    public void Restore(IEnumerable<PaletteColor> colors)
    {
        if(colors == null || !colors.Any())
        {
            return;
        }
        _colors.Clear();
        foreach(var color in colors)
        {
            Add(color);
        }
    }
}