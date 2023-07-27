namespace RequestCreation.Models
{
    public class Requests
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public int PersonsId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
