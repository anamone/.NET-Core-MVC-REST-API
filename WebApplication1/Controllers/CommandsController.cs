using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Get api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        // Get api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if(commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound();  
        }
        //Post api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }
        //Put api/commands/{id}
        [HttpPut("{id}")]
        // მარტო ერთ ველს ვერ შეცვლი, მთლიანად იცვლება
        public ActionResult UpdateCommand(int id,CommandUpdateDto commandUpdateDto)
        {
            var CommandModel = _repository.GetCommandById(id);
            if (CommandModel == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, CommandModel); //ეს აკეთებს პირდაპირ update-ს,CommandModel-ში ჩაწერს commandUpdateDto-ს

            _repository.UpdateCommand(CommandModel); // ეს არაფერს აკეთებს, ისე უბრალოდ არის ჩაწერილი
            _repository.SaveChanges();

            return NoContent();
        }
        //Patch api/commands/{id}
        [HttpPatch("{id}")]
        // ერთი ველი შეცვლა შეუძლია
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var CommandModel = _repository.GetCommandById(id);
            if (CommandModel == null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<CommandUpdateDto>(CommandModel);
            patchDoc.ApplyTo(commandToPatch,ModelState);
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem();
            }
            _mapper.Map(commandToPatch, CommandModel); //ეს აკეთებს პირდაპირ update-ს,CommandModel-ში ჩაწერს commandToPatch-ს

            _repository.UpdateCommand(CommandModel); // ეს არაფერს აკეთებს, ისე უბრალოდ არის ჩაწერილი
            _repository.SaveChanges();

            return NoContent();
        }
        //Delete api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommadn(int id)
        {
            var CommandModel = _repository.GetCommandById(id);
            if (CommandModel == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(CommandModel);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
