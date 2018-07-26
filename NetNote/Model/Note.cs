using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetNote.Model
{
    public class NoteUser:IdentityUser
    {

    }

    public class Note
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Create { get; set; }

        public int TypeId { get; set; }
        public NoteType Type { get; set; }

        public string Password { get; set; }

        public string Attachment { get; set; }
    }

    public class NoteType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Note> Notes { get; set; }
    }
}
