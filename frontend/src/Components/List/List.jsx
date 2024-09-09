import React, { useState, useContext } from "react";
import Task from "../Task/Task";
import TaskForm from "../Task/TaskForm";
import ListForm from "./ListForm";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import ListDropdown from "../Dropdowns/ListDropdown";

const List = ({ setLists, isSingleList = false }) => {
  const workspaceContext = useContext(WorkspaceContext);
  const [selectedListId, setSelectedListId] = useState(null);

  const handleShowTaskForm = (listId) => {
    setSelectedListId(listId);
  };

  const handleHideTaskForm = () => {
    setSelectedListId(null);
  };

  if (workspaceContext.lists == null) {
    return <div>Loading...</div>;
  } else {
    return (
      <div className="flex space-x-4 flex-nowrap">
        {workspaceContext.lists.map((list) => (
          <div
            key={list.listId}
            className="bg-slate-400 p-4 rounded-lg shadow-lg w-64 flex-shrink-0"
          >
            <header className="flex justify-between items-center mb-4">
              <h3 className="text-xl font-bold mb-4">{list.title}</h3>
              <ListDropdown
                listId={list.listId}
                onAddCardClick={() => handleShowTaskForm(list.listId)}
              />
            </header>
            <div className="h-64 overflow-auto">
              {list.tasks.map((task) => (
                <Task key={task.id} task={task} />
              ))}
            </div>
            {selectedListId === list.listId && (
              <TaskForm listId={list.listId} onClose={handleHideTaskForm} />
            )}
        
            {/*show add task when task form is not visible */}
            {selectedListId !== list.listId && (
              <button
                onClick={() => handleShowTaskForm(list.listId)}
                className="mt-4 p-2 bg-blue-500 text-white rounded"
              >
                Add Task
              </button>
            )}
          </div>
        ))}
        {!isSingleList && (
          <div className="mt-4">
            <ListForm onAddList={setLists} />
          </div>
        )}
      </div>
    );
  }
};

export default List;