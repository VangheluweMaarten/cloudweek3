using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using mct.models;
using Microsoft.Data.SqlClient;
namespace Company.Function
{
    public static class AddRegistration
    {
        [FunctionName("AddRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            
            var registration = JsonConvert.DeserializeObject<Registration>(requestBody);
            registration.RegistrationId = Guid.NewGuid();

            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"))){
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO PersonInformation (RegistrationId, LastName, FirstName, Email, Zipcode, Age, IsFirstTimer) VALUES (@RegistrationId, @LastName, @FirstName, @Email, @Zipcode, @Age, @IsFirstTimer)";
                    command.Parameters.AddWithValue("@RegistrationId", registration.RegistrationId);
                    command.Parameters.AddWithValue("@LastName", registration.LastName);
                    command.Parameters.AddWithValue("@FirstName", registration.FirstName);
                    command.Parameters.AddWithValue("@Email", registration.Email);
                    command.Parameters.AddWithValue("@Zipcode", registration.Zipcode);
                    command.Parameters.AddWithValue("@Age", registration.Age);
                    command.Parameters.AddWithValue("@IsFirstTimer", registration.IsFirstTimer);
                    await command.ExecuteNonQueryAsync();
                }
            }
           
            return new OkObjectResult(registration);
        }
    }
}
