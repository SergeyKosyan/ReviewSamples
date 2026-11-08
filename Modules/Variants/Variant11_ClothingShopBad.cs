namespace ReviewSamples.Modules.Variants;

public enum ClothingCustomerType
{
    Regular,
    Vip,
    Gold
}

public class ClothingItem
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class ShoppingCart
{
    public List<ClothingItem> Items { get; } = new();
}

public class ClothingShopFixed
{
    private const decimal RegularDiscount = 0.05m;
    private const decimal VipDiscount = 0.15m;
    private const decimal GoldDiscount = 0.20m;
    private const decimal LargeOrderThreshold = 5000m;
    private const decimal LargeOrderDiscount = 200m;

    public decimal CalculateTotal(ShoppingCart cart, ClothingCustomerType customerType)
    {
        if (cart is null)
            throw new ArgumentNullException(nameof(cart));
        if (cart.Items is null)
            throw new ArgumentException("Список товаров не может быть null.", nameof(cart));

        decimal total = 0m;

        foreach (var item in cart.Items)
        {
            if (item is null)
                throw new ArgumentException("Товар не может быть null.", nameof(cart));
            if (item.Price < 0)
                throw new ArgumentOutOfRangeException(nameof(item.Price), "Цена не может быть отрицательной.");
            if (item.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Количество должно быть положительным.");

            total += item.Price * item.Quantity;
        }

        total -= total * GetDiscountRate(customerType);

        if (total > LargeOrderThreshold)
            total -= LargeOrderDiscount;

        return Math.Max(0, total);
    }

    private static decimal GetDiscountRate(ClothingCustomerType customerType) =>
        customerType switch
        {
            ClothingCustomerType.Regular => RegularDiscount,
            ClothingCustomerType.Vip => VipDiscount,
            ClothingCustomerType.Gold => GoldDiscount,
            _ => 0m
        };
}