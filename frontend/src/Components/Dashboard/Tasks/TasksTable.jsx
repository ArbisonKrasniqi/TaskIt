import React, { useState, useContext, createContext } from 'react';
import UpdateTaskButton from './Buttons/UpdateTaskButton';
import CustomButton from '../Buttons/CustomButton';
import { TasksContext } from './TasksList';
import { deleteData } from '../../../Services/FetchService';
import { useNavigate } from 'react-router-dom';
import { DashboardContext } from '../../../Pages/dashboard';

export const UpdateContext = createContext();

const TasksTable = () => {
    const navigate = useNavigate();
    const tasksContext = useContext(TasksContext);
    const dashboardContext = useContext(DashboardContext);
    const [searchQuery, setSearchQuery] = useState('');

    const HandleTaskDelete = (id) => {
        async function deleteTask() {
            try {
                const data = {
                    taskId: id
                };
                const response = await deleteData('/backend/task/DeleteTask', data);
                console.log(response);
                const updatedTasks = tasksContext.tasks.filter(task => task.taskId !== id);
                tasksContext.setTasks(updatedTasks);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                tasksContext.getTasks();
            }
        }
        deleteTask();
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    // Ensure tasksContext.tasks is always an array
    const tasks = tasksContext.tasks || [];

    // Convert searchQuery to lowercase for case-insensitive search
    const lowercasedQuery = searchQuery.toLowerCase();
    
    const filteredTasks = tasks.filter(task => {
        const taskIdString = task.taskId.toString().toLowerCase();
        const listIdString = task.listId.toString().toLowerCase();
        const titleMatch = task.title.toLowerCase().includes(lowercasedQuery);
        const descriptionMatch = task.description.toLowerCase().includes(lowercasedQuery);
        const taskIdMatch = taskIdString.includes(lowercasedQuery);
        const listIdMatch = listIdString.includes(lowercasedQuery);

        // Search for date fields by converting dates to strings
        const dueDateMatch = task.dueDate.toLowerCase().includes(lowercasedQuery);
        const dateAddedMatch = task.dateAdded.toLowerCase().includes(lowercasedQuery);

        return (
            taskIdMatch ||
            titleMatch ||
            descriptionMatch ||
            listIdMatch ||
            dueDateMatch ||
            dateAddedMatch
        );
    });

    return (
        <div className='flex flex-col h-full'>
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for tasks by ID, title, description, list ID, due date, or date added"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            {/* Scrollable Table Container */}
            <div className="flex-grow overflow-x-auto">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">Task ID</th>
                            <th className="px-6 py-3">Title</th>
                            <th className="px-6 py-3">Description</th>
                            <th className="px-6 py-3">List ID</th>
                            <th className="px-6 py-3">Due Date</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredTasks.length > 0 ? (
                            filteredTasks.map((task, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{task.taskId}</td>
                                    <td className="px-6 py-4">{task.title}</td>
                                    <td className="px-6 py-4">{task.description}</td>
                                    <td className="px-6 py-4">{task.listId}</td>
                                    <td className="px-6 py-4">{task.dueDate}</td>
                                    <td className="px-6 py-4">{task.dateAdded}</td>
                                    <td className="px-6 py-4">
                                        <UpdateContext.Provider value={task}>
                                            <UpdateTaskButton/>
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleTaskDelete(task.taskId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={7}>No tasks found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default TasksTable;
