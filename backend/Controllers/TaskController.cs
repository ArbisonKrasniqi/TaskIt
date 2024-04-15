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

namespace backend.Controllers;
[Route("backend/task")]
[ApiController]

public class  TaskController : ControllerBase{

    private readonly ApplicationDBContext _context;
    private readonly ITaskRepository _taskRepo;

    public TaskController(ApplicationDBContext context, ITaskRepository taskRepo){
        _context = context;
        _taskRepo = taskRepo;

    }

  
    [HttpGet]
    public async Task<IActionResult> GetAllTasks(){
        var task = await _taskRepo.GetAllTaskAsync();
        var taskDto = task.Select(s => s.ToTaskDto());
        return Ok(taskDto);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById([FromRoute] int id){
        var task = await _taskRepo.GetTaskByIdAsync(id);
        if(task == null){
            return NotFound();
        }
        return Ok(task.ToTaskDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask ([FromBody] CreateTaskRequestDTO taskDto){
        var taskModel = taskDto.ToTaskFromCreate();
        await _taskRepo.CreateTaskAsync(taskModel);
        return CreatedAtAction(nameof(GetTaskById), new { id = taskModel.TaskId}, taskModel.ToTaskDto());
    }


   [HttpPut]
   [Route("{id}")]   
   public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] UpdateTaskRequestDTO updateDto){
        var taskModel = await _taskRepo.UpdateTaskAsync(id, updateDto);
        if(taskModel == null){
            return NotFound();
        }

     
        return Ok(taskModel.ToTaskDto());
    }



    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteTask([FromRoute] int id){
        var taskModel = await _taskRepo.DeleteTaskAsync(id);

        if(taskModel == null){
            return NotFound();
        }

      

        return NoContent();
    }




        




    

}
