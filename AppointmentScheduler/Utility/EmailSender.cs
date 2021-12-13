using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("643e0bdb6911792b9caa0ccb8a8610da"), 
                Environment.GetEnvironmentVariable("f2e72e839fba26ca9e67e328bcff6334"))
            {

            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                    {"From", new JObject {
                    {"Email", "nickguerra@gmail.com"},
                    {"Name", "Appointment Scheduler"}
                }},
                    {"To", new JArray {
                    new JObject{
                        {"Email", email},
                    }
                    }},
                {"Subject", subject},
                {"HTMLPart", htmlMessage }
                }
            });
            MailjetResponse response = await client.PostAsync(request);
         

        }
    }
}
