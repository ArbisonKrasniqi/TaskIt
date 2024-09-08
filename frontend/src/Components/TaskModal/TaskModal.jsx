import React, { useState, createContext, useEffect, useContext} from 'react';
import { TbAlignBoxLeftTopFilled } from "react-icons/tb";
import { IoMdCheckboxOutline } from "react-icons/io";
import { LuClock4 } from "react-icons/lu";
import { CgTag } from "react-icons/cg";
import { MdOutlineSubject } from "react-icons/md";
import { PiPlus } from "react-icons/pi";
import { BsPerson } from "react-icons/bs";
import AutoResizingTextarea from './AutoResizingTextarea';
import TaskActivityLog from './TaskActivityLog'
import Checklist from './Checklist';
import MembersModal from './MembersModal';
import LabelsModal from './LabelsModal';
import ChecklistModal from './ChecklistModal';
import EditLabelModal from './EditLabelModal';
import { getDataWithId } from '../../Services/FetchService';
import { useParams } from 'react-router-dom';
import { WorkspaceContext } from '../Side/WorkspaceContext';


export const TaskModalsContext = createContext();

const TaskModal = () => {

    const {getInitials} = useContext(WorkspaceContext);

    const [isMembersModalOpen, setIsMembersModalOpen] = useState(false);
    const [isLabelModalOpen, setIsLabelModalOpen] = useState(false);
    const [isChecklistModalOpen, setIsChecklistModalOpen] = useState(false);
    const [isCreateLabelModalOpen, setIsCreateLabelModalOpen] = useState(false);
    const [isEditLabelModalOpen, setIsEditLabelModalOpen] = useState(false);
    const [selectedLabel, setSelectedLabel] = useState(null);
    const {taskId} = useParams();
    const [assignedLabels, setAssignedLabels] = useState([]);
    const [taskData, setTaskData] = useState([]);
    const [listData, setListData] = useState([]);
    const [assignedMembers, setAssignedMembers] = useState([]);

    useEffect(() => {   
        const getTaskById = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/task/GetTaskById?taskId',taskId);
                setTaskData(response.data);
            } catch (error) {
                console.error("Error fetching task by id: ",error);
            }
        };
        if (taskId) {
            getTaskById();
        }
    },[taskId,taskData]);

    useEffect(() => {
        const getListById = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/list/GetListById?listId',taskData.listId);
                setListData(response.data);
                
            } catch (error) {
                console.error("Error fetching list by id: ",error);
            }
        };
            getListById();
    },[taskId,taskData]);

    useEffect(() => {
        const fetchAssignedLabels = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/label/GetLabelsByTaskId?taskId',taskId);
                setAssignedLabels(response.data);
            } catch (error) {
                console.error("Error fetching assigned labels: ",error);
                
            }
        };
        if (taskId) {
            fetchAssignedLabels();
        }
    },[taskId,assignedLabels]);

    useEffect(() => {
        const fetchTaskMembers = async () => {
            try {
                const response = await getDataWithId(`http://localhost:5157/backend/TaskMembers/GetAllTaskMembersByTaskId?taskId`, 1);
                const data = response.data;

                const taskMembers = await Promise.all(data.map(async (taskMember) => {
                    const responseTaskMemberDetail = await getDataWithId('http://localhost:5157/backend/user/adminUserID?userId', taskMember.userId);
                    return responseTaskMemberDetail.data;
                }));

                setAssignedMembers(taskMembers);
            } catch (error) {
                console.error("Error fetching task members:", error);
            }
        };

        fetchTaskMembers();
    }, [taskId]);


    const toggleMembersModal = () => {
        setIsMembersModalOpen(!isMembersModalOpen);
    };

    const toggleLabelsModal = () => {
        if (!isLabelModalOpen) {
            setIsLabelModalOpen(true);
            setIsCreateLabelModalOpen(false);
            setIsEditLabelModalOpen(false);
        } else {
            setIsLabelModalOpen(false);
        }
    };

    const toggleChecklistModal = () => {
        setIsChecklistModalOpen(!isChecklistModalOpen);
    };

    const toggleEditLabelModal = (label) => {
        if (!isEditLabelModalOpen) {
            setSelectedLabel(label);
            setIsLabelModalOpen(false);
            setIsEditLabelModalOpen(true);
        } else {
            setIsEditLabelModalOpen(false);
        }
    }

    const values = {
        isMembersModalOpen,
        toggleMembersModal,
        isLabelModalOpen,
        toggleLabelsModal,
        isChecklistModalOpen,
        toggleChecklistModal,
        isCreateLabelModalOpen,
        isEditLabelModalOpen,
        toggleEditLabelModal,
        selectedLabel,
        setIsChecklistModalOpen,
        setIsLabelModalOpen,
        assignedLabels,
        setAssignedLabels,
        taskData,
        setTaskData,
        assignedMembers,
        setAssignedMembers
    };
    

    return (
        <TaskModalsContext.Provider value={values}>
            <div className="z-30 inset-0 flex justify-center transition-colors visible bg-black/20 h-screen">
                <div className="absolute top-12 flex flex-col bg-gray-800 rounded-md w-[750px]">
                    <div className="flex flex-row items-center justify-between mx-2 mt-3 text-gray-400 h-[60px]">
                        <TbAlignBoxLeftTopFilled className="bg-gray-800 text-2xl ml-5"/>
                        <div className="flex flex-col w-[550px] -ml-16">
                            <h2 className="w-[100%] font-bold text-xl">{taskData.title}</h2>
                            <div className='flex '>
                                <span className="text-xs"> in list {listData.title}</span>
                            </div>
                        </div>
                        <button className="mx-2 hover:bg-gray-600 hover:rounded-full transition w-6 rounded-full flex justify-center">X</button>
                    </div>

                    <div className="flex text-gray-400">
                        <div className="w-3/4 flex flex-col flex-wrap">
                            <div className='flex'>
                                <div className='flex flex-col flex-wrap ml-[70px]'>
                                    <p className='w-[82px] text-xs font-semibold'>Members</p>
                                    <div className='flex flex-wrap h-auto items-center justify-start'>
                                        {assignedMembers.length > 0 ? (
                                            assignedMembers.map((member) => (
                                                <button className='rounded-full m-1'>
                                                    <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                                        {getInitials(member.firstName, member.lastName)}
                                                    </div>
                                                </button>
                                            ))
                                        ) : (
                                            <span></span>
                                        )}
                                        <button onClick={toggleMembersModal} className='w-10 h-10 rounded-full flex justify-center items-center m-1 bg-gray-700 bg-opacity-50 hover:bg-gray-700 text-xl'>
                                            <PiPlus/>
                                        </button>
                                    </div>
                                </div>

                                <div className='flex flex-col flex-wrap ml-3'>
                                    <p className='w-[100px] text-xs font-semibold'>Labels</p>
                                    <div className='flex flex-wrap h-auto items-center justify-start'>
                                        {assignedLabels.length > 0 ? (
                                            assignedLabels.map((label) => (
                                                <button
                                                    key={label.id}
                                                    className={`rounded-[4px] m-1  px-2 py-2 text-gray-100 font-semibold `}
                                                    style={{ backgroundColor: label.color }}
                                                >
                                                    {label.name}
                                                </button>
                                            ))
                                        ) : (
                                            <span className="text-gray-500 text-xs"></span>
                                        )}
                                        <button
                                            className='w-8 h-8 rounded flex justify-center items-center m-1 bg-gray-700 bg-opacity-50 hover:bg-gray-700 text-xl text-gray-400'
                                            onClick={toggleLabelsModal}>
                                            <PiPlus />
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <div className='flex flex-row'>
                                <div className='w-1/12 h-6 justify-center flex items-center mt-3 ml-4 mr-3 text-2xl '>
                                    <MdOutlineSubject/>
                                </div>
                                <div className='flex flex-col w-11/12'>
                                    <span className='h-10 items-center flex font-semibold'>Description</span>
                                    <AutoResizingTextarea
                                    taskDescription={taskData.description}
                                    />
                                </div>
                            </div>
                            <div>
                                <Checklist/>
                            </div>
                            <div>
                                <TaskActivityLog/>
                            </div>
                        </div>

                        {/*djathtas*/}
                        <div className="w-1/4">
                            <div className="flex flex-col">
                                <span className="text-xs mx-3">Add to cart</span>
                                <div className="flex flex-col text-sm font-bold m-2 p-1">
                                    <button
                                        className="flex w-full justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                                        onClick={toggleMembersModal}>
                                        <BsPerson className="mx-2 text-lg" /> Members
                                    </button>
                                    
                                    <button className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                                        onClick={toggleLabelsModal}>
                                        <CgTag className="mx-2 rotate-[135deg] text-lg"/> Labels
                                    </button>
                                    <button className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                                        onClick={toggleChecklistModal}>
                                        <IoMdCheckboxOutline className="mx-2 text-lg"/> Checklist
                                    </button>
                                    <button className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80">
                                        <LuClock4 className="mx-2 text-lg"/> Dates
                                    </button>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                    {isMembersModalOpen && <MembersModal />}
                    {isLabelModalOpen && <LabelsModal/>}
                    {isChecklistModalOpen && <ChecklistModal/>}
                    {isEditLabelModalOpen && <EditLabelModal/>}
                    
                </div>
            </div>
        </TaskModalsContext.Provider>
    );
}

export default TaskModal;
