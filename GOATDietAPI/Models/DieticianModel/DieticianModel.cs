using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOATDietAPI.Models.DieticianModel
{
    public class DieticianModel : IUser
    {
        public double Score { get; set; }
        public string DetailInformation { get; set; }
        public School GraduatedSchool { get; set; }
        public List<Experience> Experiences { get; set; }
        public string PhotoUrl { get; set; }
        public List<AvailableDay> AvailableDays { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public string PatientUid { get; set; }
        public double Score { get; set; }
    }


    public class School
    {
        public string SchoolName { get; set; }
        public string GraduationYear { get; set; }
    }
    public class Experience
    {
        public string ExperiencedField { get; set; }
        public string Year { get; set; }
        public string Workplace { get; set; }
    }
    public class AvailableDay
    {
        public string DayName { get; set; }
        public string HourInterval { get; set; }
        public string OffHours { get; set; }

    }
}