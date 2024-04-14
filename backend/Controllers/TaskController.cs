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

    public TaskController(ApplicationDBContext context){
        _context = context;

    }

  
    [HttpGet]
    public async Task<IActionResult> GetAllTasks(){
        var task = await _taskRepo.GetAllTaskAsync();
        var taskDto = task.Select(s => s.ToWorkspaceDto());
        return Ok(taskDto);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById([FromRoute] int id){
        var task = await _taskRepo.GetTaskByIdAsync(id);
        if(task == null){
            return NotFound();
        }
        return Ok(task.ToWorkspaceDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask ([FromBody] CreateTaskRequestDTO taskDto){
        var taskModel = taskDto.ToTaskFromCreate();
        await _taskRepo.CreateTaskAsync(taskModel);
        return CreatedAtAction(nameof(GetTaskById), new { id = taskModel.TaskId}, taskModel.ToTaskDto());
    }

   




        




    

}
