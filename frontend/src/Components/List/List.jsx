import React, { useContext, useState } from "react";
import Task from "../Task/Task";
import TaskForm from "../Task/TaskForm";
import ListForm from "./ListForm";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const List = ({ listData = [], setLists, isSingleList = false }) => {
  const workspaceContext = useContext(WorkspaceContext);

  if (workspaceContext.lists == null) {
    return <div>Loading...</div>;
  } else {
    return (
      <div className="p-10 bg-gray-200 min-h-screen">
        <div className="flex space-x-4">
          {workspaceContext.lists.map((list) => (
            <div
              key={list.listId}
              className="bg-slate-400 p-4 rounded-lg shadow-lg w-64"
            >
              <h3 className="text-xl font-bold mb-4">{list.title}</h3>
              {list.tasks.map((task) => (
                <Task key={task.id} task={task} />
              ))}
              {list.listId ? (
                <TaskForm listId={list.listId} />
              ) : (
                <p>Loading task form...</p>
              )}
            </div>
          ))}
          {!isSingleList && <ListForm />}
        </div>
      </div>
    );
  }
};

export default List;
