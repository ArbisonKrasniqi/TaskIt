import React, { useContext, useState } from "react";
import { postData } from "../../Services/FetchService";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const TaskForm = ({ listId }) => {
    // const {setLists} = useContext(WorkspaceContext);
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
            console.log("Task created successfully:", response.data); 
            // setLists(prevLists =>
            //     prevLists.map(list =>
            //         list.listId === listId
            //             ? { ...list, tasks: [...list.tasks, response.data] } // Add the new task to the correct list
            //             : list // Keep other lists unchanged
            //     )
            // );
    
            setTaskTitle('');
        } catch (error) {
            console.log("Error response data:", error.message); 
            setErrorMessage("Failed to create task. Please try again.");
        }
    };

    return (
        <form onSubmit={handleCreateTask}>
            <input
                type="text"
                placeholder="New task"
                value={taskTitle}
                onChange={handleTitleChange}
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
