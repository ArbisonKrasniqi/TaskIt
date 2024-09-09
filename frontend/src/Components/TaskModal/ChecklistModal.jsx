import React, { useState, useContext, useEffect, useRef} from 'react';
import { TaskModalsContext } from './TaskModal';
import { postData } from '../../Services/FetchService';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useParams } from 'react-router-dom';

function ChecklistModal() {
  const { toggleChecklistModal, setIsChecklistModalOpen } = useContext(TaskModalsContext);
  const { setChecklists, getChecklistsByTask } = useContext(WorkspaceContext);
  const [checklistTitle, setCheklistTitle] = useState('');
  const {taskId} = useParams();
  
  
  // Ref to the checklist button to position the modal below it
  const checklistButtonRef = useRef(null);
  const [modalPosition, setModalPosition] = useState({ top: 220, right: -124 });
  
  const handleAddChecklist = async () => {
    try {
      if (checklistTitle.length < 2 || checklistTitle.length > 280) {
        console.log('Checklist title should be between 2 and 280 characters');
        return;
      }

      const data = {
        title: checklistTitle,
        taskId: taskId,
      };

      const response = await postData('http://localhost:5157/backend/checklist/CreateChecklist', data);

      getChecklistsByTask();
    } catch (error) {
      console.error('Error creating checklist: ', error.message);
    }
  };

  const handleAddClick = () => {
    handleAddChecklist();
    setIsChecklistModalOpen(false);
  };

  return(
    <>
    <div
    className="absolute z-50 bg-black bg-opacity-50 rounded-md"
    style={{
      top: `${modalPosition.top}px`,
      right: `${modalPosition.right}px`,
      width: '300px',
    }}
  >
    <div className="bg-gray-900 p-5 rounded-md shadow-lg">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-sm font-semibold text-gray-400">Add checklist</h2>
        <button
          onClick={toggleChecklistModal}
          className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
        >
          X
        </button>
              <div>
              <h3 className='text-gray-400 text-sm font-semibold'>Title</h3>
              <input
                  type="text"
                  placeholder="Checklist"
                  className="w-full pl-[5px] py-1 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
              />
              <button className='w-16 h-8 rounded-sm bg-blue-400 bg-opacity-90 text-gray-800 font-semibold hover:bg-opacity-80'>Add</button>
              </div>
          </div>
      </div>
      </div>
      </>
  );
};

export default ChecklistModal;