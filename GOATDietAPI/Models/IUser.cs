using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOATDietAPI.Models
{
    public class IUser : IBaseUser
    {
        [BsonId]
        public ObjectId _Id;

        public int Uid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string UserType { get; set; }
    }
}
