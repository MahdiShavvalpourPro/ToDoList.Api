using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationDbContext _context;

        public PeopleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddManyPeople(IEnumerable<People> people)
        {
            if (people == null) throw new ArgumentNullException(nameof(people));
            await _context.
                Tbl_People.
                AddRangeAsync(people);
        }

        public async Task AddPeopleAsync(People people)
        {
            if (people == null) throw new ArgumentNullException(nameof(people));
            await _context.Tbl_People.AddAsync(people);
        }

        public async Task<bool> DeletePeopleAsync(int peopleId)
        {
            var people = await GetPeopleByIdAsync(peopleId);
            if (people != null)
            {
                _context.Tbl_People.Remove(people!);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<People>> GetPeopleAsync()
        {
            return await _context
                .Tbl_People.Where(p => p.IsRemove == false)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<People?> GetPeopleByIdAsync(int peopleId, bool includeProjects = false, bool includeTasks = false)
        {
            //if (includeProjects && includeTasks)
            //{
            //    return await _context
            //        .Tbl_People
            //        .Include(p => p.UserTasks)
            //        .Include(p => p.ProjectsList)
            //        .FirstOrDefaultAsync(c => c.Id == peopleId);
            //}
            if (includeProjects)
            {
                return await _context
                    .Tbl_People
                    .Include(p => p.ProjectsList)
                    .FirstOrDefaultAsync(p => p.Id == peopleId);
            }
            if (includeTasks)
            {
                var data = await _context
                     .Tbl_People
                     .Include(p => p.UserTasks)
                     .FirstOrDefaultAsync(p => p.Id == peopleId);
                return data;
            }

            return await _context
                .Tbl_People
                .FirstOrDefaultAsync(p => p.Id == peopleId);
        }

        public async Task<bool> PeopleExistsAsync(int peopleId)
        {
            return await _context.Tbl_People.AnyAsync(x => x.Id == peopleId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdatePeopleAsync(int peopleId, People people)
        {
            if (await PeopleExistsAsync(peopleId))
            {
                var result = await _context.Tbl_People.FirstOrDefaultAsync(x => x.Id == peopleId);
                result!.FirstName = people.FirstName;
                result.LastName = people.LastName;
                result.Email = people.Email;
                result.MobileNumber = people.MobileNumber;
                result.CreationDate = people.CreationDate;
                result.ModificationDate = DateTime.Now;
                result.IsRemove = false;
            }
        }
    }
}
