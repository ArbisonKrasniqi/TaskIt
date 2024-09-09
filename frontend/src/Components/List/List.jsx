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

const List = ({ list, children }) => {
  const boardContext = useContext(BoardContext);

  const handleShowTaskForm = (listId) => {
    boardContext.setSelectedListId(listId);
  }

  const handleHideTaskForm = () => {
    boardContext.setSelectedListId(null);
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
      style={style}
      className={`flex-shrink-0 bg-gray-800 p-4 rounded-lg shadow-lg w-[275px] h-auto ${isDragging ? 'opacity-0' : ''}`}
    >
       <header className="flex justify-between items-center mb-4">
              <h3 {...listeners} className="text-xl font-bold mb-4 text-gray-100">{list.title}</h3>
              <ListDropdown
                listId={list.listId}
                onAddCardClick={() => handleShowTaskForm(list.listId)}
              />
        </header>
        {children}
        {boardContext.selectedListId === list.listId && (
              <TaskForm listId={list.listId} onClose={handleHideTaskForm} />
        )}
        {boardContext.selectedListId !== list.listId && (
              <button
                onClick={() => handleShowTaskForm(list.listId)}
                className="mt-4 p-2 bg-blue-500 text-white rounded"
              >
                Add Task
              </button>
        )}
    </div>
  );
};

export default List;