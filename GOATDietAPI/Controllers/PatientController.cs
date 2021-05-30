using GOATDietAPI.Models;
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
    public class PatientController : Controller
    {
        [HttpGet]
        public IEnumerable<PatientModel> GetAllPatients()
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            var document = collection.Find(patient => patient.Uid >= 1).ToList();
            return document;
        }
        [HttpGet("{uid}")]
        public PatientModel GetSinglePatients(int uid)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            var document = collection.Find(patient => patient.Uid == uid).Limit(1).ToList();
            var singleResult = document.FirstOrDefault();
            return singleResult;
        }
    }
}

