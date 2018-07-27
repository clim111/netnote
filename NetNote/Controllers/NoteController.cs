using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetNote.Repository;
using NetNote.ViewModels;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace NetNote.Controllers
{
    //[Authorize]
    public class NoteController : Controller
    {
        private INoteRepository noteRepository;
        private INoteTypeRepository noteTypeRepository;

        public NoteController(INoteRepository _noteRepository, INoteTypeRepository _noteTypeRepository)
        {
            this.noteRepository = _noteRepository;
            this.noteTypeRepository = _noteTypeRepository;

        }

        // GET: Note
        public IActionResult Index(int pageindex = 1)
        {
            int pagesize = 2;
            //var notes = await noteRepository.ListAsync();
            var notes = noteRepository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            return View(notes.Item1);
        }

        // GET: Note/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var note = await noteRepository.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(note.Password))
            {
                return View();
            }
            return View(note);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int id,string password)
        {
            var note = await noteRepository.GetByIdAsync(id);
            if (!password.Equals(note.Password))
            {
                return BadRequest("密码错误，返回重新输入");
            }
            return View(note);
        }

            // GET: Note/Create
            public async Task<IActionResult> Create()
        {
            var types = await noteTypeRepository.ListAsync();
            ViewBag.Types = types.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() });
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromServices]IHostingEnvironment env, NoteModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string filename = string.Empty;
                if (model.Attachment != null)
                {
                    filename = Path.Combine("files",Guid.NewGuid().ToString() + Path.GetExtension(model.Attachment.FileName));
                    using (var stream = new FileStream(Path.Combine(env.WebRootPath, filename), FileMode.CreateNew))
                    {
                        model.Attachment.CopyTo(stream);
                    }
                }

                await noteRepository.AddAsync(new Model.Note()
                {
                    Title = model.Title,
                    Content = model.Content,
                    Create = model.Create,
                    TypeId = model.Type,
                    Password = model.Password,
                    Attachment = filename

                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Note/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Note/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}