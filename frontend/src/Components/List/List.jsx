import React, { useState } from "react";
import Task from "../Task/Task";
import TaskForm from "../Task/TaskForm";
import ListForm from "./ListForm";

const List = ({listData, setLists, isSingleList = false}) => {
    const[localLists,setLocalLists] = useState(listData || []);

    const addList = (title) => {
        const newList = { id: Date.now(), title, tasks: [] };
        if (setLists) {
          setLists((prevLists) => [...prevLists, newList]);
        } else {
          setLocalLists((prevLists) => [...prevLists, newList]);
        }
    };


    const addTask = (listId, task) => {
        const updateLists = (prevLists) =>
          prevLists.map((list) =>
            list.id === listId ? { ...list, tasks: [...list.tasks, task] } : list
          );
    
        if (setLists) {
          setLists(updateLists);
        } else {
          setLocalLists(updateLists);
        }
      };
    
      const renderLists = () => (setLists ? listData : localLists);

    return(
        <div className="p-10 bg-zinc-400 min-h-screen">
        <div className="flex space-x-4">
          {renderLists().map((list) => (
            <div key={list.id} className="bg-slate-400 p-4 rounded-lg shadow-lg w-64">
              <h3 className="text-xl font-bold mb-4">{list.title}</h3>
              {list.tasks.map((task) => (
                <Task key={task.id} task={task} />
              ))}
              <TaskForm onAddTask={(task) => addTask(list.id, task)} />
            </div>
          ))}
          {isSingleList ? null : <ListForm onAddList={addList} />}
        </div>
      </div>
    );
};

export default List;