using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageService.Database
{
    public class Image
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public byte[] Blob { get; set; }
        [Required]
        public int LinkedKey { get; set; }
        [Required]
        public LinkedTableType LinkedTableType { get; set; }
    }
}
