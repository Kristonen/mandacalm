namespace Mandacalm.Core;

public sealed class Wallet
{
    public int Points {get; private set;}

    public void Add(int amount) => Points += amount;

    public bool TrySpend(int amount)
    {
        if(amount <= 0 || amount > Points)
        {
            return false;
        }
        Points -= amount;
        return true;
    }
    public void Restore(int points) => Points = Math.Max(0, points);
}