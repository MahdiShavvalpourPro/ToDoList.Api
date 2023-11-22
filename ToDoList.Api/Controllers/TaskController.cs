using AutoMapper;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Infos;
using ToDoList.Api.Models.Project;
using ToDoList.Api.Models.Task;
using ToDoList.Api.Repositories;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/people/{peopleId}/projects/{projectId}/[action]/tasks")]
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
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(_projectRepository));
            _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
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
        public async Task<ActionResult<IEnumerable<TaskInfos>>> GetAllTasks(int peopleId, int projectId)
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

            var tasksList = await _taskRepository.GetAllTaskInfosAsync(peopleId, projectId);
            return Ok(
                //k_mapper.Map<IEnumerable<TasksDto>>(tasks)
                tasksList
                );
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskInfos>> GetTask(int peopleId, int projectId, int taskId)
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

            var taskInfos = await _taskRepository.GetTaskInfoAsync(peopleId, projectId, taskId);
            return Ok(
                //_mapper.Map<IEnumerable<TasksDto>>(tasks)
                taskInfos
                );
        }

        #endregion

        #region Update Tasks

        [HttpPatch("taskId")]
        public async Task<ActionResult> UpdateTask(
            int peopleId,
            int projectId,
            int taskId,
            JsonPatchDocument<TaskForUpdateDto> document
            )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _peopleRepository.PeopleExistsAsync(peopleId))
                return NotFound(ModelState);

            var project = await _projectRepository.GetProjectAsync(peopleId, projectId);
            if (project is null)
                return NotFound(ModelState);

            var getTask = await _taskRepository.GetTaskAsync(peopleId, projectId, taskId);
            if (getTask == null)
                return NotFound(ModelState);

            var taskToPatch = _mapper.Map<TaskForUpdateDto>(getTask);
            document.ApplyTo(taskToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(taskToPatch))
            {
                return BadRequest(ModelState);
            }
            getTask.ModificationDate = DateTime.Now;
            _mapper.Map(taskToPatch, getTask);
            if (await _taskRepository.SaveChangesAsync())
            {
                var reMap = _mapper.Map<TaskForUpdateDto>(getTask);
                _projectRepository.ChangeProjectStatus(project, Status.Done);
                return Ok(reMap);
            }
            _logger.LogInformation($"Error When Updating project: {getTask}");
            return BadRequest(ModelState);
        }

        #endregion

        #region Delete Task

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int peopleId, int projectId, int taskId)
        {
            var task = await _taskRepository.GetTaskAsync(peopleId, projectId, taskId);
            if (task is null)
                return BadRequest(ModelState);

            _taskRepository.DeleteTaskAsync(task);
            if (!await _taskRepository.SaveChangesAsync())
                return BadRequest(ModelState);

            return Ok("One Task Delete Successfully");

        }

        #endregion
    }
}
