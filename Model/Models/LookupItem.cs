using Model.Utils;

namespace Model.Models
{
    public class LookupItem
    {
        public string Name { get; set; }
        public MenuAction Action { get; set; }
        public bool IsForAdmin { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
    }
}
