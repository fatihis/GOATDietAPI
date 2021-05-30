using System;
using System.Net;
using System.Text;

namespace GOATDietAPI.Helpers
{
    public class SmsApiHelper
    {
        public static string getAuthToken()
        {
            string document = "<sms><kno>41576</kno><kulad>905384431024</kulad><sifre>24b31H10</sifre><tur>Normal</tur><gonderen>SMS TEST</gonderen><mesaj>Bu benim ilk deneme mesajim</mesaj><numaralar>5070275935,5384431024,5538702670</numaralar></sms>";
            // var kno = "41576";
            // var kad = "905384431024";
            // var ksifre = "24b31H10";
            // var orjinator = "SMS TEST";
            return XmlPost("http://panel.vatansms.com/panel/smsgonder1Npost.php", document);
        }
        private static string XmlPost(string PostAddress, string xmlData)
        {
            using (WebClient wUpload = new WebClient())
            {
                wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
        }
    }
}