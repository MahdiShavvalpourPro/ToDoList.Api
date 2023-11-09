using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Project;
using ToDoList.Api.Repositories;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/people/{peopleId}/projects/[action]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IMapper _mapper;
        private readonly IPeopleRepository _peopleReposit;
        private readonly IProjectRepository _projectReposit;

        public ProjectController(
            ILogger<ProjectController> logger,
            IMapper mapper,
            IPeopleRepository peopleReposit,
            IProjectRepository projectReposit)
        {
            _logger = logger;
            _mapper = mapper;
            _peopleReposit = peopleReposit;
            _projectReposit = projectReposit;
        }

        #region GetProject

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectsDto>>> GetProjects(int peopleId)
        {
            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
            {
                _logger.LogInformation($"An User with ID {peopleId} was not identified");
                return NotFound();
            }
            var project = await _projectReposit.GetProjectsAsync(peopleId);
            return Ok(
                _mapper.Map<IEnumerable<ProjectsDto>>(project)
                );
        }

        [HttpGet("{projectId}", Name = "GetProject")]
        public async Task<ActionResult<ProjectsDto>> GetProject(int peopleId, int projectId)
        {
            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound();

            var project = await _projectReposit.GetProjectAsync(peopleId, projectId);
            if (project == null)
                return NotFound();

            return Ok(_mapper.Map<ProjectsDto>(project));

        }


        #endregion

        #region Insert Project

        [HttpPost]
        public async Task<ActionResult> AddProject(int peopleId, [FromBody] ProjectForCreationDto project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound();

            var projectForBank = _mapper.Map<Projects>(project);

            var owner = await _peopleReposit.GetPeopleByIdAsync(peopleId);
            projectForBank.Owner = owner!;

            await _projectReposit.AddProjectAsync(peopleId, projectForBank);
            if (await _projectReposit.SaveChangesAsync())
            {
                return CreatedAtRoute(
                    "GetProject",
                    new { peopleId = peopleId, projectId = projectForBank.Id },
                    _mapper.Map<ProjectsDto>(projectForBank)
                    );
            }
            _logger.LogInformation("Error");
            return BadRequest(ModelState);
        }

        #endregion

        #region Update

        [HttpPut("{projectId}")]
        public async Task<ActionResult> UpdateProject(
            int peopleId,
            int projectId,
            ProjectForUpdateDto projectDto
            )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound(ModelState);

            var getProject = await _projectReposit.GetProjectAsync(peopleId, projectId);
            if (getProject == null) return NotFound();
            getProject.ModificationDate = DateTime.Now;

            _mapper.Map(projectDto, getProject);
            if (await _projectReposit.SaveChangesAsync())
            {
                var reMap = _mapper.Map<ProjectsDto>(getProject);
                return Ok(reMap);
            }
            _logger.LogInformation($"Error When Updating project: {getProject}");
            return BadRequest(ModelState);
        }

        [HttpPatch("{projectId}")]
        public async Task<ActionResult> PartiallyUpdateProject(
        int peopleId,
        int projectId,
        JsonPatchDocument<ProjectForUpdateDto> patchDocument
            )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _peopleReposit.PeopleExistsAsync(peopleId))
                return NotFound(ModelState);

            var getProject = await _projectReposit.GetProjectAsync(peopleId, projectId);
            if (getProject == null)
                return NotFound();

            var projectToPatch = _mapper.Map<ProjectForUpdateDto>(getProject);
            patchDocument.ApplyTo(projectToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(projectToPatch))
            {
                return BadRequest(ModelState);
            }
            getProject.ModificationDate = DateTime.Now;
            _mapper.Map(projectToPatch, getProject);
            if (await _projectReposit.SaveChangesAsync())
            {
                var reMap = _mapper.Map<ProjectsDto>(getProject);
                return Ok(reMap);
            }
            _logger.LogInformation($"Error When Updating project: {getProject}");
            return BadRequest(ModelState);
        }

        #endregion

        #region Delete

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> HardDeleteProject(int peopleId, int projectId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = await _projectReposit.GetProjectAsync(peopleId, projectId);
            if (project == null)
                return NotFound(ModelState);

            _projectReposit.DeleteProjectAsync(project);
            if (await _projectReposit.SaveChangesAsync())
                return NoContent();

            return BadRequest();
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult> SoftDeleteProject(ProjectForDeleteDto model, int peopleId, int projectId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = await _projectReposit.GetProjectAsync(peopleId, projectId);
            if (project == null)
                return NotFound(ModelState);

            project.ModificationDate = DateTime.Now;

            _mapper.Map(model, project);
            if (await _projectReposit.SaveChangesAsync())
            {
                return NoContent();
            }
            _logger.LogInformation($"Error When Deleting One project: {project}");
            return BadRequest(ModelState);
        }

        #endregion

    }
}
