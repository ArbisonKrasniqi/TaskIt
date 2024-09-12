import { getDataWithId } from "../../../Services/FetchService";
import { useContext, useState, useEffect, createContext } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";
import { useParams } from "react-router-dom";
import TaskActivityTable from "./TaskActivityTable";

export const TaskActivityContext = createContext();

const TaskActivityList = () => {
    const [activities, setActivities] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const {taskId} = useParams();

    const getActivity = async () => {
        try {
            if (taskId) {
                const response = await getDataWithId("/GetTaskActivityByTaskId?TaskId", taskId);
                setActivities(response.data.sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate)));
            }
        }
        catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    }

    useEffect(()=>{
        getActivity();
    }, []);
    
    const contextValue= {activities, setActivities, getActivity}
    return(
        <TaskActivityContext.Provider value={contextValue}>
            <TaskActivityTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </TaskActivityContext.Provider>
    );
}

export default TaskActivityList;