using Stripe;

namespace Meetups.Services;

public class PaymentService(IConfiguration configuration) : IPaymentService
{
    readonly string stripeApiKey = configuration["Stripe:SecretKey"] ?? string.Empty;


    public async Task<Session> GetCheckoutSessionAsync(string sessionId)
    {
        StripeConfiguration.ApiKey = stripeApiKey;
        var service = new SessionService();
        var session = await service.GetAsync(sessionId);

        return session;
    }

    public async Task<string> CreateCheckoutSessionAsync(string userEmail, EventDto input, string baseUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur", // or "usd", "eur", "gbp", etc.
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = input.Title,
                            // Images = new List<string> { $"{baseUrl.TrimEnd('/')}/img/events/{input.ImageUrl}" },
                            // Images = new List<string> { "https://bolta.runasp.net/img/shop/2004%20Pjesme/_01.jpg" },
                            Description = $"Date and Time: {input.Start.ToString("dddd - dd. MMMM. yyyy. u HH:mm'h'")}"
                        },
                        UnitAmount = (long)(input.TicketPrice!.Value * 100), // in Cents - if TicketPrice = 5€ => 500 cent, // 5.00€
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = $"{baseUrl.TrimEnd('/')}/payment-success/{input.Id}/{{CHECKOUT_SESSION_ID}}",
            CancelUrl = cancelUrl,
            CustomerEmail = userEmail,
            // PaymentMethodTypes = new List<string> { "card", "ideal", "sepa_debit" }, // Dodate metode plaćanja
            BillingAddressCollection = "required", // Zahtevaj adresu za naplatu
            // Locale = "hr", // Hrvatski jezik za Checkout
            Metadata = new Dictionary<string, string> // Dodaj metapodatke
            {
                { "user_email", userEmail },
                { "event_id", input.Id.ToString() },
                { "event_title", input.Title }
            }
        };

        StripeConfiguration.ApiKey = stripeApiKey;
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}