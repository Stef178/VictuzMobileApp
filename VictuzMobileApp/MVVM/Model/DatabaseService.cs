using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace VictuzMobileApp.MVVM.View
{
    public class PhotoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
    }

    public class DatabaseService
    {
        private SQLiteConnection _connection;

        public DatabaseService()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "photos.db3");
            _connection = new SQLiteConnection(databasePath);
            _connection.CreateTable<PhotoModel>();
        }

        public void AddPhoto(PhotoModel photo)
        {
            _connection.Insert(photo);
        }

        public List<PhotoModel> GetAllPhotos()
        {
            return _connection.Table<PhotoModel>().ToList();
        }

        public void DeletePhoto(PhotoModel photo)
        {
            _connection.Delete(photo);
        }
    }
}
