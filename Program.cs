using RL = Raylib_cs.Raylib;
using Raylib_cs;
using Mandacalm;

const int WIDTH = 1280;
const int HEIGHT = 720;

RL.InitWindow(WIDTH, HEIGHT, "MandaCalm");
RL.SetTargetFPS(60);

Game game = new();
while (!RL.WindowShouldClose())
{
    game.Update();
    RL.BeginDrawing();
    RL.ClearBackground(Color.SkyBlue);
    game.Draw();
    RL.EndDrawing();
}
game.Save();
RL.CloseWindow();