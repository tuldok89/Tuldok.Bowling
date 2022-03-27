using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tuldok.Bowling.Api.Dto;
using Tuldok.Bowling.Api.Utils;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Service.Exceptions;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IBowlingService _bowlingService;
        private readonly IScoreService _scoreService;

        public GamesController(IBowlingService bowlingService, IScoreService scoreService)
        {
            _bowlingService = bowlingService;
            _scoreService = scoreService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GameDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var games = (await _bowlingService.GetAllGames()).Select(x => new GameDto
                {
                    Id = x.Id,
                    Name = x.Name
                });
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var game = await _bowlingService.GetGame(id);

                return Ok(new GameDto
                {
                    Name = game.Name,
                    Id = game.Id
                });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGame([FromBody] CreateUpdateGameDto gameDto)
        {
            try
            {
                var createdGame = await _bowlingService.CreateGame(gameDto.Name);
                return CreatedAtAction(nameof(Get), new { id = createdGame.Id }, new GameDto { Id = createdGame.Id, Name = createdGame.Name});
            }
            catch (EntityNotCreatedException ex)
            {
                return StatusCode(500, new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGame(Guid id, [FromBody] CreateUpdateGameDto gameDto)
        {
            try
            {
                await _bowlingService.UpdateGame(new Game
                {
                    Name = gameDto.Name,
                    Id = id
                });

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = new { Message = ex.Message}});
            }
            catch (EntityNotUpdatedException ex)
            {
                return StatusCode(500, new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            try
            {
                await _bowlingService.DeleteGame(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpGet("{id}/Score")]
        public async Task<IActionResult> Score(Guid id)
        {
            try
            {
                var score = await _scoreService.GameScore(id);

                return Ok(new { Score = score });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }
    }
}
