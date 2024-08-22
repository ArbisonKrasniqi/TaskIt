import React, { useContext, createContext} from 'react';
import UpdateTaskButton from './Buttons/UpdateTaskButton';
import CustomButton from '../Buttons/CustomButton';
import { TasksContext } from './TasksList';
import { deleteData } from '../../../Services/FetchService';

export const UpdateContext = createContext();

const TasksTable = () => {

    const tasksContext = useContext(TasksContext);

    const HandleTaskDelete = (id) => {
        async function deleteTask() {
            try {
                const data = {
                    taskId: id
                };
                const response = await deleteData('/backend/task/DeleteTask', data);
                console.log(response);
                const updatedTasks = tasksContext.tasks.filter(task => task.taskID !== id);
                tasksContext.setTasks(updatedTasks);
            } catch (error) {
                tasksContext.setErrorMessage(error.message + id);
                tasksContext.setShowTasksErrorModal(true);
                tasksContext.getTasks();
            }
        }
        deleteTask();
    }
    return (
        <div className='overflow-x-auto'>
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
                <th className="px-6 py-3" >ID</th>
                <th className="px-6 py-3" >Title</th>
                <th className="px-6 py-3" >Description</th>
                <th className="px-6 py-3" >ListId</th>    
                <th className="px-6 py-3" >dueDate</th> 
                <th className="px-6 py-3" >dateCreated</th>     
                <th className="px-6 py-3" >Actions</th> 
            </tr>     
        </thead>

        <tbody>
            {tasksContext.tasks ? (tasksContext.tasks.map((task, index) => (
                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    <td className="px-6 py-4">{task.taskID}</td>
                    <td className="px-6 py-4">{task.title}</td>
                    <td className="px-6 py-4">{task.description}</td>
                    <td className="px-6 py-4">{task.listId}</td>
                    <td className="px-6 py-4">{task.dueDate}</td>
                    <td className="px-6 py-4">{task.dateAdded}</td>
                    <td className="px-6 py-4"> <UpdateContext.Provider value={task}> <UpdateTaskButton/>  </UpdateContext.Provider> <CustomButton color="red" text="Delete" onClick={()=>{ HandleTaskDelete(task.taskID)}}/> </td>
                </tr>
            ))): (
                <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                </tr>
            )}
        </tbody>


        </table>
      </div>  
    );
}

export default TasksTable