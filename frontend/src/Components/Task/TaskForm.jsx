import React, { useState, useContext } from "react";
import { postData } from "../../Services/FetchService";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { AiOutlineClose } from "react-icons/ai"; // Cross icon

const TaskForm = ({ listId, onClose }) => {
    const { setLists } = useContext(WorkspaceContext);
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
            setLists(prevLists =>
                prevLists.map(list =>
                    list.listId === listId
                        ? { ...list, tasks: [...list.tasks, response.data] } // Add the new task to the correct list
                        : list // Keep other lists unchanged
                )
            );
    
            setTaskTitle('');
            onClose(); // Close the form after task creation
        } catch (error) {
            console.log("Error response data:", error.message); 
            setErrorMessage("Failed to create task. Please try again.");
        }
    };

    return (
        <div className="relative p-4 bg-white border rounded shadow-lg">
            <form onSubmit={handleCreateTask} className="flex flex-col gap-4">
                <input
                    type="text"
                    placeholder="New task"
                    value={taskTitle}
                    onChange={handleTitleChange}
                    className="bg-slate-400 border-none w-full p-2 border rounded text-black placeholder-gray-700 focus:outline-none focus:border-transparent focus:bg-slate-300"
                />
                <div className="flex justify-between items-center">
                    <button
                        type="submit"
                        className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                        Add Task
                    </button>
                    <button
                        onClick={onClose}
                        className="text-gray-500 hover:text-gray-700 ml-2"
                        type="button"
                    >
                        <AiOutlineClose className="w-6 h-6" />
                    </button>
                </div>
                {errorMessage && <p className="text-red-500">{errorMessage}</p>}
            </form>
        </div>
    );
};

export default TaskForm;
