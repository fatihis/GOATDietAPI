using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOATDietAPI.Models.AppointmentModel
{
    public class AppointmentModel
    {
        [BsonId]
        public ObjectId _Id;
        public int? AppointmentId { get; set; }
        public int? PatientUid { get; set; }
        public int? DieticianUid { get; set; }
        public DateTime? Date { get; set; }
        public string Type  { get; set; }
        public string Status { get; set; }
        public string DeclineMessage { get; set; }
        public Payment? PaymentInformation { get; set; }
        
    }
    
    
    public class Payment
    {
        public double Amount { get; set; }
        public Boolean Status { get; set; }
        public DateTime TransactionTime{ get; set; }

    }

}
