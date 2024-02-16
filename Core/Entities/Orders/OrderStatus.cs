using System.Runtime.Serialization;

namespace Core.Entities.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment has been received")]
        PaymentReceived,
        [EnumMember(Value = "Payment has failed")]
        PaymentFailed
    }
}
