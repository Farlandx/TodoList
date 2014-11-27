using System;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;

namespace TodoList.Models
{
    public class DbManager : IDisposable
    {
        private MongoClient _client = null;
        private MongoServer _server = null;
        private MongoDatabase _db = null;
        private static string _connectString = "mongodb://admin:1234@ds055690.mongolab.com:55690/todolist";

        public DbManager()
        {
            _client = new MongoClient(_connectString);
            _server = _client.GetServer();
            _db = _server.GetDatabase("todolist");
        }

        void IDisposable.Dispose()
        {
        }

        /// <summary>
        /// 中斷連線
        /// </summary>
        public void Disconnection()
        {
            _db = null;
            _client = null;
            _server.Disconnect();
        }

        /// <summary>
        /// 取得MongoDb物件集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static MongoCollection<T> GetCollection<T>(string collectionName = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                Type g = typeof(T);
                collectionName = g.Name;
            }
            var result = new MongoClient(_connectString).GetServer().GetDatabase("todolist").GetCollection<T>(collectionName);
            return result;
        }

        #region CRUD
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Create<T>(T obj)
        {
            try { return _db.GetCollection<T>(obj.GetType().Name).Insert(obj).Ok; }
            catch { return false; }
        }

        /// <summary>
        /// 讀取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Read<T>()
        {
            return _db.GetCollection<T>(typeof(T).Name).AsQueryable<T>();
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update<T>(T obj)
        {
            try { return _db.GetCollection<T>(obj.GetType().Name).Save(obj).Ok; }
            catch { return false; }
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete<T>(T obj) where T : Models.BaseModel
        {
            try { return _db.GetCollection<T>(obj.GetType().Name).Remove(Query<T>.EQ(x => x.id, obj.id)).Ok; }
            catch { return false; }
        }
        #endregion

        #region GridFS
        public MongoGridFSFileInfo AddFile(MemoryStream ms, string filename)
        {
            return _db.GridFS.Upload(ms, filename);
        }

        public byte[] GetFileByID(ObjectId id)
        {
            MongoGridFSFileInfo file = _db.GridFS.FindOneById(id);
            if (file.Exists)
            {
                using (var stream = file.OpenRead())
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    return buffer;
                }
            }

            return null;
        }

        public bool DeleteFileByID(ObjectId id)
        {
            try
            {
                MongoGridFSFileInfo file = _db.GridFS.FindOneById(id);
                if (file.Exists) file.Delete();
            }
            catch { return false; }

            return true;
        }

        public MongoCursor<MongoGridFSFileInfo> GetFiles()
        {
            return _db.GridFS.FindAll();
        }
        #endregion
    }
}