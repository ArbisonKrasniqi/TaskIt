import { useState, useEffect, createContext } from "react";
import { getData } from "../../../Services/FetchService";
import TasksTable from "./TasksTable";
//tasks error modal

export const TasksContext = createContext();

const TasksList = () => {
    const [tasks, setTasks] = useState(null);
    const [showTasksErrorModal, setShowTasksErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error");

    const getTasks = async () => {
        try {
            const data = await getData('/backend/task/GetAllTasks');
            setTasks(data.data);
        } catch (error) {
            setErrorMessage(error.message);
            setShowTasksErrorModal(false);
        }
    };

    useEffect(() => {
        getTasks();
    }, []);

    const contextValue = {tasks, setTasks, showTasksErrorModal, setShowTasksErrorModal, errorMessage, setErrorMessage, getTasks};

    return (
        <TasksContext.Provider value={contextValue}>
            <TasksTable/>
            {showTasksErrorModal && /*TasksErrorModal*/1 }
        </TasksContext.Provider>
    );

}
export default TasksList