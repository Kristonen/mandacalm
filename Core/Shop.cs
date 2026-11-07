namespace Mandacalm.Core;

public sealed class Shop
{
    private readonly List<ShopItem> _items = new();
    public IReadOnlyList<ShopItem> Offers => _items.Where(item => !item.IsSoldOut).ToList();

    public Shop()
    {
        _items.Add(new("More Rings", 50, new RingUnlock(1), 5, 50));
        _items.Add(new("More Segments", 75, new SegmentUnlock(2), 10, 50));
        _items.Add(new("Red", 50, new ColorUnlock(new(255, 0, 0))));
        _items.Add(new("Green", 50, new ColorUnlock(new(0, 255, 0))));
        _items.Add(new("Blue", 50, new ColorUnlock(new(0, 0, 255))));
    }

    

    public bool TryBuy(int index, GameState state)
    {
        if (index < 0 || index >= Offers.Count)
        {
            return false;
        }
        ShopItem item = Offers[index];

        if (!state.Wallet.TrySpend(item.Price))
        {
            return false;
        }

        item.Unlock.Apply(state);
        item.MarkPurchased();
        // if (item.IsSoldOut)
        // {
        //     _items.RemoveAt(index);
        // }
        return true;
    }

    public Dictionary<string, int> GetPurchases()
    {
        Dictionary<string, int> purchases = new();
        foreach(var item in _items)
        {
            purchases[item.Name] = item.PurchaseCount;
        }
        return purchases;
    }

    public void RestorePurchases(Dictionary<string, int> purchases)
    {
        if(purchases == null)
        {
            return;
        }
        foreach(var item in _items)
        {
            if(purchases.TryGetValue(item.Name, out int value))
            {
                item.RestorePurchaseCount(value);
            }
        }
    }
}