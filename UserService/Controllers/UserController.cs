﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Database;
using UserService.DTO;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServiceDatabaseContext _context;

        public UserController(UserServiceDatabaseContext context)
        {
            _context = context;
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
                Password = registerModel.Password,
                Email = registerModel.Email,
                VerifyEmailToken = verifyToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}