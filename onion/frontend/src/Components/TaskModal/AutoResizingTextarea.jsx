import { useState, useRef, useEffect, useContext } from "react";
import { TaskModalsContext } from "./TaskModal";
import { putData } from "../../Services/FetchService";

const AutoResizingTextarea = ({ taskDescription }) => {
  const { taskData} = useContext(TaskModalsContext);
  const [description, setDescription] = useState(taskDescription);
  const taskModalsContext = useContext(TaskModalsContext);

  const [isEditing, setIsEditing] = useState(false);
  const [tempDescription, setTempDescription] = useState(taskData.description);
  const textareaRef = useRef(null);

  //Auto-resize the textarea based on its content
  useEffect(() => {
    if (textareaRef.current) {
      textareaRef.current.style.height = "auto";
      textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`; //Set to scrollHeight
    }
  }, [tempDescription]);

  const handleSaveClick = async () => {
    try {
      const data = {
        taskId: taskData.taskId,
        title: taskData.title,
        description: tempDescription,
        dateAdded: taskData.dateAdded,
        dueDate: taskData.dueDate,
        listId: taskData.listId,
      };
      await putData('http://localhost:5127/backend/task/UpdateTask', data);
      setDescription(tempDescription);
      taskModalsContext.setTaskData({ ...taskData, description: tempDescription });
      setIsEditing(false);
    } catch (error) {
      console.error("Thers been an error saving task");
      taskModalsContext.getTaskById();
    }
  };

  const handleCancelClick = () => {
    setTempDescription(description);
    setIsEditing(false);
  };

  const handleTextAreaClick = (currentContent) => {
    setIsEditing(true);
    setTempDescription(currentContent);

    setTimeout(() => {
      if (textareaRef.current) {
        textareaRef.current.focus();
        const length = textareaRef.current.value.length; //per me qit kursorin nfund
        textareaRef.current.setSelectionRange(length, length);
      }
    }, 0);
  };

  return (
    <div className="relative">
      {isEditing ? (
        <div>
          <textarea
            ref={textareaRef}
            value={tempDescription}
            onChange={(e) => setTempDescription(e.target.value)}
            className="bg-gray-700 bg-opacity-50 p-2 rounded-md resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 overflow-hidden w-full"
            rows={1}
          />
          <div className="flex justify-start space-x-2 mt-2">
            <button
              className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold rounded px-3 py-1"
              onClick={handleSaveClick}
            >
              Save
            </button>
            <button
              className="bg-gray-800 hover:bg-slate-700 text-gray-400 font-semibold rounded px-3 py-1"
              onClick={handleCancelClick}
            >
              Cancel
            </button>
          </div>
        </div>
      ) : (
        <div onClick={() => handleTextAreaClick(taskData.description)} className="cursor-pointer">
          <p className="bg-gray-700 bg-opacity-50 p-2 rounded-md">
            {taskData.description ? (taskData.description) : 'Add a more detailed description...'}
          </p>
        </div>
      )}
    </div>
  );
};

export default AutoResizingTextarea;
