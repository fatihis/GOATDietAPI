using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB;
using GOATDietAPI.Models;
using MongoDB.Driver;
using GOATDietAPI.Helpers;
using GOATDietAPI.Models.DieticianModel;
using System;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace GOATDietAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("PhoneAuthentication/{uid}")]
        public AuthToken GetAuthTokenPhone(int uid)
        {
            
            return new AuthToken()
            {
                Token = SmsApiHelper.getAuthToken(),
                CreationTime = DateTime.Now,
            };
            
        }
        [HttpPost("PhoneAuthentication/Confirmation/{uid}")]
        public Boolean SetAuthPhone(int uid)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            var filter = Builders<PatientModel>.Filter.Eq("Uid", uid);
            var updateAuthStatus = Builders<PatientModel>.Update.Set("PhoneAuthenticated", true);
            try
            {
                collection.UpdateOne(filter, updateAuthStatus);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        [HttpPost("CreatePatient")]
        public PatientModel AddNewPatient(PatientModel newPatient)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            //PatientModel np = new PatientModel()
            //{
            //    _Id = ObjectId.GenerateNewId(),
            //    Uid = 000001,
            //    Name = "Pedat",
            //    Surname = "Seker",
            //    Email = "cincinao@gmail.com",
            //    PhoneNumber = "50330202011",
            //    Password = AesOperation.EncryptString(Secrets.SymmetricKeyOAuth, "prolaterya"),
            //    UserType = "Patient",
            //    Sex = "no sex",
            //    Weight = 102.5,
            //    Height = 190.2,
            //    Pregnancy = true,
            //    ThinnestWaist = 14.2,
            //    TightestNeck = 11.2,
            //    Age = 12,       };

            PatientModel newPatientModel = new PatientModel()
            {
                _Id = ObjectId.GenerateNewId(),
                Uid = newPatient.Uid,
                Name = newPatient.Name,
                Surname = newPatient.Surname,
                Email = newPatient.Email,
                PhoneNumber = newPatient.PhoneNumber,
                Password = AesOperation.EncryptString(Secrets.SymmetricKeyOAuth, newPatient.Password),
                UserType = "Patient",
                Sex = newPatient.Sex,
                Weight = newPatient.Weight,
                Height = newPatient.Height,
                Pregnancy = newPatient.Pregnancy,
                ThinnestWaist = newPatient.ThinnestWaist,
                TightestNeck = newPatient.TightestNeck,
                Age = newPatient.Age,
            };

            collection.InsertOneAsync(newPatientModel);
            return newPatientModel;

        }
        [HttpPost("CreateDietician")]
        public DieticianModel AddNewDietician(DieticianModel newDietician)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<DieticianModel>("DieticianCollection");
            collection.InsertOneAsync(newDietician);
            return newDietician;

        }
        [HttpPost("LoginPatient")]
        public PatientModel LoginPatient(IBaseUser userInformation)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            var results = collection.Find(x => x.Email == userInformation.Email).Limit(1).ToList();
            var singleResult = results.FirstOrDefault();
            if (singleResult != null)
            {
                if (singleResult.Password.Equals(AesOperation.EncryptString(Secrets.SymmetricKeyOAuth, userInformation.Password)))
                {
                    return singleResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        [HttpPost("LoginDietician")]
        public DieticianModel LoginDietician(IBaseUser userInformation)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<DieticianModel>("DieticianColletion");
            var results = collection.Find(x => x.Email == userInformation.Email).Limit(1).ToList();
            var singleResult = results.FirstOrDefault();
            if (singleResult != null)
            {
                if (singleResult.Password.Equals(AesOperation.EncryptString(Secrets.SymmetricKeyOAuth, userInformation.Password)))
                {
                    return singleResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

       
        
    }
}
