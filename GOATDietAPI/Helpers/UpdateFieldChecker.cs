using System.Collections.Generic;
using GOATDietAPI.Models.AppointmentModel;

namespace GOATDietAPI.Helpers
{
    public class UpdateFieldChecker
    {
        public static List<string> checkAppointmentModelFields(AppointmentModel appointmentModel)
        {
            List<string> fieldValueList = new();
            // List<string> fieldValueList = new List<string>(new string[] { "PatientUid", "DieticianUid", "Date","Type","Status","DeclineMessage","PaymentInformation" });
            // foreach (string field in fieldValueList)
            // {
            //     if (appointmentModel[field] != "")
            //     {
            //         
            //     }
            // }
            if (appointmentModel.Date == null)
            {
                fieldValueList.Add("Date");
            }

            if (appointmentModel.Status == null)
            {
                fieldValueList.Add("Status");
            }

            if (appointmentModel.Type == null)
            {
                fieldValueList.Add("Type");
            }

            if (appointmentModel.DeclineMessage == null || appointmentModel.DeclineMessage == "")
            {
                fieldValueList.Add("DeclineMessage");
            }
            if (appointmentModel.PaymentInformation == null)
            {
                fieldValueList.Add("PaymentInformation");
            }if (appointmentModel.AppointmentId == null)
            {
                fieldValueList.Add("AppointmentId");
                
            }if (appointmentModel.DieticianUid == null)
            {
                fieldValueList.Add("DieticianUid");
            }

            return fieldValueList;
        }
    }
}