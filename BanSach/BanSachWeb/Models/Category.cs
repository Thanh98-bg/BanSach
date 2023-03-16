namespace BanSachWeb.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateDatetime { get; set; } = DateTime.Now;
    }
}
