namespace toDoList.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? FinalDate { get; set; }
        public bool Status { get; set; } = false;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
