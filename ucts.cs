using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace DataStream_UCTS_Upload
{
    class DataStream_UCTS
    {
        private string ServerUrl = "http://product.datastream.com";
        private string UCTSPageAddress = "/UCTS/UCTSMaint.asp";
        private string DataStreamUserId;
        private string DataStreamPassword;

        
        public DataStream_UCTS(string userId, string password)
        {
            DataStreamUserId = userId;
            DataStreamPassword = password;

        }



        /// <summary>
        /// Convert dictionary data values to string, separated by commas. 
        /// Datastream follows weekdays only for daily data, adjust the input data as this function does not skip weeeknds
        /// </summary>
        /// <param name="data"> Data dictionary</param>
        /// <returns>Concatenated string</returns>     
        /// 
        
        string ConvertDatatoString(Dictionary<string, string> data)

        {
            string StringData = "";
            var CleanData = data.ToDictionary(w => w.Key, w => double.TryParse(w.Value, out double b) ?   w.Value : "NAVALUE");
            StringData = string.Format("{0}", string.Join(",", CleanData.Values));
            return StringData;
        }




        /// <summary>
        /// Encode Password
        /// </summary>
        /// <param name="password">DataStream Password</param>
        /// <returns>Encoded password</returns>
      
        string EncodePassword(string password)
        {
            string EncodedPassword = "";
            int Seed = 199; // arbitrary number, encoding key
            foreach (char c in password)
            {
                int AscByte = System.Convert.ToInt32(c);
                int CryptedByte = AscByte ^ Seed;
                EncodedPassword += CryptedByte.ToString("D3");
                Seed = ((Seed + CryptedByte) % 255) ^ 67;
            }
            return EncodedPassword;
        }


        /// Create UCTS Url, specific to the UserID

        string GetUCTSUrl (string UserId)
        {
            return ServerUrl + UCTSPageAddress + "?UserId="+ UserId;
        }



        /// <summary>
        /// UCTS full upload, OVERWRIITE UCTS if exists without confirmation
        /// </summary>
        /// <param name="data">Data - Date (string) and Values(string) format</param>
        /// <param name="dsCode">8 Digit </param>
        /// <param name="managementGroup">Management Group</param>
        /// <param name="startDate">Start date, only accept d/m/yyyy format</param>
        /// <param name="endDate">End date, only accept d/m/yyyy format</param>
        /// <param name="frequency">frequency - Possible values D,W,M,Q,Y</param>
        /// <param name="title">Title</param>
        /// <param name="units">Unit</param>
        /// <param name="decPlaces"> Number of decimal places- as string </param>
        /// <param name="asPerc">As % - Possible values N,Y </param>
        /// <param name="freqConv">Frequency Conversion - Possible value ACT,SUM,AVG,END</param>
        /// <param name="alignment">Alignment - Possible values 1ST,MID,END</param>
        /// <param name="carryInd">Carry Indicator - Possible values YES,NO,PAD</param>
        /// <param name="primeCurr">Currency - Use DataStream to find currency code</param>
        /// <returns>*OK* if success, or error if any</returns>


        public async Task< string> UploadUCTS
            (


            Dictionary<string, string> data,
            string dsCode,
            string managementGroup,
            string startDate,
            string endDate,
            string frequency,
            string title,
            string units,
            string decPlaces,
            string asPerc,
            string freqConv,
            string alignment,
            string carryInd,
            string primeCurr          
            

            )
         {

            var parameters = new Dictionary<string, string> {
               
                    {"CallType", "Upload" },
                    {"TSMnemonic", dsCode.ToUpper()},
                    {"TSMLM", managementGroup},
                    {"TSStartDate",startDate},
                    {"TSEndDate",endDate},
                    {"TSFrequency",frequency},
                    {"TSTitle", title},
                    {"TSUnits",units},
                    {"TSDecPlaces",decPlaces},
                    {"TSAsPerc",asPerc},
                    {"TSFreqConv",freqConv},                 
                    {"TSAlignment",alignment},
                    {"TSCarryInd",carryInd},
                    {"TSPrimeCurr",primeCurr},
                    {"TSULCurr",""},
                    {"ForceUpdateFlag1","Y"},
                    {"ForceUpdateFlag2","Y"},
                    {"TSValsStart",startDate},
                    {"TSValues", ConvertDatatoString(data) },
                    {"UserOption", EncodePassword(DataStreamPassword) }
                };


            int Attempt=0;
            string ResultContent = "";

            using (var client = new HttpClient())
            {
                //Re-submit after 2 seconds if fails, max attempt 2

                while (Attempt < 3 && ResultContent != "*OK*")
                {
                    var Content = new FormUrlEncodedContent(parameters);
                    var result = await client.PostAsync(GetUCTSUrl(DataStreamUserId), Content);
                    ResultContent = await result.Content.ReadAsStringAsync();
                    if (ResultContent == "*OK*") break;
                    Attempt++;

                    System.Threading.Thread.Sleep(2000);
                }
            }
            
            return ResultContent;
        }


    }
}
