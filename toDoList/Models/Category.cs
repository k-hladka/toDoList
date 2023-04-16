﻿namespace toDoList.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Note> Note { get; } = new List<Note>();
    }
}
