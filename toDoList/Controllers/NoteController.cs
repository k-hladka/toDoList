using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using toDoList.Models;

namespace toDoList.Controllers
{
    public class NoteController : Controller
    {

        INoteRepository _repository;

        public NoteController(INoteRepository repository)
        {
            _repository = repository;
        }
        public ActionResult Index()
        {
            return View(_repository.GetNotes());
        }
        public IActionResult Create()
         {
             return View(_repository.GetCategory());
         }

         [HttpPost]
         public IActionResult Create(Note note, string category)
         {
             _repository.Create(note, category);
             return RedirectToAction("Create");
         }
    }
}
