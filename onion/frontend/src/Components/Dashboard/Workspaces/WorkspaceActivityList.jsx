import { getDataWithId } from "../../../Services/FetchService";
import { useContext, useState, useEffect, createContext } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";
import { useParams } from "react-router-dom";
import WorkspaceActivityTable from "./WorkspaceActivityTable";

export const WorkspaceActivityContext = createContext();

const WorkspaceActivityList = () => {
    const [activities, setActivities] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const {workspaceId} = useParams();

    const getActivity = async () => {
        try {
            if (workspaceId) {
                console.log(workspaceId);
                const response = await getDataWithId("/GetWorkspaceActivityByWorkspaceId?WorkspaceId", workspaceId);
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
        <WorkspaceActivityContext.Provider value={contextValue}>
            <WorkspaceActivityTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </WorkspaceActivityContext.Provider>
    );
}

export default WorkspaceActivityList;