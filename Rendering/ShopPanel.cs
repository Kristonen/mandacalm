using System.Numerics;
using Mandacalm.Core;
using RL = Raylib_cs.Raylib;
using Raylib_cs;

namespace Mandacalm.Rendering;

public static class ShopPanel
{
    private const float OfferWidth = 300f;
    private const float OfferHeight = 84f;
    private const float Gap = 12f;

    public static void Draw(Shop shop, Wallet wallet, Vector2 topLeft)
    {
        int hovered = GetIndexAt(shop, topLeft, RL.GetMousePosition());
        var offers = shop.Offers;
        for(int i = 0; i < offers.Count; i++)
        {
            ShopItem offer = offers[i];
            bool buyable = offer.Price <= wallet.Points;
            Rectangle rec = GetOfferRec(i, topLeft);
            int x = (int)rec.X + 12;
            int y = (int)rec.Y;
            RL.DrawRectangleRec(rec, buyable ? Color.RayWhite : Color.Gray);

            string title = offer.MaxPurchases > 1
                ? $"{offer.Name} {offer.PurchaseCount}/{offer.MaxPurchases}"
                : offer.Name;
            RL.DrawText(title, x, y + 8, 20, Color.DarkGray);
            RL.DrawText(offer.Unlock.Desc, x, y + 34, 14, Color.DarkGray);
            RL.DrawText($"{offer.Price} Points", x, y + 56, 16, Color.DarkGray);

            if(buyable && hovered == i)
            {
                RL.DrawRectangleLinesEx(rec, 3, Color.Beige);
            }
        }
    }

    public static int GetIndexAt(Shop shop, Vector2 topLeft, Vector2 point)
    {
        var offers = shop.Offers;
        for(int i = 0; i < offers.Count; i++)
        {
            Rectangle rec = GetOfferRec(i, topLeft);
            if(RL.CheckCollisionPointRec(point, rec))
            {
                return i;
            }
        }
        return -1;
    }

    private static Rectangle GetOfferRec(int index, Vector2 topLeft)
    {
        float y = topLeft.Y + index * (OfferHeight + Gap);
        return new(topLeft.X, y, OfferWidth, OfferHeight);
    }
}