namespace Meetups.Extensions;

public static class PaymentStatusExtensions
{
    public static PaymentStatus FromString(this string? status)
    {
        return status switch
        {
            "succeeded" => PaymentStatus.Succeeded,
            "pending" => PaymentStatus.Pending,
            "failed" => PaymentStatus.Failed,
            "canceled" => PaymentStatus.Canceled,
            "requires_payment_method" => PaymentStatus.RequiresPaymentMethod,
            "requires_confirmation" => PaymentStatus.RequiresConfirmation,
            "requires_action" => PaymentStatus.RequiresAction,
            _ => PaymentStatus.Undefined
        };
    }

    public static RefundStatus FromRefundString(this string? status)
    {
        return status switch
        {
            "pending" => RefundStatus.Pending,
            "requires_action" => RefundStatus.RequiresAction,
            "succeeded" => RefundStatus.Succeeded,
            "failed" => RefundStatus.Failed,
            "canceled" => RefundStatus.Canceled,
            _ => RefundStatus.Undefined
        };
    }


    //string status = "succeeded";
    //PaymentStatus paymentStatus = status.FromString(); // Vraća PaymentStatus.Succeeded

    //string refundStatus = "pending";
    //RefundStatus refund = refundStatus.FromRefundString(); // Vraća RefundStatus.Pending
}