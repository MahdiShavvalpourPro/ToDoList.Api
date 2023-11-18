using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoList.Api.Data;
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

        #region Delete

        [HttpDelete]
        public async Task<ActionResult> HardDelete(int peopleId, int projectId, int taskId)
        {
            if (!await _taskRepository.TaskExistsAsync(peopleId, projectId, taskId))
                return NotFound();

            var task = await _taskRepository.GetTaskAsync(peopleId, projectId, taskId);

            _taskRepository.DeleteTaskAsync(task);
            if (await _taskRepository.SaveChangesAsync())
            {
                return Ok();
            }
            _logger.LogInformation($"{nameof(HardDelete)}: {task}");
            return BadRequest();
        }

        [HttpPatch("{taskId}")]
        public async Task<ActionResult> SoftDelete
            (
            JsonPatchDocument<TaskForDeleteDto> document,
            int peopleId,
            int projectId,
            int taskId)
        {
            if (!await _peopleRepository.PeopleExistsAsync(peopleId))
            {
                _logger.LogInformation($"people With Id {peopleId} Not Found");
                return NotFound();
            }

            if (!await _projectRepository.ProjectExistsAsync(peopleId, projectId))
            {
                _logger.LogInformation($"people with project Id {projectId} not found");
                return NotFound();
            }

            if (!await _taskRepository.TaskExistsAsync(peopleId, projectId, taskId))
            {
                _logger.LogInformation($"Task With Id {taskId} Not Found");
                return NotFound();
            }

            var taskToPatch = _mapper.Map<TaskForDeleteDto>(document);
            document.ApplyTo(taskToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(taskToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map<TasksDto>(document);
            await _taskRepository.SaveChangesAsync();
            return Ok();


        }

        #endregion

        #region Update Tasks

        [HttpPatch("taskId")]
        public async Task<ActionResult> UpdateTask(
            int peopleId,
            int projectId,
            int taskId,
            Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<TaskForUpdateDto> document
            )
        {
            #region validation

            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _peopleRepository.PeopleExistsAsync(peopleId))
                return NotFound(ModelState);

            if (await _projectRepository.ProjectExistsAsync(projectId, taskId))
                return NotFound(ModelState);

            var task =await _taskRepository.GetTaskAsync(peopleId, projectId, taskId);
            if (task == null)
                return NotFound(ModelState);

            #endregion
            var taskToPatch = _mapper.Map<TaskForUpdateDto>(task);
            document.ApplyTo(taskToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(taskToPatch))
            {
                return BadRequest(ModelState);
            }
            task.ModificationDate = DateTime.Now;
            _mapper.Map(taskToPatch,task);
            if(await _taskRepository.SaveChangesAsync())
            {
                var reMap=_mapper.Map<TaskForUpdateDto>(task);
                return Ok(reMap);
            }
            _logger.LogInformation($"Error When Updating Task: {task}");
            return BadRequest(ModelState);
        }

        #endregion
    }
}
