using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
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
            allDogs.Sort((x,y)=> System.String.Compare(x.Rasa, y.Rasa, System.StringComparison.Ordinal));
            return allDogs;
        }

        public Dog GetDog(int id)
        {
            Dog dog = DBComunicator.GetDog(id);
            return dog;
        }

        [HttpPost]
        public HttpStatusCode Izgubio(IzgubioModel model)
        {
            bool isOk = DBComunicator.Insert(model);
            if (isOk)
            {
               return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }

        [HttpGet]
        public List<string> Rase()
        {
            List<string> retlist = DBComunicator.GetRaseAll();
            return retlist;
        }

        [HttpGet]
        public HttpStatusCode Pronasao(int id)
        {
            bool isOk = DBComunicator.Pronasao(id);
            if (isOk)
            {
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }
    }
}
