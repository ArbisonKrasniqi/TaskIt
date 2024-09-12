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
import { getDataWithId, putData } from '../../Services/FetchService';
import { useNavigate, useParams } from 'react-router-dom';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import DateCalendarModal from './DateCalendarModal';
import { BoardContext } from '../BoardContent/Board';


export const TaskModalsContext = createContext();

const TaskModal = () => {
  const boardContext = useContext(BoardContext);
  const navigate = useNavigate();

  const { getInitials, board } = useContext(WorkspaceContext);
  const [isMembersModalOpen, setIsMembersModalOpen] = useState(false);
  const [isLabelModalOpen, setIsLabelModalOpen] = useState(false);
  const [isChecklistModalOpen, setIsChecklistModalOpen] = useState(false);
  const [isCreateLabelModalOpen, setIsCreateLabelModalOpen] = useState(false);
  const [isEditLabelModalOpen, setIsEditLabelModalOpen] = useState(false);
  const [selectedLabel, setSelectedLabel] = useState(null);
  const { taskId, workspaceId, boardId } = useParams();
  const [assignedLabels, setAssignedLabels] = useState([]);
  const [taskData, setTaskData] = useState({});
  const [listData, setListData] = useState({});
  const [assignedMembers, setAssignedMembers] = useState([]);
  const [isCalendarOpen, setIsCalendarOpen] = useState(false);
  const [isEditingTitle, setIsEditingTitle] = useState(false);

  const specificDate = new Date('0001-01-01T00:00:00Z');

  const openCalendar = () => {
    setIsCalendarOpen(true);
  };

  const closeCalendar = () => {
      setIsCalendarOpen(!isCalendarOpen);
  };

  const getTaskById = async () => {
    try {
        const response = await getDataWithId('http://localhost:5157/backend/task/GetTaskById?taskId',taskId);
        setTaskData(response.data);
    } catch (error) {
        console.error("Error fetching task by id: ",error);
    }
};
    useEffect(() => {   
   
        if (taskId) {
            getTaskById();
        }
    },[taskId,board]);

    useEffect(() => {
        const getListById = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/list/GetListById?listId',taskData.listId);
                setListData(response.data);
                
            } catch (error) {
                console.error("Error fetching list by id: ",error);
            }
        };
            //getListById();
    },[taskId,board]);

    const fetchAssignedLabels = async () => {
        try {
            const response = await getDataWithId('http://localhost:5157/backend/label/GetLabelsByTaskId?taskId',taskId);
            setAssignedLabels(response.data);
        } catch (error) {
            console.error("Error fetching assigned labels: ",error);
            
        }
    };
    useEffect(() => {
        if (taskId) {
            fetchAssignedLabels();
        }
    },[taskId]);

    useEffect(() => {
        const fetchTaskMembers = async () => {
            try {
                const response = await getDataWithId(`http://localhost:5157/backend/TaskMembers/GetAllTaskMembersByTaskId?taskId`, taskId);
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
            
  const handleTitleChange = (e) => {
    setNewTitle(e.target.value);
};

const handleSaveTitle = async () => {
    try {
        if (newTitle.trim()) {
            const data = {
                taskId: taskId,
                title: newTitle,
                description: taskData.description,
                dateAdded: taskData.dateAdded,
                dueDate: taskData.dueDate
            };
            await putData('http://localhost:5157/backend/task/UpdateTask', data);
            setTaskData((prevData) => ({
                ...prevData,
                title: newTitle,
            }));
            getTaskActivities();
            setIsEditingTitle(false);
        }
    } catch (error) {
        console.error('Error updating task title: ', error);
    }
};

const handleCancelEdit = () => {
    setNewTitle(taskData.title); // Revert to the original title
    setIsEditingTitle(false);
};

const [taskActivities, setTaskActivities] = useState([]);


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

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        const formattedDate = date.toLocaleDateString('en-US');
        return `${formattedDate}`;
    };


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
  };


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
        setAssignedMembers,
        closeCalendar,
        getTaskById,
        taskActivities,
        getTaskActivities,
        fetchAssignedLabels,
    };
    

    return (
        <TaskModalsContext.Provider value={values}>
          <div className="fixed z-30 inset-0 flex justify-center transition-colors visible bg-black bg-opacity-55">
            <div
              className="absolute top-12 flex flex-col bg-gray-800 rounded-md w-full max-w-[750px] h-[90%] overflow-y-auto"
              style={{ scrollbarWidth: 'thin', msOverflowStyle: 'none' }}
            >
                <div className="flex flex-row items-start justify-between mx-2 mt-3 text-gray-400">
                    <div className="flex items-center justify-around space-x-3">
                        <TbAlignBoxLeftTopFilled className="bg-gray-800 text-2xl ml-5" />
                        <div className="flex flex-col w-full md:w-[550px]">
                        <h2 className="font-bold text-xl">
                        {isEditingTitle ? (
                                        <div className="flex items-center space-x-2">
                                            <input
                                                type="text"
                                                value={newTitle}
                                                onChange={handleTitleChange}
                                                className="border rounded p-1 bg-gray-800"
                                                autoFocus
                                            />
                                            <button
                                                className="bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded px-3 py-1"
                                                onClick={handleSaveTitle}
                                            >
                                                Save
                                            </button>
                                            <button
                                                className="bg-gray-800 hover:bg-slate-700 text-gray-400 font-semibold rounded px-3 py-1"
                                                onClick={handleCancelEdit}
                                            >
                                                Cancel
                                            </button>
                                        </div>
                                    ) : (
                                        <div className="flex items-center space-x-2">
                                            <span className='cursor-pointer'
                                             onClick={() => {setIsEditingTitle(true)
                                              setNewTitle(taskData.title);
                                             }}>{taskData.title}</span>
                                            
                                        </div>
                                    )}
                        </h2>
                        <div className="flex text-xs">
                            <span> in list {listData.title}</span>
                            <span className="ml-[100px]  text-[12px]">
                            Due Date:{' '}
                            <span className="text-red-400 text-opacity-75">
                                {formatDateTime(taskData.dueDate) === formatDateTime(specificDate)
                                ? ''
                                : formatDateTime(taskData.dueDate)}
                            </span>
                            </span>
                        </div>
                        </div>
                    </div>
                    <button onClick={() => {navigate(`/main/board/${workspaceId}/${boardId}`)}} className="mt-2 md:mt-0 mx-2 hover:bg-gray-600 hover:rounded-full transition w-6 h-6 rounded-full flex items-center justify-center">
                        X
                    </button>
                </div>

      
              <div className="flex flex-col sm:flex-col md:flex-row text-gray-400">
                <div className="w-full md:w-3/4 flex flex-col flex-wrap">
                  <div className="flex">
                    {/* Members */}
                    <div className="flex flex-col flex-wrap ml-[70px]">
                      <p className="w-[82px] text-xs font-semibold">Members</p>
                      <div className="flex flex-wrap h-auto items-center justify-start">
                        {assignedMembers.length > 0 ? (
                          assignedMembers.map((member) => (
                            <button className="rounded-full m-1">
                              <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                {getInitials(member.firstName, member.lastName)}
                              </div>
                            </button>
                          ))
                        ) : (
                          <span></span>
                        )}
                        <button
                          onClick={toggleMembersModal}
                          className="w-10 h-10 rounded-full flex justify-center items-center m-1 bg-gray-700 bg-opacity-50 hover:bg-gray-700 text-xl"
                        >
                          <PiPlus />
                        </button>
                      </div>
                    </div>
      
                    {/* Labels */}
                    <div className="flex flex-col flex-wrap ml-3">
                      <p className="w-[100px] text-xs font-semibold">Labels</p>
                      <div className="flex flex-wrap h-auto items-center justify-start">
                        {taskData?.labels?.length > 0 ? (
                          taskData.labels.map((label) => (
                            <button
                              key={label.labelId}
                              className="rounded-[4px] m-1 px-2 py-2 text-gray-100 font-semibold"
                              style={{ backgroundColor: label.color }}
                            >
                              {label.name}
                            </button>
                          ))
                        ) : (
                          <span className="text-gray-500 text-xs">
                            No labels assigned
                          </span>
                        )}
      
                        <button
                          className="w-8 h-8 rounded flex justify-center items-center m-1 bg-gray-700 bg-opacity-50 hover:bg-gray-700 text-xl text-gray-400"
                          onClick={toggleLabelsModal}
                        >
                          <PiPlus />
                        </button>
                      </div>
                    </div>
                  </div>
      
                  {/* Description */}
                  <div className="flex flex-row">
                    <div className="w-1/12 h-6 justify-center flex items-center mt-3 ml-4 mr-3 text-2xl ">
                      <MdOutlineSubject />
                    </div>
                    <div className="flex flex-col w-11/12">
                      <span className="h-10 items-center flex font-semibold">
                        Description
                      </span>
                      <AutoResizingTextarea taskDescription={taskData.description} />
                    </div>
                  </div>
      
                  {/* Checklist */}
                  <div>
                    <Checklist />
                  </div>
      
                  {/* Task Activity Log */}
                  <div>
                    <TaskActivityLog />
                  </div>
                </div>
      
                {/*djathas*/}
                <div className="w-full md:w-1/4 sm:w-full">
                  <div className="flex flex-col">
                    <span className="text-xs mx-3">Add to card</span>
                    <div className="flex flex-col text-sm font-bold m-2 p-1">
                      <button
                        className="flex w-full justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                        onClick={toggleMembersModal}
                      >
                        <BsPerson className="mx-2 text-lg" /> Members
                      </button>
      
                      <button
                        className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                        onClick={toggleLabelsModal}
                      >
                        <CgTag className="mx-2 rotate-[135deg] text-lg" /> Labels
                      </button>
      
                      <button
                        className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                        onClick={toggleChecklistModal}
                      >
                        <IoMdCheckboxOutline className="mx-2 text-lg" /> Checklist
                      </button>
      
                      <button
                        onClick={openCalendar}
                        className="flex justify-start items-center bg-gray-600 bg-opacity-30 rounded-sm h-8 my-1 hover:bg-opacity-80"
                      >
                        <LuClock4 className="mx-2 text-lg" /> Dates
                      </button>
                    </div>
                  </div>
                </div>
              </div>
      
              {isMembersModalOpen && <MembersModal />}
              {isLabelModalOpen && <LabelsModal />}
              {isChecklistModalOpen && <ChecklistModal />}
              {isEditLabelModalOpen && <EditLabelModal />}
              {isCalendarOpen && <DateCalendarModal />}
            </div>
          </div>
        </TaskModalsContext.Provider>
      );
      
}

export default TaskModal;