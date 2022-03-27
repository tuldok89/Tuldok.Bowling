using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tuldok.Bowling.Api.Dto;
using Tuldok.Bowling.Api.Utils;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Service.Exceptions;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Api.Controllers
{
    [Route("api/Games/{gameId}/[controller]")]
    [ApiController]
    public class ShotsController : ControllerBase
    {
        private readonly IBowlingService _bowlingService;

        public ShotsController(IBowlingService bowlingService)
        {
            _bowlingService = bowlingService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid gameId)
        {
            try
            {
                var frames = await _bowlingService.GetAllFrames(gameId);
                var framesList = new List<FrameDto>();

                foreach (var frame in frames)
                {
                    var shots = (await _bowlingService.GetAllShots(frame.Id)).OrderBy(x => x.SequenceNumber);
                    framesList.Add(new FrameDto
                    {
                        Id = frame.Id,
                        Shot1 = shots.ElementAtOrDefault(0)?.FallenPins,
                        Shot2 = shots.ElementAtOrDefault(1)?.FallenPins,
                        Shot3 = shots.ElementAtOrDefault(2)?.FallenPins,
                        FrameNumber = frame.SequenceNumber
                    });
                }

                return Ok(framesList);
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

        [HttpGet("{frameId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid gameId, Guid frameId)
        {
            try
            {
                var frame = await _bowlingService.GetFrame(frameId);

                var shots = (await _bowlingService.GetAllShots(frame.Id)).OrderBy(x => x.SequenceNumber);

                return Ok(new FrameDto
                {
                    Id = frame.Id,
                    Shot1 = shots.ElementAtOrDefault(0)?.FallenPins,
                    Shot2 = shots.ElementAtOrDefault(1)?.FallenPins,
                    Shot3 = shots.ElementAtOrDefault(2)?.FallenPins,
                    FrameNumber = frame.SequenceNumber
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

        [HttpDelete("{frameId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid gameId, Guid frameId)
        {
            try
            {
                await _bowlingService.DeleteFrame(frameId);

                return NoContent();
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFrame(Guid gameId, [FromBody] CreateUpdateFrameDto createUpdateFrameDto)
        {
            try
            {
                if (createUpdateFrameDto.Shot1 == null && createUpdateFrameDto.Shot2 == null && createUpdateFrameDto.Shot3 == null)
                {
                    throw new InvalidOperationException();
                }

                var frame = await _bowlingService.CreateFrame(gameId);

                if (createUpdateFrameDto.Shot1.HasValue)
                {
                    await _bowlingService.CreateShot(frame.Id, createUpdateFrameDto.Shot1.Value);
                }

                if (createUpdateFrameDto.Shot2.HasValue)
                {
                    await _bowlingService.CreateShot(frame.Id, createUpdateFrameDto.Shot2.Value);
                }

                if (createUpdateFrameDto.Shot3.HasValue)
                {
                    await _bowlingService.CreateShot(frame.Id, createUpdateFrameDto.Shot3.Value);
                }

                return Created($"~/api/Games/{gameId}/Shots/{frame.Id}", new FrameDto
                {
                    Id = frame.Id,
                    FrameNumber = frame.SequenceNumber,
                    Shot1 = createUpdateFrameDto.Shot1,
                    Shot2 = createUpdateFrameDto.Shot2,
                    Shot3 = createUpdateFrameDto.Shot3
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (DuplicateSequenceException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (EntityNotCreatedException ex)
            {
                return StatusCode(500, new { Message = ex.Message});
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message});
            }
            catch (EntityCountExceededException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (PinFallsExceededException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorMessages.UnknownError });
            }
        }

        [HttpPut("{frameId}")]
        [HttpPatch("{frameId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid gameId, Guid frameId, [FromBody] CreateUpdateFrameDto createUpdateFrameDto)
        {
            try
            {
                if (createUpdateFrameDto.Shot1 == null && createUpdateFrameDto.Shot2 == null && createUpdateFrameDto.Shot3 == null)
                {
                    throw new InvalidOperationException();
                }

                await _bowlingService.DeleteAllShots(frameId);

                if (createUpdateFrameDto.Shot1.HasValue)
                {
                    await _bowlingService.CreateShot(frameId, createUpdateFrameDto.Shot1.Value);
                }

                if (createUpdateFrameDto.Shot2.HasValue)
                {
                    await _bowlingService.CreateShot(frameId, createUpdateFrameDto.Shot2.Value);
                }

                if (createUpdateFrameDto.Shot3.HasValue)
                {
                    await _bowlingService.CreateShot(frameId, createUpdateFrameDto.Shot3.Value);
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (DuplicateSequenceException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (EntityNotCreatedException ex)
            {
                return StatusCode(500, new { Message = ex.Message});
            }
            catch (EntityCountExceededException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (PinFallsExceededException ex)
            {
                return BadRequest(new { Message = ex.Message});
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
    }
}
