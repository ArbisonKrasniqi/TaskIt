import {useContext, useEffect} from 'react';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import {useNavigate, useParams} from 'react-router-dom';
const Table = () =>{

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US');
    };

    const {opened}=useParams();
    const {tasks, getInitials, WorkspaceId, getTasks} = useContext(WorkspaceContext);
    useEffect(()=>{
       
            getTasks();
    
    }, [opened]);

    const navigate = useNavigate();
    return(
        <div className="min-h-screen h-full overflow-y-auto" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
         <div className="font-semibold font-sans text-gray-400 flex justify-normal flex-col">
         <h2 className="text-2xl mt-5 ml-5">Table</h2>
         <table className="table-auto w-full text-left text-gray-400 mt-10">
        <thead className='border-b border-gray-400'>
                    <tr>
                        <th className="px-4 py-2">Task</th>
                        <th className="px-4 py-2">List</th>
                        <th className="px-4 py-2">Board</th>
                        <th className="px-4 py-2">Labels</th>
                        <th className="px-4 py-2">Members</th>
                        <th className="px-4 py-2">Due Date</th>
                    </tr>
                </thead>
            <tbody>
                {tasks.length === 0 ? (
                    <tr>
                        <td className="px-4 py-2" colSpan="6">No tasks available</td>
                    </tr>
                ) : tasks.map((task, index)=>(
                    <tr key={index} className="hover:bg-gray-700 hover:cursor-pointer"
                    // onClick={() => navigate(`/main/board/${WorkspaceId}/${task.boardId}/${task.taskId}`)}
                    >
                        <td className="px-4 py-2">{task.taskTitle}</td>
                        <td className="px-4 py-2">{task.listTitle}</td>
                        <td className="px-4 py-2">{task.boardTitle}</td>

                        {/* Labels */}
                      
                        <td className="px-4 py-2">
                        {task.labels && task.labels.length > 0 ? (
                            <div className="flex flex-col">
                             {task.labels.map(label => (
                             <span 
                                key={label.labelId} 
                                className="inline-block rounded-full px-3 py-1 text-sm font-semibold text-white mb-2"
                                style={{ backgroundColor: label.color }}>
                               {label.name || " "}
                             </span>
                        ))}
                      </div>
    ) : (
        <span>No labels</span>
    )}
</td>

                        {/* Members */}
                        <td className="px-4 py-2">
    {task.taskMembers && task.taskMembers.length > 0 ? (
        <div className="flex flex-col">
            {task.taskMembers.map(member => (
                <div key={member.taskMemberId} className='flex flex-row p-4 text-gray-400'>
                    <div className="flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-white bg-gradient-to-r from-orange-400 to-orange-600">
                        {getInitials(member.firstName, member.lastName)}
                    </div>
                    <div className="ml-2 mt-1">
                        <div className="text-sm font-medium text-gray-200">
                            {member.firstName} {member.lastName}
                        </div>
                    </div>
                </div>
            ))}
        </div>
    ) : (
        <span>No members</span>
    )}
</td>

                        <td className="px-4 py-2">{(formatDate(task.dueDate) != '1/1/1' )? (formatDate(task.dueDate)) : ""} </td>
                    </tr>
                ))}
            </tbody>
        </table>
        </div>
        </div>
    );
};

export default Table;