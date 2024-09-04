import React, { useContext, useEffect, useState } from "react";
import Task from "../Task/Task";
import TaskForm from "../Task/TaskForm";
import ListForm from "./ListForm";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const List = ({ listData = [], setLists, isSingleList = false }) => {
  const workspaceContext = useContext(WorkspaceContext);

  const [localLists, setLocalLists] = useState(listData);
  const [hoveredListIndex, setHoveredListIndex] = useState(null); // Track hovered list

  if(workspaceContext.lists == null) {
    return (<div> skasen</div>);
  }else {
    return(
      <div className="p-10 bg-gray-200 min-h-screen">
      <div className="flex space-x-4">
        {workspaceContext.lists.map((listData, index) => (
          <div
            div key={workspaceContext.lists.listId} 
            className="bg-slate-400 p-4 rounded-lg shadow-lg w-64"
          >
            <h3 className="text-xl font-bold mb-4">{listData.title}</h3>
            {/* Uncomment the following lines to render tasks and task form */}
            {/* {listData.tasks.map((task) => (
              <Task key={task.id} task={task} />
            ))}
            <TaskForm onAddTask={(task) => addTask(listData.id, task)} /> */}
          </div>
        ))}
        {isSingleList ? null : <ListForm  />}
      </div>
    </div>
    )
  }
};

export default List;
