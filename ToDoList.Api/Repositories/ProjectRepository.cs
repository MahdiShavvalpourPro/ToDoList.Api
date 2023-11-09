using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ApplicationDbContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddProjectAsync(int peopleId, Projects project)
        {
            if (await PeopleExists(peopleId))
            {
                await _context.AddAsync(project);
            }
        }

        public Task DeleteProjectAsync(int peopleId, int projectId)
        {
            throw new NotImplementedException();
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

        public async Task<IEnumerable<Projects?>> GetProjectsAsync(int peopleId)
        {
            return await _context.Tbl_Project.Where(x => x.OwnerId == peopleId).ToListAsync();
        }

        public async Task<bool> PeopleExists(int peopleId)
        {
            return await _context.Tbl_People.AnyAsync(p => p.Id == peopleId);
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
