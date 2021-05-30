using GOATDietAPI.Models.AppointmentModel;
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
    public class AppointmentController : Controller
    {
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
        public IEnumerable<AppointmentModel> GetSinglePatientAppointment(int uid)
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
            singleResult.Status = "deleted by dietician";
            appointmentTrashbinCollection.InsertOneAsync(singleResult);

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
            singleResult.Status = "deleted by patient";
            appointmentTrashbinCollection.InsertOneAsync(singleResult);

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
    }
}
