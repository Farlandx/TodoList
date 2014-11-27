using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace TodoList.Models
{
    public class BaseModel
    {
        [Key]
        [Required]
        public string id { get; protected set; }

        public BaseModel() { this.id = ObjectId.GenerateNewId(DateTime.Now).ToString(); }
        public BaseModel(string init) { this.id = ObjectId.GenerateNewId(DateTime.Now).ToString(); }
    }
}
