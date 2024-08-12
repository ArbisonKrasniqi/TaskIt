import { RxActivityLog } from "react-icons/rx";
import TaskComment from "./TaskComment";

const TaskActivityLog = () => {
    return (
        <div>
            <div className='flex flex-row'>
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

            <div className="flex flex-row my-2">
                <div className='w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl '>
                <button className='rounded-full mr-1'>
                                    <img
                                        src="https://via.placeholder.com/40"
                                        alt="Profile"
                                        className="w-8 h-8 rounded-full"/>
                                </button>
                </div>
                <div className='flex flex-col w-11/12'>
                <p className="text-sm"><b>Endrit Musaj</b> completed blablabla...</p>
                <p className="text-[12px]">Aug 2, 2024, 1:14 PM</p>
                                
                </div>
            </div>
        </div> 
        );
  };
  
export default TaskActivityLog