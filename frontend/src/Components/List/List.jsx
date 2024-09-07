import React from "react";
import Task from "../Task/Task.jsx";
import TaskForm from "../Task/TaskForm.jsx";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { BoardContext } from "../BoardContent/Board.jsx";
import { useContext } from "react";

const List = ({ list }) => {
  const boardContext = useContext(BoardContext);
  const { attributes, listeners, setNodeRef, transform, transition } = useSortable({
    id: list.listId
  });

  const style = {
    transition,
    transform: CSS.Transform.toString(transform),
  };

  return (
    <div
      ref={setNodeRef}
      {...attributes}
      {...listeners}
      style={style}
      className="bg-slate-400 p-4 rounded-lg shadow-lg w-64"
    >
      <h3 className="text-xl font-bold mb-4">{list.title}</h3>

      {boardContext.boardTasks
      .filter((task) => task.listId === list.listId)
      .sort((a, b) => a.index - b.index)
      .map((task) => (
        <Task key={task.taskId} task={task} />
      ))}
      <TaskForm listId={list.listId} />
    </div>
  );
};

export default List;
