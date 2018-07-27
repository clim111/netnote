using Microsoft.EntityFrameworkCore;
using NetNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetNote.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NetNote.DAL
{
    public class NoteContext:IdentityDbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {}

        public DbSet<Note> Notes { get; set; }

        public DbSet<NetNote.ViewModels.NoteModel> NoteModel { get; set; }

        public DbSet<NoteType> NoteTypes { get; set; }
    }
}
