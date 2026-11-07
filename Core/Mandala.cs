namespace Mandacalm.Core;

public sealed class Mandala
{
    private readonly Cell[,] _cells;

    public MandalaParameter Parameters { get; }

    public Mandala(MandalaParameter parameters, Cell[,] cells)
    {
        if (cells.GetLength(0) != parameters.RingCount || cells.GetLength(1) != parameters.SegmentCount)
        {
            throw new ArgumentException("Cell grid size does not match the parameters");
        }

        Parameters = parameters;
        _cells = cells;
    }

    public Cell GetCell(int ring, int segment) => _cells[ring, segment];

    public IEnumerable<Cell> AllCells
    {
        get
        {
            foreach (Cell cell in _cells)
            {
                yield return cell;
            }
        }
    }

    public bool IsComplete()
    {
        foreach(var cell in _cells)
        {
            if (!cell.IsPainted)
            {
                return false;
            }
        }
        return true;
    }
}
