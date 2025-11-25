namespace ECommerceBW.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Cover { get; set; }
        public decimal? Price { get; set; }
        public int Amount { get; set; }
    }
}
