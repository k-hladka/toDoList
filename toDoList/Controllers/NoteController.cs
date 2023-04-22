using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Index(bool sort=false, string FieldName="")
        {
            var result = _repository.GetNotes();
            if (sort)
            {
                result = _repository.Sort(FieldName);
            }
            return View(result);
        }
        public IActionResult Create()
         {
             return View(_repository.GetCategories());
         }

         [HttpPost]
         public IActionResult Create(Note note, string category, string typeStorage)
         {
             _repository.Create(note, category, typeStorage);
             return RedirectToAction("Create");
         }
        public IActionResult Completed(bool status, int id)
        {
            _repository.Completed(status, id);
            return RedirectToAction("Index");
        }
        public IActionResult CompletedXml(bool status, int id)
        {
            _repository.CompletedXml(status, id);
            return RedirectToAction("Index");
        }
    }
}
