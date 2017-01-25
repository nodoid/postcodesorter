using Newtonsoft.Json;
using postcodesorter.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace postcodesorter.webservices
{
    public class Webservice
    {
        public static async Task<string> GetPostcode(double lon, double lat)
        {
            var url = string.Format("https://api.postcodes.io/postcodes/lon/{0}/lat/{1}", lon, lat);
            var postcode = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var obj = JsonConvert.DeserializeObject<Geolocation>(response.Content.ReadAsStringAsync().Result);
                    postcode = obj.result[0].postcode;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return postcode;
        }

        static double ToKilometers(string val)
        {
            return Convert.ToDouble(val.Split(' ')[0])/1.6;
        }

        public static async Task<List<SortedListData>> SortPostcodeByDistance(string myPostcode, List<string> postcodes)
        {
            var unsortedData = new List<SortedListData>();

            foreach(var p in postcodes)
            {
                var url = string.Format("http://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&mode=driving&language=en-EN&sensor=false", myPostcode.ToLowerInvariant(), p.ToLowerInvariant());
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(url);
                        var obj = JsonConvert.DeserializeObject<Postcode>(response.Content.ReadAsStringAsync().Result);
                        unsortedData.Add(new SortedListData
                        {
                            PostCode = p,
                            Distance = ToKilometers(obj.rows[0].elements[0].distance.text)
                        });
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine("Exception in GetData {0}-{1}", ex.Message, ex.InnerException);
#endif
                }
            }

            var sortedData = unsortedData.OrderBy(t => t.Distance).ToList();
            return sortedData;
        }
    }
}
