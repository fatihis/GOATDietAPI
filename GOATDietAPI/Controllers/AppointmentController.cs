using GOATDietAPI.Models.AppointmentModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GOATDietAPI.Helpers;

namespace GOATDietAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        [HttpGet("{appointmentId}")]
        public AppointmentModel GetSingleAppointment(int appointmentId)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var document = collection.Find(appointment =>appointment.AppointmentId == appointmentId).Limit(1).ToList().FirstOrDefault();
            return document;
        }
        [HttpGet("patient/{uid}")]
        public IEnumerable<AppointmentModel> GetAllAppointmentsPatient(int uid)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var document = collection.Find(appointment =>appointment.PatientUid == uid).ToList();
            return document;
        } 
        [HttpGet("dietician/{uid}")]
        public IEnumerable<AppointmentModel> GetAllDieticianAppointment(int uid)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var document = collection.Find(appointment => appointment.DieticianUid == uid).ToList();
            return document;
        }
        [HttpPost("CreateAppointment")]
        public AppointmentModel AddNewAppointment(AppointmentModel newAppointment)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            // if (newAppointment.PatientUid != null){
            //    var authCheck = AuthenticationController.GetAuthStatusPatient(newAppointment.PatientUid.Value);
            // }
            AppointmentModel lastIdDocument = collection.Find(patient => true).SortByDescending(e => e.AppointmentId).ToList().FirstOrDefault();
            var newAppointmentAppointmentId = lastIdDocument == null ? 0 : lastIdDocument.AppointmentId;
            newAppointment.AppointmentId = newAppointmentAppointmentId;
            collection.InsertOneAsync(newAppointment);
            return newAppointment;
        }
        [HttpDelete("Dietician/{appointmentId}")]
        public Boolean DeleteAppointmentByDietician(int appointmentId)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var document = collection.Find(appointment => appointment.AppointmentId == appointmentId).Limit(1).ToList();

            //add to old appointments
            var singleResult = document.FirstOrDefault();
            var appointmentTrashbinCollection = database.GetCollection<AppointmentModel>("OldAppointmentCollection");
            if (singleResult != null)
            {
                singleResult.Status = "deleted by dietician";
                appointmentTrashbinCollection.InsertOneAsync(singleResult);
            }
            else
            {
                return false;
            }

            //delete
            var filter = Builders<AppointmentModel>.Filter.Eq("AppointmentId ", appointmentId);
            try
            {
                collection.DeleteOne(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
        [HttpDelete("Patient/{appointmentId}")]
        public Boolean DeleteAppointmentByPatient(int appointmentId)
        {
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var document = collection.Find(appointment => appointment.AppointmentId == appointmentId).Limit(1).ToList();

            //add to old appointments
            var singleResult = document.FirstOrDefault();
            var appointmentTrashbinCollection = database.GetCollection<AppointmentModel>("OldAppointmentCollection");
            if (singleResult != null)
            {
                singleResult.Status = "deleted by patient";
                appointmentTrashbinCollection.InsertOneAsync(singleResult);
            }
            else
            {
                return false;
            }

            //delete
            var filter = Builders<AppointmentModel>.Filter.Eq("AppointmentId ", appointmentId);
            try
            {
                collection.DeleteOne(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        [HttpPut("Dietician/CreateUpdateRequest/{appointmentId}")]
        public Boolean UpdateAppointmentRequestByDietician(int appointmentId)
        {
            return true;
        }
        [HttpPut("UpdateAppointment/{appointmentId}")]
        public List<string> UpdateAppointment(int appointmentId, AppointmentModel updatedAppointmentModel)
        {
            // var queryUpdateSelection = from appData in updatedAppointmentModel select new AppointmentModel().appointmentId;
            List<string> fieldsToUpdate = UpdateFieldChecker.checkAppointmentModelFields(updatedAppointmentModel);
            var client = new MongoClient(Secrets.DatabaseKey);
            var database = client.GetDatabase("DietDB");
            var collection = database.GetCollection<AppointmentModel>("AppointmentCollection");
            var filter = Builders<AppointmentModel>.Filter.Eq("AppointmentId", updatedAppointmentModel.AppointmentId);
            foreach (var field in fieldsToUpdate)
            {
                switch (field)
                {
                    case "AppointmentId": 
                        var updateAppointmentId = Builders<AppointmentModel>.Update.Set("AppointmentId", updatedAppointmentModel.AppointmentId);
                        collection.UpdateOne(filter, updateAppointmentId);
                        break;
                    case "PatientUid":
                        var updatePatientUid = Builders<AppointmentModel>.Update.Set("PatientUid", updatedAppointmentModel.PatientUid);
                        collection.UpdateOne(filter, updatePatientUid);
                        break;
                    case "DieticianUid":
                        var updateDieticianUid = Builders<AppointmentModel>.Update.Set("DieticianUid", updatedAppointmentModel.DieticianUid);
                        collection.UpdateOne(filter, updateDieticianUid);
                        break;
                    case "Date":
                        var updateDate = Builders<AppointmentModel>.Update.Set("Date", updatedAppointmentModel.Date);
                        collection.UpdateOne(filter, updateDate);
                        break;
                    case "Type":
                        var updateType = Builders<AppointmentModel>.Update.Set("Type", updatedAppointmentModel.Type);
                        collection.UpdateOne(filter, updateType);
                        break;
                    case "Status":
                        var updateStatus = Builders<AppointmentModel>.Update.Set("Status", updatedAppointmentModel.Status);
                        collection.UpdateOne(filter, updateStatus);
                        break;
                    case "DeclineMessage":
                        var updateDeclineMessage = Builders<AppointmentModel>.Update.Set("DeclineMessage", updatedAppointmentModel.DeclineMessage);
                        collection.UpdateOne(filter, updateDeclineMessage);
                        break;
                    case "PaymentInformation":
                        var updatePaymentInformation = Builders<AppointmentModel>.Update.Set("PaymentInformation", updatedAppointmentModel.PaymentInformation);
                        collection.UpdateOne(filter, updatePaymentInformation);
                        break;
                }
                
            }
            return fieldsToUpdate;
        }
    }
}
