namespace Mandacalm.Screen;

public enum ScreenState
{
    Painting, Shop, Result, Menu, Generator
}

public interface IScreen
{
    ScreenState? Update();
    void Draw();
    void OnEnter() {}
}