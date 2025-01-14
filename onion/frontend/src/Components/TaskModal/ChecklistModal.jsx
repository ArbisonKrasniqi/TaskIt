import React, { useState, useContext, useEffect, useRef} from 'react';
import { TaskModalsContext } from './TaskModal';
import { postData } from '../../Services/FetchService';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useParams } from 'react-router-dom';

function ChecklistModal() {
  const { toggleChecklistModal, setIsChecklistModalOpen, getTaskActivities } = useContext(TaskModalsContext);
  const { setChecklists, getChecklistsByTask } = useContext(WorkspaceContext);
  const { getTaskById} = useContext(TaskModalsContext);
  const [checklistTitle, setCheklistTitle] = useState('');
  const {taskId} = useParams();
  
  
  // Ref to the checklist button to position the modal below it
  const checklistButtonRef = useRef(null);
  const [modalPosition, setModalPosition] = useState({ top: 220, right: 0 });

  useEffect(() => {
    if (checklistButtonRef.current) {
      const rect = checklistButtonRef.current.getBoundingClientRect();
      setModalPosition({ top: rect.bottom, right: rect.right });
    }
  }, [checklistButtonRef]);

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

      await postData('http://localhost:5127/backend/checklist/CreateChecklist', data);

      getTaskActivities();
      getChecklistsByTask();
    } catch (error) {
      console.error('Error creating checklist');
    }
  };

  const handleAddClick = () => {
    handleAddChecklist();
    setIsChecklistModalOpen(false);
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
      <div className="bg-gray-900 w-full max-w-xs p-5 rounded-md shadow-lg overflow-y-auto" style={{ maxHeight: '80vh' }}>
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-sm font-semibold text-gray-400">Add Checklist</h2>
          <button
            onClick={toggleChecklistModal}
            className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
          >
            X
          </button>
        </div>

        <h3 className="text-gray-400 text-sm font-semibold mb-2">Title</h3>
        <input
          value={checklistTitle}
          onChange={(e) => setCheklistTitle(e.target.value)}
          type="text"
          placeholder="Checklist"
          className="w-full pl-2 py-1 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
        />

        <button
          className="w-full h-8 rounded-sm bg-blue-400 text-gray-800 font-semibold hover:bg-blue-500"
          onClick={handleAddClick}
        >
          Add
        </button>
      </div>
    </div>
  );
}

export default ChecklistModal;
