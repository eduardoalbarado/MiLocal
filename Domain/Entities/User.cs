namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? B2CUserId { get; set; }
        public string? GmailId { get; set; }
        public string? FacebookId { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
