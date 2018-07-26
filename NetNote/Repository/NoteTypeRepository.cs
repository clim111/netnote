using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetNote.DAL;
using NetNote.Model;

namespace NetNote.Repository
{
    public class NoteTypeRepository : INoteTypeRepository
    {
        private NoteContext context;
        public NoteTypeRepository(NoteContext _context)
        {
            context = _context;
        }

        public Task<List<NoteType>> ListAsync()
        {
            return context.NoteTypes.ToListAsync();
        }
    }
}
