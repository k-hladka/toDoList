﻿namespace toDoList.Models
{
    public interface INoteRepository
    {
        void Create(Note note, string category);
        void Completed(bool status, int id);
        List<Note> Sort(string FieldName);
        List<Note> GetNotes(string sqlString = "SELECT * FROM Notes");
        List<Category> GetCategories();
    }
}
