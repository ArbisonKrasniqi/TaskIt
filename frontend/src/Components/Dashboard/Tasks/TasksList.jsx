import { useState, useEffect, createContext, useContext } from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import TasksTable from "./TasksTable";
import DashboardErrorModal from "../DashboardErrorModal.jsx";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard.jsx";

export const TasksContext = createContext();

const TasksList = () => {
    const { listId } = useParams();
    const [tasks, setTasks] = useState(null);
    const dashboardContext = useContext(DashboardContext);

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
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };

    useEffect(() => {
        getTasks();
    }, [listId]);

    const contextValue = {tasks, setTasks, getTasks};

    return (
        <TasksContext.Provider value={contextValue}>
            <TasksTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/> }
        </TasksContext.Provider>
    );

}
export default TasksList