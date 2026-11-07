using Mandacalm.Core;
using Mandacalm.Persistance;
using Mandacalm.Screen;
using Raylib_cs;
using RL = Raylib_cs.Raylib;

namespace Mandacalm;

public sealed class Game
{
    private readonly GameState _state = new();
    private readonly Dictionary<ScreenState, IScreen> _screens;
    private ScreenState _current = ScreenState.Painting;
    private SaveSystem _saveSystem = new();
    private double _lastSaveTime;

    public Game()
    {
        _screens = new()
        {
            [ScreenState.Painting] = new PaintScreen(_state),
            [ScreenState.Shop] = new ShopScreen(_state),
            [ScreenState.Result] = new ResultScreen(_state),
        };
        GameSnapShot? shot = _saveSystem.Load();
        if(shot != null)
        {
            _state.Restore(shot);
        }
    }
    public void Update()
    {
        if(RL.GetTime() - _lastSaveTime > 30)
        {
            Save();
            _lastSaveTime = RL.GetTime();
        }
        if (RL.IsKeyPressed(KeyboardKey.Tab))
        {
            _current = _current switch
            {
                ScreenState.Painting => ScreenState.Shop,
                ScreenState.Shop => ScreenState.Painting,
                ScreenState.Result => _current,
                _ => throw new ArgumentException(),
            };
        }
        ScreenState? nextScreen = _screens[_current].Update();
        if (nextScreen != null)
        {
            _current = nextScreen.Value;
            Save();
        }
#if DEBUG
        if (RL.IsKeyPressed(KeyboardKey.F3))
        {
            _saveSystem.Save(_state.CreateSnapshot());
            Console.WriteLine("--- Snapshot taken ---");
            PrintState();
        }

        if (RL.IsKeyPressed(KeyboardKey.F4) && _saveSystem != null)
        {
            GameSnapShot? shot = _saveSystem.Load();
            if(shot != null)
            {
                bool ok = _state.Restore(shot);
                Console.WriteLine($"--- Restore: {ok} ---");
                PrintState();
            }
        }
#endif
    }
    public void Draw()
    {
        _screens[_current].Draw();
        RL.DrawText($"Points: {_state.Wallet.Points}", 50, RL.GetScreenHeight() - 50, 20, Color.Black);
    }

    public void Save()
    {
        GameSnapShot shot = _state.CreateSnapshot();
        _saveSystem.Save(shot);
        _lastSaveTime = RL.GetTime();
    }

    private void PrintState()
    {
        Console.WriteLine($"Points: {_state.Wallet.Points}");
        Console.WriteLine($"Seed: {_state.Parameter.Seed}, Rings: {_state.Parameter.RingCount}, Segments: {_state.Parameter.SegmentCount}");
        Console.WriteLine($"Colors in palette: {_state.Palette.Count}");
        Console.WriteLine($"Painted cells: {_state.Mandala.AllCells.Count(c => c.IsPainted)}");
        Console.WriteLine($"Offers: {string.Join(", ", _state.Shop.Offers.Select(o => $"{o.Name} {o.PurchaseCount}/{o.MaxPurchases} ({o.Price})"))}");
    }
}
