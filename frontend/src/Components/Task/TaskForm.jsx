import React, { useContext, useState } from "react";
import { postData } from "../../Services/FetchService";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { BoardContext } from "../BoardContent/Board";


const TaskForm = ({ listId }) => {
    const boardContext = useContext(BoardContext);
    const [taskTitle, setTaskTitle] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleTitleChange = (e) => {
        setTaskTitle(e.target.value);
    };

    const handleCreateTask = async (e) => {
        e.preventDefault();
        if (taskTitle.length < 2 || taskTitle.length > 20) {
            setErrorMessage("Task title must be between 2 and 20 characters.");
            return;
        }

        const newTask = {
            title: taskTitle,
            listId: listId,
        };

        console.log("Creating task for listId:", listId); 

        try {
            const response = await postData("/backend/task/CreateTask", newTask);
            var createdTask = response.data;
            createdTask.uniqueId = `${response.data.taskId}-${response.data.listId}`;
            console.log("Task created successfully:", createdTask);
             
            boardContext.setTasks([...boardContext.tasks, createdTask]);
            setErrorMessage("");
            setTaskTitle('');
        } catch (error) {
            console.log("Error response data:", error.message); 
            setErrorMessage("error.message");
        }
    };

    return (
        <form onSubmit={handleCreateTask}>
            <input
                type="text"
                placeholder="New task"
                value={taskTitle}
                onChange={handleTitleChange}
                onClick={(e) => {e.stopPropagation()}}
                className="bg-slate-400 border-none w-full p-2 mb-2 border rounded text-black placeholder-gray-700 focus:outline-none focus:border-transparent focus:bg-slate-300"
            />
            <button
                type="submit"
                className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
            >
                Add Task
            </button>
            {errorMessage && <p className="text-red-500">{errorMessage}</p>}
        </form>
    );
};

export default TaskForm;
