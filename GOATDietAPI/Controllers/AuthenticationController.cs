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
        public string AddNewPatient(PatientModel newPatient)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<PatientModel>("PatientCollection");
            var checkExistance =
                collection.Find(patient => patient.Email == newPatient.Email || patient.PhoneNumber == newPatient.PhoneNumber).ToList().FirstOrDefault();
            switch (checkExistance)
            {
                case null:
                {
                    PatientModel lastIdDocument = collection.Find(patient => true).SortByDescending(e => e.Uid).ToList().FirstOrDefault();
                    PatientModel newPatientModel = new PatientModel()
                    {
                        _Id = ObjectId.GenerateNewId(),
                        Uid = lastIdDocument.Uid == null ? 0 : lastIdDocument.Uid + 1 ,
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
                    break;
                }
                default:
                    return "Email or Phone Number Exist";
            }
            
            return "Done";

        }
        [HttpPost("CreateDietician")]
        public string AddNewDietician(DieticianModel newDietician)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<DieticianModel>("DieticianCollection");
            DieticianModel lastIdDocument = collection.Find(patient => true).SortByDescending(e => e.Uid).ToList().FirstOrDefault();
            var checkExistance =
                collection.Find(dietician => dietician.Email == newDietician.Email || dietician.PhoneNumber == newDietician.PhoneNumber).ToList().FirstOrDefault();
            switch (checkExistance)
            {
                case null:
                {
                    DieticianModel newDieticianModel = new DieticianModel()
                    {
                        _Id = ObjectId.GenerateNewId(),
                        Uid = lastIdDocument.Uid == null ? 0 : lastIdDocument.Uid + 1,
                        Name = newDietician.Name,
                        Surname = newDietician.Surname,
                        Email = newDietician.Email,
                        PhoneNumber = newDietician.PhoneNumber,
                        Password = AesOperation.EncryptString(Secrets.SymmetricKeyOAuth, newDietician.Password),
                        UserType = "Dietician",
                        Score = 0,
                        DetailInformation = newDietician.DetailInformation,
                        GraduatedSchool = newDietician.GraduatedSchool,
                        Experiences = newDietician.Experiences,
                        PhotoUrl = newDietician.PhotoUrl,
                        AvailableDays = newDietician.AvailableDays,
                        Comments = newDietician.Comments,
                    };
                    collection.InsertOneAsync(newDietician);
                    return "User created";
                }
                default:
                    return "Email or Phone Number Exist";
            }
            
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
