using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Project;
using ToDoList.Api.Models.Task;
using ToDoList.Api.Repositories;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/people/{peopleId}/projects/{projectId}/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<TaskController> _logger;
        private readonly IMapper _mapper;


        public TaskController(
            ITaskRepository taskRepository,
            ILogger<TaskController> logger,
            IMapper mapper,
            IProjectRepository projectRepository,
            IPeopleRepository peopleRepository
            )
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _peopleRepository = peopleRepository;
        }



        #region Task creation method

        [HttpPost]
        public async Task<ActionResult> InsertTask(
            int peopleId,
            int projectId,
            TaskForCreationDto model
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _peopleRepository.PeopleExistsAsync(peopleId))
            {
                _logger.LogInformation($"The user with ID {peopleId} not found");
                return BadRequest($"The user with ID {peopleId} not found");
            }

            if (!await _projectRepository.ProjectExistsAsync(peopleId, projectId))
            {
                _logger.LogInformation($"The user with ID {peopleId} does not have a project");
                return BadRequest($"The user with ID {peopleId} does not have a project");
            }

            var data = _mapper.Map<Tasks>(model);
            if (await _taskRepository.InsertTaskAsync(peopleId, projectId, data))
            {
                return Ok();
            }
            _logger.LogInformation("A problem occurred while creating the task");
            return BadRequest(ModelState);
        }

        #endregion

        #region Get Data

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasksDto>>> GetAllTasks(int peopleId, int projectId)
        {
            if (!await _peopleRepository.PeopleExistsAsync(peopleId))
            {
                _logger.LogInformation($"The user with ID {peopleId} not found");
                return BadRequest($"The user with ID {peopleId} not found");
            }

            if (!await _projectRepository.ProjectExistsAsync(peopleId, projectId))
            {
                _logger.LogInformation($"The user with ID {peopleId} does not have a project");
                return BadRequest($"The user with ID {peopleId} does not have a project");
            }

            var tasks = await _taskRepository.GetAllTasksAsync(peopleId, projectId);
            return Ok(
                _mapper.Map<IEnumerable<TasksDto>>(tasks)
                );
        }

        #endregion
    }
}
