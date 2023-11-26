using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using mct.models;
namespace Company.Function
{
    public static class GetRegistration
    {
        [FunctionName("GetRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            List<Registration> list = new List<Registration>();
            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"))){
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM PersonInformation";
                    var registrations = await command.ExecuteReaderAsync();
                    while(await registrations.ReadAsync()){
                        list.Add(new Registration(){
                            RegistrationId = Guid.Parse(registrations["RegistrationId"].ToString()),
                            LastName = registrations["LastName"].ToString(),
                            FirstName = registrations["FirstName"].ToString(),
                            Email = registrations["Email"].ToString(),
                            Zipcode = int.Parse(registrations["Zipcode"].ToString()),
                            Age = int.Parse(registrations["Age"].ToString()),
                            IsFirstTimer = bool.Parse(registrations["IsFirstTimer"].ToString())
                        });
                        
                    }
                    
                    // var json = JsonConvert.SerializeObject(registrations);
                    return new OkObjectResult(list);
                }
            }

            
            
            
        }
    }
}
