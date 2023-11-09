using AutoMapper;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoList.Api.Data;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.Project;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/Test/{peopleId}")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public TestController(ILogger<TestController> logger, IMapper mapper, ApplicationDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        #region CreateFakeData

        //[HttpPost]
        //public async Task<ActionResult> InsertFakeDataToPeople()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    var peopleList = new List<PeopleForDisplayDto>()
        //    {
        //        new PeopleForDisplayDto()
        //        {
        //            FirstName="Mahdi",
        //            LastName="Shavvalpour",
        //            MobileNumber="09398911063",
        //            Email=null
        //        },
        //         new PeopleForDisplayDto()
        //        {
        //            FirstName="Ali",
        //            LastName="Ahmadi",
        //            MobileNumber="093989114777",
        //            Email="Ali@Ahmadi.com"
        //        },
        //          new PeopleForDisplayDto()
        //        {
        //            FirstName="Iman",
        //            LastName="Madaeni",
        //            MobileNumber="09142563398",
        //            Email="Iman@gmail.com"
        //        },
        //           new PeopleForDisplayDto()
        //        {
        //            FirstName="Mohammad",
        //            LastName="Hashemi",
        //            MobileNumber="09138911063",
        //            Email=null
        //        }
        //    };
        //    var data = peopleList.Select(person => _mapper.Map<People>(person)).ToList();

        //    await _context.Tbl_People.AddRangeAsync(data);
        //    await _context.SaveChangesAsync();
        //    _logger.LogInformation($"{peopleList.Count} of people added to tbl_people");
        //    return NoContent();
        //}

        [HttpPost]
        public async Task<ActionResult> InsertFakeDataToProject(int peopleId, [FromBody] ProjectForCreationDto project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var owner = _context.Tbl_People.Find(peopleId);
            if (owner == null)
            {   
                return NotFound();
            }
            //project.OwnerId = peopleId;
            //var data = projects.Select(project => _mapper.Map<Projects>(project)).ToList();
            var data = _mapper.Map<Projects>(project);

            await _context.Tbl_Project.AddAsync(data);
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation($"One Project Added For {owner.LastName}");
            return NoContent();
        }

        #endregion
    }
}
