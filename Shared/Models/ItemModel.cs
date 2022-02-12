using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared
{
    class ItemModel
    {
        [Key]
        public int ImageId { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public ItemModel(int ImageId, string Name, string ImageName, int UserId)
        {
            this.ImageId = ImageId;
            this.Name = Name;
            this.ImageName = ImageName;
            this.UserId = UserId;
        }
    }
}
