using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<CommandUpdateDto, Command>();
            CreateMap<Command,CommandUpdateDto>();
        }
    }
}
