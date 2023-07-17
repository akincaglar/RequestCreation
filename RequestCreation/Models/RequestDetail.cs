namespace RequestCreation.Models
{
    public class RequestDetail
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string CitizenName { get; set; }
        public string? CitizenPhone { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? StatusName { get; set; }
        public string? CreatedName { get; set; }
        public string? Note { get; set; }
    }
}
