using Raylib_cs;
using RL = Raylib_cs.Raylib;

public static class UiButton
{
    public static void Draw(Rectangle rec, string label, bool enabled = true)
    {
        bool hover = enabled && RL.CheckCollisionPointRec(RL.GetMousePosition(), rec);
        Color back = !enabled ? Color.Gray : hover ? Color.White : Color.RayWhite;

        RL.DrawRectangleRec(rec, back);
        RL.DrawRectangleLinesEx(rec, 2, Color.DarkGray);

        const int fontSize = 24;
        int width = RL.MeasureText(label, fontSize);
        RL.DrawText(label, (int)(rec.X + rec.Width/2- width/2), (int)(rec.Y + rec.Height/2 - fontSize/2), fontSize, Color.DarkGray);
    }

    public static bool IsClicked(Rectangle rec, bool enabled = true)
    {
        return enabled
            && RL.IsMouseButtonPressed(MouseButton.Left)
            && RL.CheckCollisionPointRec(RL.GetMousePosition(), rec);
    }
}