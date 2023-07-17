namespace RequestCreation.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
