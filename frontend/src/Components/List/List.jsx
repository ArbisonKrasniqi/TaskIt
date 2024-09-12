import React, { useContext } from "react";
import Task from "../Task/Task.jsx";
import TaskForm from "../Task/TaskForm.jsx";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { useState } from "react";
import { useEffect } from "react";
import { useRef } from "react";
import { BoardContext } from "../BoardContent/Board.jsx";
import ListDropdown from "../Dropdowns/ListDropdown.jsx";
import { putData } from "../../Services/FetchService.jsx";

const List = ({ list, children }) => {
  const boardContext = useContext(BoardContext);
  const [isEditingListTitle, setIsEditingListTitle] = useState(false);
  const [newListTitle, setNewListTitle] = useState('');
  const [errorMessage, setErrorMessage] = useState("");
  const {getLists} = useContext(BoardContext);
  const handleShowTaskForm = (listId) => {
    boardContext.setSelectedListId(listId);
  }

  const handleHideTaskForm = () => {
    boardContext.setSelectedListId(null);
  }

  const handleListTitleChange = (e) => {
    setNewListTitle(e.target.value)
  }

  const handleSaveListTitle = async () => {
    try {
      if (newListTitle.trim()) {
        if (newListTitle.length < 2 || newListTitle.length > 20) {
          setErrorMessage("List title must be between 2 and 20 characters.");
          return;
        }

        const data = {
          listId: list.listId,
          title: newListTitle
        };

        await putData('http://localhost:5157/backend/list/UpdateList',data);
        setIsEditingListTitle(false);
        getLists();
        setErrorMessage('');
      }
    } catch (error) {
      
    }
  }

  const handleCancelEdit = () => {
    setNewListTitle('');
    setIsEditingListTitle(false);
  }

  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: list.listId,
    data: {
      type: 'list',
    },
  });

  const style = {
    transition,
    transform: CSS.Translate.toString(transform),
  };

  return (
    <div
      ref={setNodeRef} 
      {...attributes}
      {...listeners}
      style={style}
      className={`flex-shrink-0 bg-gray-800 p-2.5 rounded-lg shadow-lg w-[275px] h-auto ${isDragging ? 'opacity-0' : ''}`}
    >
       <header className="flex justify-between items-center">

          {isEditingListTitle ? (
                  <div className="flex flex-col justify-start space-x-2">
                      <input
                          type="text"
                          value={newListTitle}
                          onChange={handleListTitleChange}
                          className="border rounded p-1 bg-gray-800 text-white font-bold"
                          autoFocus
                      />
                      {errorMessage && (
                        <p className="text-red-500">{errorMessage}</p>
                      )}
                      <div className="flex flex-row justify-start my-1">
                      <button
                          className="bg-blue-500 hover:bg-blue-600 text-white font-semibold rounded px-3 py-1"
                          onClick={handleSaveListTitle}
                      >
                          Save
                      </button>
                      <button
                          className="bg-gray-800 hover:bg-slate-700 text-gray-400 font-semibold rounded px-3 py-1"
                          onClick={() => {handleCancelEdit(); setErrorMessage('')}}
                      >
                          Cancel
                      </button>
                      </div>
                  </div>
              ) : (
                <h3 className="text-xl font-bold pb-4 text-gray-100 w-full h-full"
                    onClick={() => {setIsEditingListTitle(true); setNewListTitle(list.title);}}>
                      {list.title}
                </h3>
              )}


              {isEditingListTitle ? ('') : (
                <ListDropdown
                listId={list.listId}
                onAddCardClick={() => handleShowTaskForm(list.listId)}
              />
              )}
        </header>
        {children}
        {boardContext.selectedListId === list.listId && (
              <TaskForm listId={list.listId} onClose={handleHideTaskForm} />
        )}
        {boardContext.selectedListId !== list.listId && (
              <button
                onClick={() => handleShowTaskForm(list.listId)}
                className="bg-transparent text-gray-500 px-4 py-2 rounded hover:bg-gray-700"
              >
                + Add Task
              </button>
        )}
    </div>
  );
};

export default List;