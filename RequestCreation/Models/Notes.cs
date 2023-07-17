namespace RequestCreation.Models
{
    public class Notes
    {
        public int Id { get; set; } 
        public string Note { get; set; }
        public int RequestsId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
