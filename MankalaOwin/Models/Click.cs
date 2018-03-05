namespace MankalaOwin.Models
{
    public class Click
    {
        public long ID { get; set; }
        public BoxName BoxName { get; set; }
        public bool Reset { get; set; }
    }

    public enum BoxName { A1, A2, A3, A4, A5, A6, AG, B1, B2, B3, B4, B5, B6, BG }
}