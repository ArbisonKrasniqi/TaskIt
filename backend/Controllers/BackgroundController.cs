
using AutoMapper;
using backend.DTOs.Background.Input;
using backend.DTOs.Background.Output;
using backend.DTOs.User.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("backend/background")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly IBackgroundRepository _backgroundRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        
        public BackgroundController(IBackgroundRepository backgroundRepo, IUserRepository userRepo, IMapper mapper)
        {
            _backgroundRepo = backgroundRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        //GET ALL
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet(template: "GetAllBackgrounds")]
        public async Task<IActionResult> GetAllBackgrounds()
        {
            try
            {
                var backgrounds = await _backgroundRepo.GetAllBackgroundsAsync();
                

                if (backgrounds.Count == 0)
                    return Ok(new List<BackgroundDto>()); //Kthe list te zbrazet

                var backgroundDto = _mapper.Map<List<BackgroundDto>>(backgrounds);

                return Ok(backgroundDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }

        //GET BY ID
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetBackgroundByID")]
        public async Task<IActionResult> GetBackgroundById(int id)
        {
            try
            {
                var background = await _backgroundRepo.GetBackgroundByIdAsync(id);

                if (background == null)
                    return NotFound("Background Not Found");

                var backgroundDto = _mapper.Map<BackgroundDto>(background);
                return Ok(backgroundDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }

        //GET BY CreatorID
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetBackgroundsByCreatorId")]
        public async Task<IActionResult> GetBackgroundsByCreatorId(string craetorId)
        {
            try
            {
                if (!await _userRepo.UserExists(craetorId))
                {
                    return BadRequest("User Not Found");
                }
                
                var backgrounds = await _backgroundRepo.GetBackgroundsByCreatorIdAsync(craetorId);

                if (backgrounds.Count == 0)
                    return NotFound("Backgrounds Not Found!");

                var backgroundDto = _mapper.Map<IEnumerable<BackgroundDto>>(backgrounds);
                return Ok(backgroundDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }
        
        //GET ACTIVE BACKGROUNDS
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetActiveBackgrounds")]
        public async Task<IActionResult> GetActiveBackgrounds()
        {
            try
            {
                var backgrounds = await _backgroundRepo.GetActiveBackgroundsAsync();

                if (backgrounds.Count == 0)
                    return NotFound("No Active Backgrounds!");

                var backgroundDto = _mapper.Map<IEnumerable<BackgroundDto>>(backgrounds);
                return Ok(backgroundDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }


        //CREATE
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("CreateBackground")]
        public async Task<IActionResult> CreateBackground(CreateBackgroundDto backgroundDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (backgroundDto.ImageFile == null || backgroundDto.ImageFile.Length == 0)
            {
                return BadRequest("No file uploaded!");
            }

            try
            {
                
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await backgroundDto.ImageFile.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }
                
                
                var backgroundModel = new Background
                {
                    CreatorId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value,
                    Title = backgroundDto.Title,
                    ImageData = imageData
                };

                await _backgroundRepo.CreateBackgroundAsync(backgroundModel);
                return CreatedAtAction(nameof(GetBackgroundById), new {id = backgroundModel.BackgroundId}, _mapper.Map<BackgroundDto>(backgroundModel));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }

        //UPDATE
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("UpdateBackground")]
        public async Task<IActionResult> UpdateBackgroundById(UpdateBackgroundDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (updateDto.ImageFile.Length == 0)
            {
                return BadRequest("No file uploaded!");
            }
            
            try
            {
                var background = await _backgroundRepo.UpdateBackgroundByIdAsync(updateDto);

                if (background == null)
                    return NotFound("Background Not Found");

                var backgroundDto = _mapper.Map<BackgroundDto>(background);
                return Ok(backgroundDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        //DELETE BY ID
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("DeleteBackgroundByID")]
        public async Task<IActionResult> DeleteBoardById([FromQuery] BackgroundIdDto backgroundIdDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var backgroundModel = await _backgroundRepo.DeleteBackgroundByIdAsync(backgroundIdDto.BackgroundId);

                if (backgroundModel == null)
                    return NotFound("Background Does Not Exist!");

                return Ok("Background Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }
        }

        //DELETE BY CreatorID
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("DeleteBackgroundsByCreatorId")]
        public async Task<IActionResult> DeleteBackgroundsByCreatorId(UserIdDTO userIdDto)
        {
            if (!await _userRepo.UserExists(userIdDto.id))
                return StatusCode(404, "User Not Found!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var backgroundModel = await _backgroundRepo.DeleteBackgroundsByCreatorId(userIdDto.id);

                if (backgroundModel.Count == 0)
                    return NotFound("Backgrounds Not Founded!");

                return Ok("Backgrounds Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }

        }
    }
}