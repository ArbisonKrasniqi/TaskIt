import React, { useRef, useEffect, useContext, useState } from 'react';
import { TaskModalsContext } from './TaskModal';
import { FaAngleLeft } from "react-icons/fa6";
import { deleteData, putData } from '../../Services/FetchService';
import { useParams } from 'react-router-dom';


function EditLabelModal() {
    const { toggleEditLabelModal, toggleLabelsModal, selectedLabel, setIsLabelModalOpen, setAssignedLabels } = useContext(TaskModalsContext);
    const inputRef = useRef(null);
    const {taskId} = useParams();
    const [labelName, setLabelName] = useState(selectedLabel ? selectedLabel.name : '');

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
        }
    }, []);

    const handleSave = async () => {
        try {
            if (labelName) {
                const updatedLabel = {
                    labelId: selectedLabel.labelId,
                    name: labelName
                };

                await putData('http://localhost:5157/backend/label/UpdateLabel', updatedLabel);
            } else {
                console.log("Name should be between 2 and 280 characters");
                
            }
            toggleEditLabelModal();
            setIsLabelModalOpen(true);
            
        } catch (error) {
            console.error("Error updating label:", error);
        }
    };

    const handleDelete = async () => {
        try {
            if (labelName) {
                const updateLabel = {
                    labelId: selectedLabel.labelId,
                    name: ''  
                };

                await putData('http://localhost:5157/backend/label/UpdateLabel', updateLabel);

                const labelData = {
                    labelId: selectedLabel.labelId,
                    taskId: taskId
                }
                await deleteData('http://localhost:5157/backend/taskLabel/RemoveLabelFromTask',labelData);
                setAssignedLabels(prev => prev.filter(id => id !== selectedLabel.labelId));
            }
            toggleEditLabelModal();
            setIsLabelModalOpen(true);
        } catch (error) {
            
        }
    };

    return (
        <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
          <div className="bg-gray-900 w-[300px] max-w-md p-5 rounded-md shadow-lg overflow-y-auto" style={{ maxHeight: '80vh' }}>
            <div className="flex justify-between items-center mb-4">
              <button
                className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                onClick={toggleLabelsModal}
              >
                <FaAngleLeft />
              </button>
              <h2 className="text-sm font-semibold text-gray-400">Edit Label</h2>
              <button
                className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                onClick={toggleEditLabelModal}
              >
                X
              </button>
            </div>
    
            <h3 className='text-gray-400 text-sm font-semibold mb-2'>Title</h3>
            <input
              type="text"
              ref={inputRef}
              value={labelName}
              onChange={(e) => setLabelName(e.target.value)}
              className="w-full pl-2 py-1 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
            />
    
            <h3 className='text-gray-400 text-sm font-semibold mb-2'>Color</h3>
            <div
              className="w-full h-8 mb-4 rounded-sm"
              style={{ backgroundColor: selectedLabel.color }}
            ></div>
    
            <div className='flex justify-between'>
              <button
                onClick={handleSave}
                className='w-[120px] h-8 rounded-sm bg-blue-600 text-gray-900 font-semibold hover:bg-blue-700'
              >
                Save
              </button>
              <button
                className='w-[120px] h-8 rounded-sm bg-red-600 text-gray-900 font-semibold hover:bg-red-700'
                onClick={handleDelete}
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      );
}

export default EditLabelModal;
