using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace ExploreCalifornia.WebApp.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpPost]
        [Route("Book")]
        public IActionResult Book()
        {
            var tourname = Request.Form["tourname"];
            var name = Request.Form["name"];
            var email= Request.Form["email"];
            var needsTransport = Request.Form["transport"] == "on";

            // Send messages here...
            var factory =new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //First parameter is an exchange name, then exhange type,durable exchange
            //a Durable exhange survives a broker restart
            channel.ExchangeDeclare("myExchange", ExchangeType.Fanout, true);
            var message = "";
            var bytes = System.Text.Encoding.UTF8.GetBytes(message); 
            
           channel.BasicPublish("myExhange","",null,bytes);
            channel.Close();
            connection.Close();

            return Redirect($"/BookingConfirmed?tourname={tourname}&name={name}&email={email}");
        }

        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel()
        {
            var tourname = Request.Form["tourname"];
            var name = Request.Form["name"];
            var email = Request.Form["email"];
            var cancelReason = Request.Form["reason"];

            // Send cancel message here

            return Redirect($"/BookingCanceled?tourname={tourname}&name={name}");
        }
    }
}