namespace Meetups.Services;

public class PaymentService() //: IPaymentService
{
    public async Task CreateCheckoutSessionAsync()
    {
        var service = new Stripe.Checkout.SessionService();
        var options = new Stripe.Checkout.SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
            {
                new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Meetup Ticket",
                        },
                        UnitAmount = 2000, // $20.00
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "https://yourdomain.com/success",
            CancelUrl = "https://yourdomain.com/cancel",
        };




        //await using var db = await contextFactory.CreateDbContextAsync();

        //try
        //{
        //    var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
        //    if (user is null) return Result.Error("User not found!");

        //    var eventExist = await db.Events.AnyAsync(x => x.Id == eventId);
        //    if (!eventExist) return Result.Error("Event not found!");

        //    var rsvpExist = await db.Rsvps.AnyAsync(x => x.EventId == eventId && x.UserId == user.Id);
        //    if (rsvpExist) return Result.Error("User has already Rsvped for this event!");

        //    var rsvp = new Rsvp
        //    {
        //        Id = Guid.NewGuid(),
        //        UserId = user.Id,
        //        EventId = eventId,
        //        RsvpDate = DateTime.Now,
        //        Status = RsvpStatus.Going,
        //        PaymentId = paymentId
        //    };

        //    await db.Rsvps.AddAsync(rsvp);
        //    await db.SaveChangesAsync();

        //    return Result.Ok("Rsvp added!");
        //}
        //catch (Exception ex)
        //{
        //    return Result.Error($"Database error: {ex.Message}");
        //}
    }



}