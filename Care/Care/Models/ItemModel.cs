
using Microsoft.AspNetCore.Http;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Care.Models
{
    public class ItemModel
    {
        [Key]
        public int ImageId { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        public string Category { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public MediaFile ImageFile { get; set; }

        public ItemModel(string Name, string ImageName, int UserId, string Condition, string Category)
        {
            this.Name = Name;
            this.ImageName = ImageName;
            this.UserId = UserId;
            this.Category = Category;
            this.Condition = Condition;
        }

        public ItemModel()
        {
        }
    }
}
