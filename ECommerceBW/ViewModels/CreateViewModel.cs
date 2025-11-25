namespace ECommerceBW.ViewModels
{
    public class CreateViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Cover { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public decimal? Price { get; set; }
    }
}
