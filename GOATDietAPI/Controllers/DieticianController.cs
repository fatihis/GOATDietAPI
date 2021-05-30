using GOATDietAPI.Models;
using GOATDietAPI.Models.DieticianModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOATDietAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DieticianController : Controller
    {
        [HttpGet]
        public IEnumerable<DieticianModel> GetAllDieticians()
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<DieticianModel>("DieticianCollection");
            var document = collection.Find(dietician => dietician.Uid >= 0).ToList();
            return document;
        }

        [HttpGet("{uid}")]
        public DieticianModel GetSingleDieticians(int uid)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<DieticianModel>("DieticianCollection");
            var document = collection.Find(dietician => dietician.Uid == uid ).Limit(1).ToList();
            var singleResult = document.FirstOrDefault();
            return singleResult;
        }

    }
}
