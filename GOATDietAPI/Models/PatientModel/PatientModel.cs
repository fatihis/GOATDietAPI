using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOATDietAPI.Models
{
    public class PatientModel : IUser
    {

        public string Sex { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public Boolean Pregnancy { get; set; }
        public double TightestNeck { get; set; }
        public double ThinnestWaist { get; set; }
        public Boolean PhoneAuthenticated { get; set; }
    }
}
