using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pixogram.BusinessLayer.Repository;
using Pixogram.Entities;

namespace Pixogram_DotNet_MongoDb.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("api/user")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var products = await _userRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var product = await _userRepository.Get(id);
            return Ok(product);
        }

        [HttpPost]
        [Route("api/user/addValues")]
        public IActionResult Post(User model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                    return BadRequest("Please enter UserName");
                else if (string.IsNullOrWhiteSpace(model.Password))
                    return BadRequest("Please enter Password");
                else if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                    return BadRequest("Please enter ConfirmPassword");
                else if (string.IsNullOrWhiteSpace(model.Email))
                    return BadRequest("Please enter Email");

                _userRepository.Create(model);

                return Ok("Your product has been added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


    }
}