using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetNote.Repository;

namespace NetNote.API
{
    [Route("api/[controller]")]
    public class NoteController : Controller
    {
        private INoteRepository noteRepository;
        private INoteTypeRepository noteTypeRepository;

        public NoteController(INoteRepository _noteRepository, INoteTypeRepository _noteTypeRepository)
        {
            noteRepository = _noteRepository;
            noteTypeRepository = _noteTypeRepository;
        }

        // GET: Note/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(int pageindex = 1)
        {
            var pagesize = 10;
            var notes = noteRepository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            var result = notes.Item1.Select(r => new Model.Note
            {
                Id = r.Id,
                Title = r.Title,
                Content = string.IsNullOrEmpty(r.Password) ? r.Content : "加密内容",
                Attachment = string.IsNullOrEmpty(r.Password) ? r.Attachment : ""
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id, string password)
        {
            var note = await noteRepository.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(note.Password) && !note.Password.Equals(password))
                {
                    return Unauthorized();
                }
                var result = new ViewModels.NoteModel()
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    //Attachment =new FileStream(note.Attachment,FileMode.Open,FileAccess.Read),
                    Type = note.Type.Id
                };
                return Ok(result);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ViewModels.NoteModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string filename = string.Empty;
            await noteRepository.AddAsync(new Model.Note()
            {
                Title = model.Title,
                Content = model.Content,
                Create = DateTime.Now,
                TypeId = model.Type,
                Password = model.Password,
                Attachment = filename
            });
            return CreatedAtAction("Index", "");
        }
    }
}