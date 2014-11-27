using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TodoListController : ApiController
    {
        private DbManager _db = new DbManager();

        // GET: api/TodoList
        /// <summary>
        /// Read all items
        /// </summary>
        /// <returns></returns>
        public IQueryable<TodoListItem> Get()
        {
            //List<TodoListItem> items = _db.Read<TodoListItem>().ToList();
            return _db.Read<TodoListItem>();
        }

        // GET: api/TodoList/5
        /// <summary>
        /// Read item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TodoListItem Get(string id)
        {
            return _db.Read<TodoListItem>().SingleOrDefault(x => x.id == id);
        }

        // POST: api/TodoList
        /// <summary>
        /// Create a item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TodoListItem Post([FromBody]TodoListItem item)
        {
            if (_db.Create<TodoListItem>(item))
            {
                return item;
            }
            return null;
        }

        // PUT: api/TodoList/5
        /// <summary>
        /// Update item by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        public void Put(string id, [FromBody]TodoListItem item)
        {
            TodoListItem oldItem = _db.Read<TodoListItem>().SingleOrDefault(x => x.id == id);
            if (oldItem != null)
            {
                oldItem.complete = item.complete;
                oldItem.text = item.text;
                oldItem.important = item.important;
                _db.Update<TodoListItem>(oldItem);
            }
        }

        // DELETE: api/TodoList/5
        /// <summary>
        /// Delete item by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            TodoListItem item = _db.Read<TodoListItem>().SingleOrDefault(x => x.id == id);
            if (item != null)
            {
                _db.Delete<TodoListItem>(item);
            }
        }
    }
}
