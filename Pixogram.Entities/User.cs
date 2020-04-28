using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pixogram.Entities
{
   public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string Id { get; set; }

        [BsonElement("FirstName")]
     //   [Required]
        public  string FirstName { get; set; }

        [BsonElement("LastName")]
      //  [Required]
        public string LastName { get; set; }

        [BsonElement("UserName")]
        [Required]
        public  string UserName { get; set; }

        [BsonElement("Email")]
        [Display(Name = "Email")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public string Email { get; set; }

        [BsonElement("Password")]
     //   [Required]
        public  string Password { get; set; }

        [BsonElement("ConfirmPassword")]
     //   [Required]
        public  string ConfirmPassword { get; set; }

        [BsonElement("ImageUrl")]
        [Display(Name = "Photo")]
        [DataType(DataType.ImageUrl)]
      //  [Required]
        public  string ProfilePicture { get; set; }

    }
}
