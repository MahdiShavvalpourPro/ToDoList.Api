﻿using AutoMapper;
using ToDoList.Api.Data.Entities;
using ToDoList.Api.Models.People;
using ToDoList.Api.Models.Project;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Profiler
{
    public class PeopleProfile : Profile
    {
        public PeopleProfile()
        {

            #region People

            //source | dest

            //Convert People Enity To People For Display Dto
            CreateMap<People, PeopleForDisplayDto>();
            CreateMap<People, PeopleDto>();

            //Convert People For Creation Dto To People
            CreateMap<ProjectForCreationDto, Projects>();


            CreateMap<Projects, ProjectsDto>();
            CreateMap<People, PeopleWithIncludeProjectDto>();

            CreateMap<Tasks, TasksDto>();
            CreateMap<People, PeopleWithIncludeTaskDto>();

            CreateMap<PeopleForCreationDto, People>();

            //CreateMap<JsonPatchDocument, People>();
            CreateMap<People, PeopleForUpdateDto>();
            CreateMap<PeopleForUpdateDto, People>();

            CreateMap<People, PeopleForDeleteDto>();
            CreateMap<PeopleForDeleteDto, People>();

            #endregion


            #region Projects

            CreateMap<Projects, ProjectsDto>();
            CreateMap<Projects, ProjectForCreationDto>();
            CreateMap<Projects, ProjectsDto>();

            CreateMap<ProjectsDto, ProjectForCreationDto>();


            CreateMap<ProjectForUpdateDto, Projects>();
            CreateMap<Projects, ProjectForUpdateDto>();

            CreateMap<ProjectForDeleteDto, Projects>();
            #endregion

            #region Tasks

            CreateMap<TaskForCreationDto, Tasks>();
            CreateMap<Tasks, TasksDto>()
                .ForMember(dest=>dest.TaskName,opt=>opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.TaskStatus,opt=>opt.MapFrom(src=>src.TaskStatus))
                .ForMember(dest=>dest.StartTaskTime,opt=>opt.MapFrom(src=>src.StartTime))
                .ForMember(dest=>dest.EndTaskTime,opt=>opt.MapFrom(src=>src.EndTime))
                .ForMember(dest=>dest.PriorityLevelTask,opt=>opt.MapFrom(src=>src.PriorityLevel))
                .ForMember(dest=>dest.TaskDescription,opt=>opt.MapFrom(src=>src.Description))
                ;


            #endregion

        }
    }
}
