using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace coding_project_v2
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {            
            ProcessJsonFile();           
        }

        private static async void ProcessJsonFile()
        {        
                try
            {

                //using (StreamReader reader = new StreamReader(@"basic_endpoints.json"))
                using (StreamReader reader = new StreamReader(@"bonus_endpoints.json"))
                {
                    string json = reader.ReadToEnd();                   
                    var jsonServices = JsonConvert.DeserializeObject<dynamic>(json); 

                    //Console.WriteLine(jsonServices.services[1]);

                    /*** Loop through each enabled service and get it's endpoints ***/
                    foreach(var service in jsonServices.services)
                    {
                        if ((bool)service.enabled)
                        {                            
                            /*** Loop through each service's enabled endpoint and create a resource url (baseURL + resource) ***/
                            foreach (var endpoint in service.endpoints)
                            {
                                if ((bool)endpoint.enabled)
                                {
                                    string resourceUrl = service.baseURL + endpoint.resource;

                                    /*** Make API call using resourceUrl ***/                                    
                                    var task = Task.Run(() => getJsonResourceAsync(resourceUrl));
                                    task.Wait();

                                    /*** Deserialize api response ***/                                   
                                    JObject apiResponse = JObject.Parse(task.Result);
                                   
                                    /*** 
                                     * For each response, check if the element & identifier, match with what was returned from the
                                     * api
                                     ***/
                                    foreach(var response in endpoint.response)
                                    {
                                        string param = response.element;
                                        string valueFromApi = (string)apiResponse.GetValue(param);

                                        Console.WriteLine(" --> [Local] Json file value");
                                        if (response.regex != null)
                                        {                                           
                                            string localRegexVal = response.regex;
                                            Console.WriteLine(localRegexVal);
                                            Console.WriteLine("-->  API Response");
                                            Console.WriteLine(response.element + " -> " + valueFromApi);
                                            checkRegex(localRegexVal, valueFromApi);
                                            compareTwoStrings(localRegexVal, valueFromApi);
                                        }

                                        if (response.identifier != null)
                                        {
                                            
                                            string localIdentifierVal = response.identifier;
                                            Console.WriteLine(localIdentifierVal);
                                            Console.WriteLine("-->  API Response");
                                            Console.WriteLine(response.element + " -> " + valueFromApi);
                                            compareTwoStrings(localIdentifierVal, valueFromApi);
                                        }                                        
                                    }                                  
                                }
                            }                                                   
                        }                        
                    }
                }
             
            } catch (Exception ex)
            {
                Console.WriteLine("Could not deserialize json file :" + ex.Message.ToString());               
            }           
        }        

        /***
         * Makes a Get request to a resource api endpoint and converts the returned json string into a usable
         * c# object
         ***/
        private static async Task<string> getJsonResourceAsync(string resourceUrl)
        {
            Console.WriteLine("Connecting to resource endpoint -> (" + resourceUrl + ")");
            Console.WriteLine();
            var response = String.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(resourceUrl);
                if (message.IsSuccessStatusCode) response = await message.Content.ReadAsStringAsync();
            }                          
            return response;
        }     

        private static void compareTwoStrings(string local, string fromApi)
        {
            if (local.Equals(fromApi))
            {
                Console.WriteLine("[-----------  Api Response is as expected ------------]");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("[-----------  Api Response is NOT what was expected ------------]");
                Console.WriteLine();
            }
        }

        private static void checkRegex(string localRegexPattern, string apiResponseValue) 
        {
            bool result = Regex.IsMatch(apiResponseValue, localRegexPattern);
            if (result)
            {
                Console.WriteLine("Regex pattern and Api Response value match.");
            } else
            {
                Console.WriteLine("Regex pattern and Api Response value do not match.");
            }
        }
    }
}
