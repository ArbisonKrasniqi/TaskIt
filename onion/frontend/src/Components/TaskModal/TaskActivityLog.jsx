import React, {useContext, useState, useEffect} from 'react';
import { RxActivityLog } from "react-icons/rx";
import TaskComment from "./TaskComment";
import { TaskModalsContext } from "./TaskModal";
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useParams } from 'react-router-dom';
import { getDataWithId } from '../../Services/FetchService';
import { MainContext } from '../../Pages/MainContext';

const TaskActivityLog = () => {

    const {taskId} = useParams();
    const {getInitials, getInitialsFromFullName} = useContext(WorkspaceContext);
    const {userInfo} = useContext(MainContext);
    const {taskActivities, getTaskActivities} = useContext(TaskModalsContext);
    
    const [activityLimit, setActivityLimit] = useState(5);
    useEffect(() => {
        getTaskActivities();
    },[taskId]);

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        const formattedDate = date.toLocaleDateString('en-US');
        const formattedTime = date.toLocaleTimeString('en-US', {
            hour: '2-digit',
            minute: '2-digit',
            hour12: true 
        });
        return `${formattedDate} - ${formattedTime}`;
    };

    const loadMoreActivities = () => {
        setActivityLimit((prevLimit) => prevLimit + 5); 
    };

    return (
        <div>
            <div className='flex flex-row mt-1'>
                <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                    <RxActivityLog/>
                </div>
                <div className='flex flex-row w-11/12'>
                    <span className='h-10 items-center flex  font-semibold'>Activity</span>  
                </div>
            </div>

            <div className="flex flex-row mt-2">
                <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                    <div className="flex-shrink-0 w-9 h-9 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                    {getInitialsFromFullName(userInfo.name)}
                    </div>
                </div>

                <div className='flex flex-row w-11/12'>
                    <div className='flex flex-col w-full'>
                    <div className='items-start flex'>
                        <TaskComment />
                    </div>
                    </div>
                </div>
            </div>


            <div className="flex flex-col my-3">
                {taskActivities
                    .sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate))
                    .slice(0, activityLimit) //shfaqi varesisht prej limitit aktual 
                    .map((activity, index) => (
                    <div className='flex flex-row my-1' key={index}>
                        <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                            <div className="flex-shrink-0 w-9 h-9 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                {getInitials(activity.userFirstName, activity.userLastName)}
                            </div>
                        </div>
                        <div className='flex flex-col w-11/12'>
                            <p className="text-sm"><b>{activity.userFirstName} {activity.userLastName}</b> {activity.actionType} {activity.entityName}</p>
                            <p className="text-[12px]">{formatDateTime(activity.actionDate)}</p>     
                        </div>
                    </div>
                ))}
            </div>
            {activityLimit < taskActivities.length && (
                <div className="flex justify-center my-3">
                    <button 
                        className="font-semibold p-2 h-10 rounded-[4px] text-sm bg-gray-600 bg-opacity-30"
                        onClick={loadMoreActivities}
                    >
                        Load More
                    </button>
                </div>
            )}
        </div> 
        );
  };
  
export default TaskActivityLog