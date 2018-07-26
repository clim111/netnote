using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetNote.DAL;
using NetNote.Model;

namespace NetNote.Repository
{
    public class NoteRepository : INoteRepository
    {
        private NoteContext context;
        public NoteRepository(NoteContext _context)
        {
            context = _context;
        }

        public Task AddAsync(Note note)
        {
            context.Notes.Add(note);
            return context.SaveChangesAsync();
        }

        public Task<Note> GetByIdAsync(int id)
        {
            return context.Notes.Include(p=>p.Type).FirstOrDefaultAsync(r =>r.Id == id);
        }

        public Task<List<Note>> ListAsync()
        {
            return context.Notes.Include(t=>t.Type).ToListAsync();
        }

        public Tuple<List<Note>, int> PageList(int pageindex, int pagesize)
        {
            var query = context.Notes.Include(t => t.Type).AsQueryable();
            int count = query.Count();
            int pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var notes = query.OrderByDescending(r=>r.Create).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList();
            return new Tuple<List<Note>, int>(notes,pagecount);
        }

        public Task updateAsync(Note note)
        {
            context.Entry(note).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }
    }
}
