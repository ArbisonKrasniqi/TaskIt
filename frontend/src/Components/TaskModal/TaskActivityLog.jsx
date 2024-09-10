import React, {useContext, useState, useEffect} from 'react';
import { RxActivityLog } from "react-icons/rx";
import TaskComment from "./TaskComment";
import { TaskModalsContext } from "./TaskModal";
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useParams } from 'react-router-dom';
import { getDataWithId } from '../../Services/FetchService';

const TaskActivityLog = () => {

    const {taskData} = useContext(TaskModalsContext);
    const {taskId} = useParams();
    const {getInitials} = useContext(WorkspaceContext);
    const [taskActivities, setTaskActivities] = useState([]);



    useEffect(() => {
        const getTaskActivities = async () => {
            try {
                if (taskId) {
                    const taskActivityResponse = await getDataWithId('http://localhost:5157/GetTaskActivityByTaskId?TaskId', taskId);
                    const taskActivityData = taskActivityResponse.data;
                    if (taskActivityData && Array.isArray(taskActivityData) && taskActivityData.length > 0) {
                        setTaskActivities(taskActivityData);
                        console.log("ACTIVITYYYYYY: ",taskActivityData);
                        
                    } else {
                        setTaskActivities([]);
                        console.log("There is no task activity");   
                    }
                }
            } catch (error) {
                console.error("There has been an error fetching taskId");
            }
        };
        getTaskActivities();
    },[taskActivities]);

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

    return (
        <div>
            <div className='flex flex-row mt-1'>
                <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                    <RxActivityLog/>
                </div>
                <div className='flex flex-row justify-between  w-11/12'>
                    <span className='h-10 items-center flex  font-semibold'>Activity</span>
                    <button className="font-semibold p-2 h-10 rounded-[4px] text-sm bg-gray-600 bg-opacity-30">Hide Details</button>
                    
                </div>
            </div>

            <div className="flex flex-row mt-2">
                <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                <button className='rounded-full mr-1'>
                                    <img
                                        src="https://via.placeholder.com/40"
                                        alt="Profile"
                                        className="w-8 h-8 rounded-full"/>
                                </button>
                </div>
                <div className='flex flex-row w-11/12'>
                <span className='h-10 items-center flex'>
                    <TaskComment/>
                </span>
                                
                </div>
            </div>

            <div className="flex flex-col my-3">
                {taskActivities
                .sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate)) // Sorting by actionDate
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
        </div> 
        );
  };
  
export default TaskActivityLog