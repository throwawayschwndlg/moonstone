using moonstone.core.exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.utils
{
    public class MSWebRequest
    {
        protected const int REQUEST_TIMEOUT = 1000;

        public static dynamic RequestJson(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = REQUEST_TIMEOUT;

                var responseText = string.Empty;
                using (var response = request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    var streamReader = new StreamReader(stream);
                    responseText = streamReader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject(responseText);
            }
            catch (Exception e)
            {
                throw new RequestJsonException(
                    $"Failed to request json from {url}. Probably timed out.", e);
            }
        }
    }
}