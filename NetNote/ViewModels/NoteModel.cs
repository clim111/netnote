using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetNote.ViewModels
{
    public class NoteModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "标题")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }
        public DateTime Create { get; set; }

        [Display(Name ="类型")]
        public int Type { get; set; }

        [Display(Name ="密码")]
        public string Password { get; set; }

        [Display(Name ="附件")]
        [NotMapped]
        public IFormFile Attachment { get; set; }
    }
}
