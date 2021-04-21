using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Messaging;
using System;
using System.Text;
using System.Threading.Tasks;
using UserService.Database;
using UserService.DTO;
using BC = BCrypt.Net.BCrypt;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServiceDatabaseContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public UserController(UserServiceDatabaseContext context, ILogger<UserController> logger, IMessagePublisher messagePublisher)
        {
            _context = context;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            //_logger.LogInformation("Entering 'test' method");
            //_logger.LogInformation("Building factory");
            //var factory = new ConnectionFactory();
            //factory.UserName = "guest";
            //factory.Password = "guest";
            //var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
            //          new AmqpTcpEndpoint("rabbitmq"),
            //          new AmqpTcpEndpoint("localhost")
            //    };
            //factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            //_logger.LogInformation("Opening connection...");
            //using var connection = factory.CreateConnection(endpoints);
            //_logger.LogInformation("Opening channel...");
            //using var channel = connection.CreateModel();
            //channel.QueueDeclare("user-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            //var message = new { Id = 1, Username = "Siebren" };
            //var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            //_logger.LogInformation("Publishing message...");
            //channel.BasicPublish("", "user-queue", null, body);
            //_logger.LogInformation("Returning OK");

            await _messagePublisher.PublishMessageAsync("", new { Id = 1, Username = "Siebren XD" });
            return Ok(new { });
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest("REGISTER.NO_DATA");
            }

            if (string.IsNullOrWhiteSpace(registerModel.Username))
            {
                return BadRequest("REGISTER.NO_USERNAME");
            }

            if (string.IsNullOrWhiteSpace(registerModel.Password))
            {
                return BadRequest("REGISTER.NO_PASSWORD");
            }

            if (string.IsNullOrWhiteSpace(registerModel.Email))
            {
                return BadRequest("REGISTER.INCORRECT_EMAIL");
            }

            if (string.IsNullOrWhiteSpace(registerModel.Name))
            {
                return BadRequest("REGISTER.NO_NAME");
            }

            if(await _context.Users.AnyAsync(x => x.Username == registerModel.Username))
            {
                return BadRequest("REGISTER.USERNAME_TAKEN");
            }

            if (await _context.Users.AnyAsync(x => x.Email == registerModel.Email))
            {
                return BadRequest("REGISTER.EMAIL_TAKEN");
            }

            Guid? verifyToken = Guid.Empty;
            do
            {
                verifyToken = Guid.NewGuid();
            } while (await _context.Users.AnyAsync(x => x.VerifyEmailToken == verifyToken));

            var user = new User()
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                Password = BC.HashPassword(registerModel.Password, BC.GenerateSalt(12)),
                Email = registerModel.Email,
                VerifyEmailToken = verifyToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
