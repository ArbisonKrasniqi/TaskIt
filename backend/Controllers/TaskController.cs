using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;
using backend.DTOs.Task;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.DTOs.List;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace backend.Controllers;


[Route("backend/task")]
[ApiController]

public class  TaskController : ControllerBase{

    private readonly IListRepository _listRepo;
    private readonly ITaskRepository _taskRepo;

    public TaskController(ITaskRepository taskRepo, IListRepository listRepo){
        _listRepo = listRepo;
        _taskRepo = taskRepo;

    }

  
    [HttpGet("GetAllTasks")]
    public async Task<IActionResult> GetAllTasks(){
        try{
            var tasks = await _taskRepo.GetAllTaskAsync();
            if(tasks.Count == 0){
                return NotFound("No tasks found");
            }

            var taskDto = tasks.Select(x => x.ToTaskDto());
            return Ok(taskDto);

        }catch(Exception e){
            return StatusCode(500, e.Message);
        }
    }
    

    [HttpGet("GetTaskById")]
    public async Task<IActionResult> GetTaskById (int TaskId){
        try{
            var tasks = await _taskRepo.GetTaskByIdAsync(TaskId);

            if(tasks == null){
                return NotFound("Task not found");
            }
            return Ok(tasks.ToTaskDto());
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error");
        }

    }

    [HttpPut("UpdateTask")]
    public async Task<IActionResult> UpdateTask (UpdateTaskRequestDTO taskDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{

            var taskModel = await _taskRepo.UpdateTaskAsync(taskDto);

            if(taskModel == null){
                return NotFound("Task not found");
            }
            return Ok(taskModel.ToTaskDto());

        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!");
        }
        
    }

    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask (int taskId){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
            var taskModel = await _taskRepo.DeleteTaskAsync(taskId);
            if(taskModel == null){
                return NotFound("Task dose not exists");
            }
            return Ok("Task Deleted");


        }catch(Exception e){
            return StatusCode(500, "Internal Server Error ");
        }

    }



    // relationship task and list 1-many
    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask (CreateTaskRequestDTO taskDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        try{
            if(!await _listRepo.ListExists(taskDto.ListId)){
                return BadRequest("List did not exist");
                
            }
            var taskModel = taskDto.ToTaskFromCreate();
            await _taskRepo.CreateTaskAsync(taskModel);
            return CreatedAtAction(nameof(GetTaskById), new { id = taskModel.TaskId}, taskModel.ToTaskDto());

        }catch(Exception e){
            return StatusCode(500, e.Message);
        }

    }

    [HttpGet ("GetTaskByListId")]

    public async Task<IActionResult> GetTaskByListId (int listId){
        try{
            if (!await _listRepo.ListExists(listId))
            {
                return NotFound("List Not Found");
            }
            
            var tasks = await _taskRepo.GetTaskByListId(listId);

            if (tasks.Count == 0)
            {
                return BadRequest("Task Not Found");
            }

            return Ok(tasks);

        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!");
        }

    }

    [HttpDelete("DeleteTaskByListId")]

    public async Task<IActionResult> DeleteTaskByListId (ListIdDTO listIdDTO){

        if(!await _listRepo.ListExists(listIdDTO.ListId)){
            return StatusCode(404, "List Not Found");
        }

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }


        try{

            var taskModel = await _taskRepo.DeleteTaskByListIdAsync(listIdDTO.ListId);

            if(taskModel.Count == 0){
                return NotFound ("Task not found");
            }
            return Ok("Task Deleted");

        }catch(Exception e){
            return StatusCode(404, "Internal Server Error!");
        }

    }


}