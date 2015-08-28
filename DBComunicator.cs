using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Providers.Entities;
using dogfinderapi2.Models;
using MySql.Data.MySqlClient;

namespace dogfinderapi2
{
    public class DBComunicator
    {
        #region Properties
       
        readonly static MemoryCache memoryCache = MemoryCache.Default;
        private static List<Dog> Dogs
        {
            get
            {
                if (memoryCache["dogs"] == null)
                {
                    memoryCache["dogs"] = getAll();
                }
                return memoryCache["dogs"] as List<Dog>;
            }
        }
        private static List<DogPreview> DogsPreview
        {
            get
            {
                if (memoryCache["DogsPreview"] == null)
                {
                    memoryCache["DogsPreview"] = getAllDogPreview();
                }
                return memoryCache["DogsPreview"] as List<DogPreview>;
            }
        }
        private static readonly string MySqlConnectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
        public static MySqlConnection mySqlConnection = new MySqlConnection(MySqlConnectionString);
        #endregion

        #region API

        public static List<DogPreview> GetAll()
        {
            return DogsPreview;
        }

        public static Dog GetDog(int id)
        {
            Dog dog = Dogs.FirstOrDefault(x => x._Id == id.ToString());
            return dog;
        }

        public static bool Insert(IzgubioModel dogModel)
        {
            int numOfColumns = 0;
            try
            {
                byte[] slika = Convert.FromBase64String(dogModel.slika1);
                dogModel.slika1 = Encoding.UTF8.GetString(slika);

                slika = Convert.FromBase64String(dogModel.slika2);
                dogModel.slika2 = Encoding.UTF8.GetString(slika);
                mySqlConnection.Open();
                string sql = string.Format(@"insert into dogfinderdb.dogs(name, weight, description,kind, slika1, slika2, grad)
                                             values('{0}',{1},'{2}', '{3}', '{4}', '{5}', '{6}')", dogModel.ime, dogModel.tezina, dogModel.info,
                    dogModel.rasa, dogModel.slika1, dogModel.slika2, dogModel.grad);
                MySqlCommand command = new MySqlCommand(sql, mySqlConnection);
                numOfColumns = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                mySqlConnection.Close();
            }
            if (numOfColumns != 0)
            {
                RefreshCache();
            }
           return numOfColumns != 0;
        }
        #endregion

        #region Helpers

        private static List<Dog> getAll()
        {
            List<Dog> allDogs = new List<Dog>();
            try
            {
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand("Select * from dogs", mySqlConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dog dog = new Dog();
                        dog._Id = reader.GetString(0);
                        dog.Name = reader.GetString("Name");
                        dog.Slika1 = GetSlika(reader, 1);
                        dog.Age = reader.GetString("Age");
                        dog.Weight = reader.GetString("Weight");
                        dog.Height = reader.GetString("Height");
                        dog.Description = reader.GetString("Description");
                        dog.Found = reader.GetString("Found");
                        dog.dateString = reader.GetString("dateString");
                        dog.Deleted = reader.GetString("Deleted");
                        dog.Vlasnik = reader.GetString("Vlasnik");
                        dog.Email = reader.GetString("Email");
                        dog.Mob = reader.GetString("Mob");
                        dog.Slika2 = GetSlika(reader, 2);
                        dog.Opstina = reader.GetString("Opstina");
                        dog.Grad = reader.GetString("Grad");
                        dog.Drzava = reader.GetString("Drzava");
                        dog.Vlasnik = reader.GetString("Vlasnik");

                        allDogs.Add(dog);
                    }
                }
            }
            finally
            {
                mySqlConnection.Close();
            }
            return allDogs;
        }

        private static List<DogPreview> getAllDogPreview()
        {
            List<DogPreview> allDogs = new List<DogPreview>();
            try
            {
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand("Select * from dogs", mySqlConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DogPreview dog = new DogPreview();
                        dog._Id = reader.GetString(0);
                        dog.Name = reader.GetString("Name");
                        dog.Slika1 = reader.GetString("Slika1");//GetSlika(reader, 1);
                        allDogs.Add(dog);
                    }
                }
            }
            finally
            {
                mySqlConnection.Close();
            }
            return allDogs;
        }

        private static string GetSlika(MySqlDataReader reader, int numSlika)
        {
            int column = reader.GetOrdinal("Slika" + numSlika);
            if (numSlika == 1)
            {
                long size = reader.GetBytes(column, 0, null, 0, 0);
                byte[] buffer = new byte[size];
                reader.GetBytes(14, 0, buffer, 0, (int)size);
                return Convert.ToBase64String(buffer);
            }
            else
            {

                long size = reader.GetBytes(column, 0, null, 0, 0);
                if (size == 0)
                {//moze da ima samo jednu sliku
                    return string.Empty;
                }
                byte[] buffer = new byte[size];
                reader.GetBytes(15, 0, buffer, 0, (int)size);
                string base64String_Slika1 = Convert.ToBase64String(buffer);
                return base64String_Slika1;
            }
            
        }


        private static void RefreshCache()
        {
            memoryCache["DogsPreview"] = null;
            memoryCache["dogs"] = null;
        }
        #endregion

    }
}