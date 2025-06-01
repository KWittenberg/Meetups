namespace Meetups.Enums;

public enum PaymentStatus
{
    Undefined,
    Succeeded,
    Pending,
    Failed,
    Canceled,
    RequiresPaymentMethod,
    RequiresConfirmation,
    RequiresAction
}