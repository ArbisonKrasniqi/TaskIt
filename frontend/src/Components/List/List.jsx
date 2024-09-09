import React from "react";
import Task from "../Task/Task.jsx";
import TaskForm from "../Task/TaskForm.jsx";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { useState } from "react";
import { useEffect } from "react";
import { useRef } from "react";

const List = ({ list, children }) => {
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
      <h3 {...listeners} className="text-xl font-bold mb-4 text-slate-200 text-center">{list.title}</h3>

      {children}
      <TaskForm listId={list.listId} />
    </div>
  );
};

export default List;
