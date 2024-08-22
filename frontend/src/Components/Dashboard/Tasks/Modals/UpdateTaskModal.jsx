import React, { useState, useContext } from 'react';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css"; // Import the styles
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../TasksTable';
import { TasksContext } from '../TasksList';
import CustomButton from '../../Buttons/CustomButton';

const UpdateTaskModal = (props) => {
    const updateContext = useContext(UpdateContext);
    const tasksContext = useContext(TasksContext);

    // Initialize date
    const initialDueDate = updateContext.dueDate ? new Date(updateContext.dueDate) : new Date();

    const [title, setTitle] = useState(updateContext.title);
    const [description, setDescription] = useState(updateContext.description);
    const [dueDate, setDueDate] = useState(initialDueDate);
    const [listId, setListId] = useState(updateContext.listId);

    // Format date as "2024-07-27T04:30:00" without converting to UTC
    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            // Format dueDate for submission
            const formattedDueDate = formatDate(dueDate);

            const data = {
                taskId: updateContext.taskID,
                title: title,
                description: description,
                dueDate: formattedDueDate,
                listId: listId,
            };

            const response = await putData('/backend/task/UpdateTask', data);
            console.log(response.data);

            const updatedTasks = tasksContext.tasks.map(task => {
                if (task.taskID === updateContext.taskID) {
                    return {
                        ...task,
                        title: title,
                        description: description,
                        dueDate: formattedDueDate,  // Ensure this is a string
                        listId: listId,
                    };
                } else {
                    return task;
                }
            });

            tasksContext.setTasks(updatedTasks);
            props.setShowUpdateInfoModal(false);
            
        } catch (error) {
            tasksContext.setErrorMessage(error.message);
            tasksContext.setShowTasksErrorModal(true);
            tasksContext.getTasks();

            props.setShowUpdateInfoModal(false);
        }
    }

    return (
        <div className='fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50'>
            <form onSubmit={handleSubmit} className='bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto'>
                <div className='mb-6'>
                    <label htmlFor="title" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Title</label>
                    <input value={title}
                           onChange={(e) => setTitle(e.target.value)}
                           type='text'
                           id='title'
                           className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                </div>

                <div className='mb-6'>
                    <label htmlFor="description" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Description</label>
                    <textarea value={description}
                              onChange={(e) => setDescription(e.target.value)}
                              id='description'
                              className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                </div>

                <div className='mb-6'>
                    <label htmlFor="listId" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">List ID</label>
                    <input value={listId}
                           onChange={(e) => setListId(e.target.value)}
                           type="text"
                           id="listId"
                           className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                </div>

                <div className='mb-6'>
                    <label htmlFor="dueDate" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Due Date</label>
                    <DatePicker
                        selected={dueDate}
                        onChange={(date) => setDueDate(date)}
                        showTimeSelect
                        dateFormat="yyyy-MM-dd'T'HH:mm:ss"
                        timeFormat="HH:mm:ss"
                        className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500'
                    />
                </div>

                <div className='flex justify-around'>
                    <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed" />
                    <CustomButton type="submit" text="Update" color="longGreen" />
                </div>
            </form>
        </div>
    );
}

export default UpdateTaskModal;
