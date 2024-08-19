import {useContext, useEffect, useState} from 'react';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { getDataWithId } from '../../Services/FetchService';
const Table = () =>{

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        // Format date to 'MM/DD/YYYY' or any other format you prefer
        return date.toLocaleDateString('en-US');
    };
    const {WorkspaceId} = useContext(WorkspaceContext);
    const [tasks, setTasks] = useState([]);
    
    useEffect(()=>{
    const getTasks =async ()=>{

        try{
            const tasksResponse = await getDataWithId('http://localhost:5157/backend/task/GetTasksByWorkspaceId?workspaceId', WorkspaceId);
            const tasksData = tasksResponse.data;
            console.log("Tasks data: ",tasksData);
            setTasks(tasksData);
        }catch (error) {
            console.error(error.message);
        }
    };
   
        getTasks();
        console.log("Workspace id ",WorkspaceId);
        console.log("Tasks fetched: ",tasks);
    }, [WorkspaceId]);

    return(
        <div className="min-h-screen h-full" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
         <div className="font-semibold font-sans text-gray-400 flex justify-normal flex-col">
         <h2 className="text-2xl mt-5 ml-5">Table</h2>
         <table className="table-auto w-full text-left text-gray-400 mt-10">
        <thead className='border-b border-gray-400'>
                    <tr>
                        <th className="px-4 py-2">Task</th>
                        <th className="px-4 py-2">List</th>
                        <th className="px-4 py-2">Description</th>
                        <th className="px-4 py-2">Labels</th>
                        <th className="px-4 py-2">Members</th>
                        <th className="px-4 py-2">Due Date</th>
                    </tr>
                </thead>
            <tbody>
                {tasks.length === 0 ? (
                    <tr>
                    <th className="px-4 py-2"></th>
                    <th className="px-4 py-2"></th>
                    <th className="px-4 py-2"></th>
                    <th className="px-4 py-2"></th>
                    <th className="px-4 py-2"></th>
                </tr>
                ) : tasks.map((task, index)=>(
                    <tr key={index} className="hover:bg-gray-700">
                    <td className="px-4 py-2">{task.title}</td>
                    <td className="px-4 py-2"> </td>
                    <td className="px-4 py-2">{task.description}</td>
                    {/* <td className="px-4 py-2">
                        {task.labels.map(label => (
                            <span key={label} className="inline-block bg-blue-500 rounded-full px-3 py-1 text-sm font-semibold text-white mr-2">
                                {label}
                            </span>
                        ))}
                    </td> */}
                    {/* <td className="px-4 py-2">
                        {task.members.map(member => (
                            <span key={member} className="inline-block bg-green-500 rounded-full px-3 py-1 text-sm font-semibold text-white mr-2">
                                {member}
                            </span>
                        ))}
                    </td> */}
                    <td className="px-4 py-2"> </td>
                    <td className="px-4 py-2"> </td>
                    <td className="px-4 py-2">{formatDate(task.dueDate)}</td>
                </tr>
                ))}
               
            </tbody>
        </table>
        </div>
        </div>
    );



};
export default Table