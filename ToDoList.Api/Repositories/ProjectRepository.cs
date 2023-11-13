using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;
        private readonly IPeopleRepository _peopleRepository;

        public ProjectRepository(ApplicationDbContext context, ILogger<ProjectRepository> logger, IPeopleRepository projectRepository)
        {
            _context = context;
            _logger = logger;
            _peopleRepository = projectRepository;
        }
        public async Task AddProjectAsync(int peopleId, Projects project)
        {
            if (await _peopleRepository.PeopleExistsAsync(peopleId))
            {
                await _context.AddAsync(project);
            }
        }

        public void DeleteProjectAsync(Projects projects)
        {
            _context.Tbl_Project.Remove(projects);
        }

        public Task DeleteProjectsAsync(int peopleId)
        {
            throw new NotImplementedException();
        }

        public async Task<Projects?> GetProjectAsync(int peopleId, int projectId)
        {
            return await _context.
                Tbl_Project
                .FirstOrDefaultAsync(x => x.OwnerId == peopleId && x.Id == projectId);
        }

        public async Task<Projects?> GetProjectAsync(int projectId)
        {
            return await _context.Tbl_Project.
                FirstOrDefaultAsync(c => c.Id == projectId);
        }

        public async Task<IEnumerable<Projects?>> GetProjectsAsync(int peopleId)
        {
            return await _context.Tbl_Project.Where(x => x.OwnerId == peopleId).ToListAsync();
        }

        public async Task<bool> ProjectExistsAsync(int peopleId, int projectId)
        {
            if (await _peopleRepository.PeopleExistsAsync(peopleId))
            {
                return await _context
                    .Tbl_Project
                    .AnyAsync(x => x.Id == projectId);
            }
            return false;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex);
                return false;
            }
        }

        //public async Task UpdateProjectAsync(int peopleId, int projectId, Projects project)
        //{
        //    var getProject = await _context.Tbl_Project.FirstOrDefaultAsync(x => x.OwnerId == peopleId && x.Id == projectId);
        //    getProject!.Name = project.Name;
        //    getProject.ProjectStatus = project.ProjectStatus;
        //    getProject.PriorityLevel = project.PriorityLevel;
        //    getProject.Descrption = project.Descrption;
        //}
    }
}
