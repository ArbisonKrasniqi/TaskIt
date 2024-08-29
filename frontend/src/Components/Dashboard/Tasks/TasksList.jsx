import { useState, useEffect, createContext } from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import TasksTable from "./TasksTable";
import TasksErrorModal from "./Modals/TasksErrorModal.jsx";
import { useParams } from "react-router-dom";

export const TasksContext = createContext();

const TasksList = () => {
    const { listId } = useParams();
    const [tasks, setTasks] = useState(null);
    const [showTasksErrorModal, setShowTasksErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error");

    const getTasks = async () => {
        try {
            if (listId) {
                const tasksWithListId = await getDataWithId('backend/task/GetTaskByListId?listId',listId);
                setTasks(tasksWithListId.data);
            } else {
                const allTasks = await getData('/backend/task/GetAllTasks');
                setTasks(allTasks.data);
            }   
        } catch (error) {
            setErrorMessage(error.message);
            setShowTasksErrorModal(true);
        }
    };

    useEffect(() => {
        getTasks();
    }, [listId]);

    const contextValue = {tasks, setTasks, showTasksErrorModal, setShowTasksErrorModal, errorMessage, setErrorMessage, getTasks};

    return (
        <TasksContext.Provider value={contextValue}>
            <TasksTable/>
            {showTasksErrorModal && <TasksErrorModal/> }
        </TasksContext.Provider>
    );

}
export default TasksList