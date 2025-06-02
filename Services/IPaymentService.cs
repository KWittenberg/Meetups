using Stripe;

namespace Meetups.Services;

public interface IPaymentService
{
    Task<Session> GetCheckoutSessionAsync(string sessionId);

    Task<string> CreateCheckoutSessionAsync(string userEmail, EventDto input, string baseUrl, string cancelUrl);

    Task<Refund> CreateRefundAsync(RsvpDto input);
}