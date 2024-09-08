import React, { useState, createContext, useEffect, useContext} from 'react';
import { TbAlignBoxLeftTopFilled } from "react-icons/tb";
import { IoMdCheckboxOutline } from "react-icons/io";
import { LuClock4 } from "react-icons/lu";
import { CgTag } from "react-icons/cg";
import { MdOutlineSubject } from "react-icons/md";
import { PiPlus } from "react-icons/pi";
import { MdOutlineRemoveRedEye } from "react-icons/md";
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


export const TaskModalsContext = createContext();

const TaskModal = () => {

    const [isMembersModalOpen, setIsMembersModalOpen] = useState(false);
    const [isLabelModalOpen, setIsLabelModalOpen] = useState(false);
    const [isChecklistModalOpen, setIsChecklistModalOpen] = useState(false);
    const [isCreateLabelModalOpen, setIsCreateLabelModalOpen] = useState(false);
    const [isEditLabelModalOpen, setIsEditLabelModalOpen] = useState(false);
    const [selectedLabel, setSelectedLabel] = useState(null);
    const {taskId} = useParams();
    const [assignedLabels, setAssignedLabels] = useState([]);

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
        setAssignedLabels
    };
    

    return (
        <TaskModalsContext.Provider value={values}>
            <div className="z-30 inset-0 flex justify-center transition-colors visible bg-black/20 h-screen">
                <div className="absolute top-12 flex flex-col bg-gray-800 rounded-md w-[750px]">
                    <div className="flex flex-row items-center justify-between mx-2 mt-3 text-gray-400 h-[60px]">
                        <TbAlignBoxLeftTopFilled className="bg-gray-800 text-2xl ml-5"/>
                        <div className="flex flex-col w-[550px] -ml-16">
                            <h2 className="w-[100%] font-bold text-xl">Task Name</h2>
                            <div className='flex '>
                                <span className="text-xs"> in list LISTNAME</span>
                                <span className='ml-2'><MdOutlineRemoveRedEye/></span>
                            </div>
                        </div>
                        <button className="mx-2 hover:bg-gray-600 hover:rounded-full transition w-6 rounded-full flex justify-center">X</button>
                    </div>

                    <div className="flex text-gray-400">
                        <div className="w-3/4 flex flex-col flex-wrap">
                            <div className='flex'>
                                <div className='flex flex-col ml-[70px]'>
                                    <p className='w-[82px] text-xs font-semibold'>Members</p>
                                    <div className='flex h-12 items-center justify-around'>
                                        <button className='rounded-full mr-1'>
                                            <img
                                                src="https://via.placeholder.com/40"
                                                alt="Profile"
                                                className="w-8 h-8 rounded-full"/>
                                        </button>
                                        <button onClick={toggleMembersModal} className='w-8 h-8 rounded-full flex justify-center items-center mr-1 bg-gray-700 bg-opacity-50 hover:bg-gray-700 text-xl'>
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
                                    <AutoResizingTextarea/>
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
