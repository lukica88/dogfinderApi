using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using dogfinderapi2.Models;

namespace dogfinderapi2.Controllers
{
    public class DogController : ApiController
    {
        // GET api/dog
        public IEnumerable<DogPreview> GetAll()
        {
            List<DogPreview> allDogs = DBComunicator.GetAll();
            return allDogs;
        }

        public Dog GetDog(int id)
        {
            Dog dog = DBComunicator.GetDog(id);
            return dog;
        }

        [HttpPost]
        public void Izgubio(IzgubioModel model)
        {
            bool isOk = DBComunicator.Insert(model);
        }

        [HttpGet]
        public List<string> Rase()
        {
            List<string> retlist = new List<string>{"PitBul", "Rotvajler"};
            return retlist;
        }
    }
}
