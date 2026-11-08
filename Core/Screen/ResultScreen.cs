using RL = Raylib_cs.Raylib;
using Raylib_cs;
using Mandacalm.Core;


namespace Mandacalm.Screen;

public sealed class ResultScreen : IScreen
{
    const int MainFontSize = 50;
    const int SubFontSize = 28;
    private readonly GameState _state;
    public ResultScreen(GameState state)
    {
        _state = state;
    }
    public ScreenState? Update()
    {
        if (RL.IsKeyPressed(KeyboardKey.Enter))
        {
            return ScreenState.Menu;
        }
        return null;
    }
    public void Draw()
    {
        string[] lines =
        {
            $"You earn {_state.LastResult?.Total} Points",
            $"Painted Areas: {_state.LastResult?.Base} Points",
            $"Ring Bonus: {_state.LastResult?.RingBonus} Points",
            $"Mirror Bonus: {_state.LastResult?.MirrorBonus} Points",
        };

        int x = RL.GetScreenWidth()/2;
        int y = RL.GetScreenHeight()/2 - (MainFontSize + (lines.Length - 2) * SubFontSize);
        
        for(int i = 0; i < lines.Length; i++)
        {
            int fontsize = i == 0 ? MainFontSize : SubFontSize;
            int width = RL.MeasureText(lines[i], fontsize);
            RL.DrawText(lines[i], x - width/2, y, fontsize, Color.Gold);

            y += fontsize + 15;
        }
    }
}