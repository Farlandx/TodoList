using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoList.Models
{
    public class TodoListItem : BaseModel
    {
        public bool complete { set; get; }
        public string text { set; get; }
        public bool important { set; get; }
    }
}