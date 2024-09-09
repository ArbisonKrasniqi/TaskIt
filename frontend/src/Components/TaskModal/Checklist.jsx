import React, { useState, useContext, useRef } from 'react';
import { IoMdCheckboxOutline } from 'react-icons/io';
import { FaEllipsisH } from 'react-icons/fa';
import { deleteData, postData, putData, getDataWithId } from '../../Services/FetchService.jsx';
import ChecklistItemDeleteModal from './ChecklistItemDeleteModal.jsx';
import { TaskModalsContext } from './TaskModal.jsx';
import { useParams } from 'react-router-dom';
import {WorkspaceContext} from '../Side/WorkspaceContext';


const Checklist = () => {
  const { checklists, checklistItems, setChecklistItems, setChecklists, fetchChecklistItems } = useContext(WorkspaceContext);
  const {setIsChecklistModalOpen} = useContext(TaskModalsContext);
  const [checklistItemDotsOpen, setChecklistItemDotsOpen] = useState(null);
  const [addingItem, setAddingItem] = useState(null);
  const [newItemContent, setNewItemContent] = useState('');
  const [editingItemId, setEditingItemId] = useState(null);
  const [editedContent, setEditedContent] = useState('');

  const [editingChecklistId, setEditingChecklistId] = useState(null);
  const [editedChecklistTitle, setEditedChecklistTitle] = useState('');
  const {taskId} = useParams();

  
  // Reference to the input element for focusing
  const inputRef = useRef(null);
  const handleChange = async (checklistId, checklistItemId) => {
    setChecklistItems(prevItems => {
      const updatedItems = {...prevItems};
      const checklistItems = updatedItems[checklistId].map(checklistItem => {
        if (checklistItem.checklistItemId === checklistItemId) {
          return {...checklistItem, checked: !checklistItem.checked};
        }
        return checklistItem;
      });
      updatedItems[checklistId] = checklistItems;
      return updatedItems;
    });

    try {
      
      await putData(`http://localhost:5157/backend/checklistItems/ChangeChecklistItemChecked?checklistItemId=${checklistItemId}`,{});
    } catch (error) {
      console.error("Error fetching checklists:", error.message);
    }
  };

  const toggleChecklistItemDots = (checklistItemId) => {
    setChecklistItemDotsOpen(prevId => (prevId === checklistItemId ? null : checklistItemId));
    setEditingItemId(null);
    setAddingItem(null);
    setEditingChecklistId(null);
  };

  const closeModal = () => {
    setChecklistItemDotsOpen(null);
  };

  const handleChecklistDelete = async (checklistId) => {
    try {
      const items = await getDataWithId('http://localhost:5157/backend/checklistItems/GetChecklistItemByChecklistId?checklistId', checklistId);;

      if (items.length > 0) {
        await deleteData('http://localhost:5157/backend/checklistItems/DeleteChecklistItembyChecklistId', { checklistId }); 
      }
      await deleteData('http://localhost:5157/backend/checklist/DeleteChecklist', { checklistId });

      const updatedChecklists = checklists.filter(
        (checklist) => checklist.checklistId !== checklistId
      );
      setChecklists(updatedChecklists);
    } catch (error) {
      console.error("Error deleting checklist: ", error.message);
    }
  };

  const handleAddClick = (checklistId) => {
    if (addingItem !== checklistId) {
      setEditingItemId(null);
      setEditingChecklistId(null);
      setChecklistItemDotsOpen(null);
      setIsChecklistModalOpen(false);
    }
    setAddingItem(checklistId);
  };

  const handleAddItem = async (checklistId) => {
    try {
      if (newItemContent.length < 2 || newItemContent.length > 280) {
        console.log("Item content should be between 2 and 280 characters!");
        return;
      }

      const data = {
        content: newItemContent,
        checklistId: checklistId
      };

      const response = await postData(`http://localhost:5157/backend/checklistItems/CreateChecklistItem`, data);

      setChecklistItems(prevItems => {
        const updatedItems = { ...prevItems };
        updatedItems[checklistId] = [...updatedItems[checklistId], response.data];
        return updatedItems;
      });

      setNewItemContent('');
      setAddingItem(null);
    } catch (error) {
      console.error("Error adding checklist item:", error.message);
    }
  };

  const handleCancelAddItem = () => {
    setAddingItem(null);
    setNewItemContent('');
  };

  // Handle the click on a checklist item to start editing
  const handleItemClick = (checklistItemId, currentContent) => {
    setEditingItemId(checklistItemId);
    setEditedContent(currentContent);
    
  };

  
  
  return (
    <div>
      {checklists.length > 0 && checklists.map(checklist => (
        <div key={checklist.checklistId} className="my-4">
          <div className="flex flex-row">
            <div className="w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl">
              <IoMdCheckboxOutline />
            </div>
            <div className="flex flex-row justify-between w-11/12">
              <span className="h-10 items-center flex font-semibold">{checklist.title}</span>
              <button className="font-semibold p-2 h-10 rounded-[4px] text-sm bg-gray-600 bg-opacity-30 w-[70px]">
                Delete
              </button>
            </div>
          </div>

          {checklistItems[checklist.checklistId] && checklistItems[checklist.checklistId].map(checklistItem => (
            <div key={checklistItem.checklistItemId} className="flex flex-row items-center">
              <label className="w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3">
                <input
                  type="checkbox"
                  checked={checklistItem.checked}
                  onChange={() => handleChange(checklist.checklistId, checklistItem.checklistItemId)}
                  className="hidden"
                />
                <div className={`w-4 h-4 rounded cursor-pointer border-2 flex items-center justify-center transition-colors ${
                  checklistItem.checked ? 'bg-blue-200 border-blue-200 text-gray-800' : 'bg-gray-800 border-gray-700'
                }`}>
                  {checklistItem.checked && (
                    <svg
                      className="w-4 h-4"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth="4"
                        d="M5 13l4 4L19 7"
                      />
                    </svg>
                  )}
                </div>
              </label>
              <div className="flex items-center w-11/12">
                <span className="text-sm flex my-2 items-center justify-between w-[100%] hover:bg-gray-700 rounded-xl p-2">
                  {checklistItem.content}
                  <button className="w-[5%] h-6 rounded-full flex justify-center items-center text-sm bg-gray-600 bg-opacity-75 outline-white">
                    <FaEllipsisH />
                  </button>
                </span>
              </div>
            </div>
          ))}
          <p className='ml-20'><button className='bg-gray-600 bg-opacity-30 text-sm rounded  px-2 py-1 items-center justify-center'>Add an item</button></p>
        </div>
      ))}
      
    </div>
  );
};

export default Checklist;
