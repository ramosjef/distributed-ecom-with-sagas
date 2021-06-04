namespace Checkout.Domain.Orders;

public sealed class OrderId
{
    public OrderId(long value) => Value = value;

    public long Value { get; set; }

    public bool Equals(OrderId other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is OrderId id && Equals(id);
    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(OrderId left, OrderId right) => left.Equals(right);
    public static bool operator !=(OrderId left, OrderId right) => !(left == right);
    public static bool operator >(OrderId leff, OrderId right) => leff.Value > right.Value;
    public static bool operator <(OrderId leff, OrderId right) => leff.Value < right.Value;
    public static bool operator >=(OrderId leff, OrderId right) => leff.Value >= right.Value;
    public static bool operator <=(OrderId leff, OrderId right) => leff.Value <= right.Value;

    public static implicit operator long(OrderId orderId) => orderId.Value;
    public static implicit operator OrderId(long orderId) => new(orderId);
}
