import React from 'react';
import { deleteData } from '../../Services/FetchService';

const ChecklistItemDeleteModal = ({ checklistItemId, closeModal }) => {

  const handleDelete = async () => {
    try {
        await deleteData('http://localhost:5157/backend/checklistItems/DeleteChecklistItem',{checklistItemId});
    } catch (error) {
        console.error("Error deleting checklist item:", error.message);
        if (error.response) {
            console.error("Response status:", error.response.status);
            console.error("Response data:", error.response.data);
            console.error("Response headers:", error.response.headers);
        }
    }
  };

  return (
    <div className="absolute right-[-110px] z-10 rounded top-[45px] bg-gray-900 py-1">
      <div className='flex w-[150px] justify-around text-sm text-gray-500'>
        <p>Item actions</p>
        <button 
          className='text-end mr-2' 
          onClick={closeModal}
        >
          X
        </button>
      </div>
      <button
        className="bg-gray-900 text-[17px] bg-opacity-60 w-[100%] text-start pl-2 mt-2 focus:outline-none hover:bg-gray-700 items-center"
        onClick={handleDelete}
      >
        Delete
      </button>
    </div>
  );
};

export default ChecklistItemDeleteModal;
