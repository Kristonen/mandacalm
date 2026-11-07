namespace Mandacalm.Core;

public record ShopItemRecord(string Name, int Price, IUnlock Unlock);

public sealed class ShopItem
{
    public string Name {get;}
    public int PurchaseCount {get; private set;}
    public int MaxPurchases {get;}
    public IUnlock Unlock {get;}
    private readonly int _basePrice;
    private readonly int _priceStep;

    public ShopItem(string name, int basePrice, IUnlock unlock, int maxPurchases = 1, int priceStep = 0)
    {
        Name = name;
        MaxPurchases = maxPurchases;
        _basePrice = basePrice;
        _priceStep = priceStep;
        Unlock = unlock;
    }
    public int Price => _basePrice + PurchaseCount * _priceStep;
    public bool IsSoldOut => PurchaseCount >= MaxPurchases;
    public void MarkPurchased() => PurchaseCount++;
    public void RestorePurchaseCount(int count) => PurchaseCount = Math.Clamp(count, 0, MaxPurchases);
}