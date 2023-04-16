namespace toDoList.Models
{
    public interface INoteRepository
    {
        void Create(Note note, string category);
        List<Note> GetNotes();
        List<Category> GetCategory();
    }
}
