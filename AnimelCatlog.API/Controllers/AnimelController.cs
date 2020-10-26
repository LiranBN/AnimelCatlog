using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimelCatlog.Domain.Data;
using AnimelCatlog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimelCatlog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnimelController : ControllerBase
    {
        private readonly IAnimelRepository animelRepo;

        public AnimelController(IAnimelRepository animelRepo)
        {
            this.animelRepo = animelRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimelsAsync()
        {
            var animels = await animelRepo.GetAnimelsAsync();
            return Ok(animels);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAnimelCatlogAsync([FromBody] Animel animel)
        {
            var errors = await animelRepo.ValidateAsync(animel);
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedAnimel = await animelRepo.UpdateAnimelAsync(animel);
            if (updatedAnimel == null)
            {
                return StatusCode(500);
            }
            return Ok(updatedAnimel);
        }
    }
}
