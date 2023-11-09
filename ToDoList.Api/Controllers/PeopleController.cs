using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.People;
using ToDoList.Api.Repositories;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/people/[action]")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPeopleRepository _peopleReposit;
        private readonly IMapper _mapper;

        public PeopleController(ILogger<PeopleController> logger, IMapper mapper, IPeopleRepository peopleReposit)
        {
            _logger = logger;
            _peopleReposit = peopleReposit;
            _mapper = mapper;
        }

        #region Get

        [HttpGet]
        [Route("{peopleId}", Name = "GetPeopleInfos")]
        public async Task<IActionResult> GetPeopleInfos(int peopleId, bool includeProjects = false, bool includeTasks = false)
        {
            //Check People Exists
            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
            {
                return NotFound();
            }

            var peopleData = await _peopleReposit.GetPeopleByIdAsync(peopleId, includeProjects, includeTasks);

            //Get People With tasks list and project list
            //if (includeTasks && includeProjects)
            //    return Ok(_mapper.Map<PeopleDto>(peopleData));

            //Get People With Project List
            if (includeProjects)
                return Ok(_mapper.Map<PeopleWithIncludeProjectDto>(peopleData));

            //Get People With Tasks list
            if (includeTasks)
                return Ok(_mapper.Map<PeopleWithIncludeTaskDto>(peopleData));

            //get People With Persoanl Property
            //if (!includeProjects && !includeTasks)
            return Ok(_mapper.Map<PeopleForDisplayDto>(peopleData));
        }

        #endregion

        #region Post

        [HttpPost]
        public async Task<ActionResult> AddPeople(PeopleForCreationDto peopleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var people = _mapper.Map<People>(peopleDto);
            await _peopleReposit.AddPeopleAsync(people);

            var peopleForDisplay = _mapper.Map<PeopleForDisplayDto>(people);

            if (await _peopleReposit.SaveChangesAsync())
            {
                return CreatedAtRoute
                   (
                   "GetPeopleInfos",
                   new { peopleId = people.Id },
                   peopleForDisplay
                   );
            }
            _logger.LogInformation("An error occurred while saving");
            return BadRequest("An error occurred while saving");

        }

        #endregion

        #region Patch / Put

        [HttpPut("{peopleId}", Name = "UpdatePeople")]
        //[ActionName("UpdatePeople")]
        public async Task<ActionResult> UpdatePeople(int peopleId, PeopleForCreationDto people)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound();

            var peopleMapped = _mapper.Map<People>(people);
            peopleMapped.ModificationDate = DateTime.Now;

            await _peopleReposit.UpdatePeopleAsync(peopleId, peopleMapped);
            if (await _peopleReposit.SaveChangesAsync())
            {
                var reMapp = _mapper.Map<PeopleForDisplayDto>(peopleMapped);
                return CreatedAtRoute
                    (
                    "GetPeopleInfos",
                    new { peopleId = peopleId },
                    reMapp
                    );
            }
            _logger.LogInformation("An error occurred while Updating");
            return BadRequest();
        }

        [HttpPatch("{peopleId}")]
        public async Task<ActionResult> PartiallyUpdatePeople
            (
            int peopleId,
            JsonPatchDocument<PeopleForUpdateDto> patchDocument
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var people = await _peopleReposit.GetPeopleByIdAsync(peopleId);
            if (people is null)
                return NotFound();

            people.ModificationDate = DateTime.Now;
            var peopleToPatch = _mapper.Map<PeopleForUpdateDto>(people);
            patchDocument.ApplyTo(peopleToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(peopleToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(peopleToPatch, people);
            await _peopleReposit.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Hard And Soft Delete

        [HttpDelete("{peopleId}")]
        public async Task<ActionResult> DeletePeople(int peopleId)
        {
            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound();

            if (await _peopleReposit.DeletePeopleAsync(peopleId))
            {
                await _peopleReposit.SaveChangesAsync();
                return NoContent();
            }
            _logger.LogInformation("An error occurred while deleting a person");
            return BadRequest();
        }

        [HttpPatch("{peopleId}")]
        public async Task<ActionResult> SoftDeletePeople
            (
            int peopleId,
            JsonPatchDocument<PeopleForDeleteDto> patchDocument
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var people = await _peopleReposit.GetPeopleByIdAsync(peopleId);
            if (people is null)
                return NotFound();

            var peopleToPatch = _mapper.Map<PeopleForDeleteDto>(people);
            patchDocument.ApplyTo(peopleToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(peopleToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(peopleToPatch, people);
            await _peopleReposit.SaveChangesAsync();

            return NoContent();
        }

        #endregion
    }
}
