namespace Mandacalm.Screen;

public enum ScreenState
{
    Painting, Shop, Result
}

public interface IScreen
{
    ScreenState? Update();
    void Draw();
}