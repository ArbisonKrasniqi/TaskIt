import React, { useState, useContext, useRef } from 'react';
import { IoMdCheckboxOutline } from 'react-icons/io';
import { FaEllipsisH } from 'react-icons/fa';
import { WorkspaceContext } from '../Side/WorkspaceContext.jsx';
import { deleteData, postData, putData, getDataWithId } from '../../Services/FetchService.jsx';
import ChecklistItemDeleteModal from './ChecklistItemDeleteModal.jsx';
import { TaskModalsContext } from './TaskModal.jsx';
import { useParams } from 'react-router-dom';


const Checklist = () => {
  const { checklists, checklistItems, setChecklistItems, setChecklists, getChecklistsByTask } = useContext(WorkspaceContext);
  const {setIsChecklistModalOpen, getTaskActivities} = useContext(TaskModalsContext);
  const [checklistItemDotsOpen, setChecklistItemDotsOpen] = useState(null);
  const [addingItem, setAddingItem] = useState(null);
  const [newItemContent, setNewItemContent] = useState('');
  const [editingItemId, setEditingItemId] = useState(null);
  const [editedContent, setEditedContent] = useState('');

  const [editingChecklistId, setEditingChecklistId] = useState(null);
  const [editedChecklistTitle, setEditedChecklistTitle] = useState('');

  
  // Reference to the input element for focusing
  const inputRef = useRef(null);

  const handleChange = async (checklistId, checklistItemId) => {
    setChecklistItems(prevItems => {
      const updatedItems = { ...prevItems };
      const checklistItems = updatedItems[checklistId].map(checklistItem => {
        if (checklistItem.checklistItemId === checklistItemId) {
          return { ...checklistItem, checked: !checklistItem.checked };
        }
        return checklistItem;
      });
      updatedItems[checklistId] = checklistItems;
      return updatedItems;
    });

    try {
      await putData(`http://localhost:5157/backend/checklistItems/ChangeChecklistItemChecked?checklistItemId=${checklistItemId}`, {});
      getTaskActivities();
      getChecklistsByTask();
      setAddingItem(null);
      setEditingItemId(null);
      setEditingChecklistId(null);
      setChecklistItemDotsOpen(null);
      setIsChecklistModalOpen(false);
    } catch (error) {
      console.log("Error updating checklist item:", error.message);
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
        getTaskActivities();
        getChecklistsByTask();
      }
      await deleteData('http://localhost:5157/backend/checklist/DeleteChecklist', { checklistId });
      getTaskActivities();
      getChecklistsByTask();

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

      getTaskActivities();
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
    
    // Focus the input after setting the editing item
    setTimeout(() => {
      if (inputRef.current) {
        setAddingItem(null);
        setChecklistItemDotsOpen(null);
        setEditingChecklistId(null);
        inputRef.current.focus();
      }
    }, 0);
  };

  const handleSaveEdit = async (checklistId, checklistItem) => {
    try {
      if (editedContent.length < 2 || editedContent.length > 280) {
        console.log("Item content should be between 2 and 280 characters!");
        return;
      }

      const data = {
        checklistItemId: checklistItem.checklistItemId,
        content: editedContent,
        checked: checklistItem.checked,
        checklistId: checklistId
      };

      await putData('http://localhost:5157/backend/checklistItems/UpdateChecklistItem', data);

      setChecklistItems(prevItems => {
        const updatedItems = { ...prevItems };
        updatedItems[checklistId] = updatedItems[checklistId].map(item =>
          item.checklistItemId === checklistItem.checklistItemId ? { ...item, content: editedContent } : item
        );
        return updatedItems;
      });

      getTaskActivities();
      setEditingItemId(null);
      setEditedContent('');
    } catch (error) {
      console.error('Error updating checklist item:', error.message);
    }
  };

  const handleCancelEdit = () => {
    setEditingItemId(null);
    setEditedContent('');
  };

  const handleTitleEdit = (checklistId, currentTitle) => {
    setEditingChecklistId(checklistId);
    setEditedChecklistTitle(currentTitle);
    setAddingItem(null);
    setEditingItemId(null);
    setChecklistItemDotsOpen(null);
    setTimeout(() => {
      if (inputRef.current) {
        inputRef.current.focus();
      }
    }, 0);
  };


  const handleSaveTitle = async (checklistId, taskId) => {

    try {
      if (editedChecklistTitle.length < 2 || editedChecklistTitle.length > 280) {
        console.log("Title should be between 2 and 280 characters!");
        return;
      }

      const data = {
        checklistId: checklistId,
        title: editedChecklistTitle,
        taskId: taskId
      };

      await putData('http://localhost:5157/backend/checklist/UpdateChecklist',data);

      const updatedChecklists = checklists.map((checklist) =>
        checklist.checklistId === checklistId ? { ...checklist, title: editedChecklistTitle } : checklist
      );

      setChecklists(updatedChecklists);
      getTaskActivities();
      setEditingChecklistId(null);
      setEditedChecklistTitle('');
    } catch (error) {
      console.error("Error updating checklist title: ",error.message);
      
      
    }
  }



  const handleCancelEditTitle = () => {
    setEditingChecklistId(null);
    setEditedChecklistTitle('');
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
              {editingChecklistId === checklist.checklistId ? (
                  <div className="text-sm flex my-2 items-center justify-between w-[100%] p-2">
                    <input
                      type="text"
                      value={editedChecklistTitle}
                      onChange={(e) => setEditedChecklistTitle(e.target.value)}
                      className="bg-gray-700 text-sm rounded p-1 w-full mr-2"
                      minLength={2}
                      maxLength={280}
                      ref={inputRef} // Attach the reference to the input
                    />
                    <button
                      className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold text-sm rounded px-2 py-1 mr-2"
                      onClick={() => handleSaveTitle(checklist.checklistId, checklist.taskId)}
                    >
                      Save
                    </button>
                    <button
                      className="bg-gray-800 hover:bg-slate-700 font-semibold text-sm rounded px-2 py-1"
                      onClick={handleCancelEditTitle}
                    >
                      Cancel
                    </button>
                  </div>
                ) : (
                  <span
                    className="h-10 items-center flex font-semibold cursor-pointer"
                    onClick={() => handleTitleEdit(checklist.checklistId, checklist.title)}
                  >
                    {checklist.title}
                  </span>
                )}
              <button
                className="font-semibold p-2 h-10 rounded-[4px] text-sm bg-gray-600 bg-opacity-30 w-[70px]"
                onClick={() => handleChecklistDelete(checklist.checklistId)}
              >
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

              <div className="flex items-center w-11/12 relative">
                {editingItemId === checklistItem.checklistItemId ? (
                  <div className="text-sm flex my-2 items-center justify-between w-[100%] p-2">
                    <input
                      type="text"
                      value={editedContent}
                      onChange={(e) => setEditedContent(e.target.value)}
                      className="bg-gray-700 text-sm rounded p-1 w-full mr-2"
                      minLength={2}
                      maxLength={280}
                      ref={inputRef} // Attach the reference to the input
                    />
                    <button
                      className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold text-sm rounded px-2 py-1 mr-2"
                      onClick={() => handleSaveEdit(checklist.checklistId, checklistItem)}
                    >
                      Save
                    </button>
                    <button
                      className="bg-gray-800 hover:bg-slate-700 font-semibold text-sm rounded px-2 py-1"
                      onClick={handleCancelEdit}
                    >
                      Cancel
                    </button>
                  </div>
                ) : (
                  <span
                    className="text-sm flex my-2 items-center justify-between w-[100%] hover:bg-gray-700 rounded-xl p-2 cursor-pointer"
                    onClick={() => handleItemClick(checklistItem.checklistItemId, checklistItem.content)}
                  >
                    {checklistItem.content}
                    <button
                      className="w-[5%] flex h-6 rounded-full hover:bg-gray-600 justify-center items-center"
                      onClick={(e) => {
                        e.stopPropagation();
                        toggleChecklistItemDots(checklistItem.checklistItemId);
                      }}
                    >
                      <FaEllipsisH />
                    </button>
                  </span>
                )}

                {checklistItemDotsOpen === checklistItem.checklistItemId && (
                  <ChecklistItemDeleteModal
                    checklistItemId={checklistItem.checklistItemId}
                    closeModal={closeModal}
                  />
                )}
              </div>
            </div>
          ))}

          <div className="flex w-full items-center px-4 mt-4 mb-2">
            {addingItem === checklist.checklistId ? (
              <div className="flex flex-col ml-10 my-2 w-[70%]">
                  <textarea className="bg-gray-700 text-sm rounded p-1 w-[100%] mr-2"
                  placeholder="Add an item"
                  value={newItemContent}
                  minLength={2}
                  maxLength={280}
                  onChange={(e) => setNewItemContent(e.target.value)}></textarea>
                <div className='flex mt-1'>
                  <button
                    className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold text-sm rounded px-2 py-1 mr-2"
                    onClick={() => handleAddItem(checklist.checklistId)}
                  >
                    Add
                  </button>
                  <button
                    className="bg-gray-800 hover:bg-slate-700 font-semibold text-sm rounded px-2 py-1"
                    onClick={handleCancelAddItem}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            ) : (
              <button
                className="bg-gray-600 bg-opacity-30 text-sm rounded px-2 py-1"
                onClick={() => handleAddClick(checklist.checklistId)}
              >
                Add an item
              </button>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};

export default Checklist;
