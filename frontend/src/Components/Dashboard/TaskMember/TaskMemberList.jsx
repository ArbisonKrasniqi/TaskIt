import { createContext, useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard";
import { getData, getDataWithId } from "../../../Services/FetchService";
import DashboardErrorModal from "../DashboardErrorModal";
import TaskMemberTable from "./TaskMemberTable";

export const TaskMembersContext = createContext();

const TaskMemberList = () => {
    const {taskId} = useParams();
    const [taskMembers, setTaskMembers] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const getTaskMembers = async () => {
        try {
            if (taskId) {
                const taskMembersResponse = await getDataWithId("/backend/TaskMembers/GetAllTaskMembersByTaskId?taskId", taskId);
                setTaskMembers(taskMembersResponse.data);
            } else {
                const allTaskMembersResponse = await getData("/backend/TaskMembers/GetAllTaskMembers");
                setTaskMembers(allTaskMembersResponse.data);
            }
    
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    }

    useEffect(() => {
        getTaskMembers();
    }, [taskId])

    const contextValue = {taskMembers, setTaskMembers, getTaskMembers};

    return (
        <TaskMembersContext.Provider value={contextValue}>
            {dashboardContext.ShowDashboardErrorModal && <DashboardErrorModal/>}
            <TaskMemberTable/>
        </TaskMembersContext.Provider>

    )

}

export default TaskMemberList;