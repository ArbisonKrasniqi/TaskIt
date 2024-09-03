import { createContext, useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard";
import { getData, getDataWithId } from "../../../Services/FetchService";
import ChecklistsTable from "./ChecklistsTable.jsx";
import DashboardErrorModal from "../DashboardErrorModal";
export const ChecklistsContext = createContext();

const ChecklistsList = () => {
    const {taskId} = useParams();
    const [checklists, setChecklists] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const getChecklists = async () => {
        try {
            if (taskId) {
                const checklistsWithTaskId = await getDataWithId('/backend/checklist/GetChecklistByTaskId?taskId', taskId);
                setChecklists(checklistsWithTaskId.data);
            } else {
                const allChecklists = await getData('/backend/checklist/GetAllChecklists');
                setChecklists(allChecklists.data);
            }
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };

    useEffect(() => {
        getChecklists();
    }, [taskId]);

    const contextValue = {checklists, setChecklists, getChecklists};

    return(
        <ChecklistsContext.Provider value={contextValue}>
            <ChecklistsTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </ChecklistsContext.Provider>
    )
} 
export default ChecklistsList;