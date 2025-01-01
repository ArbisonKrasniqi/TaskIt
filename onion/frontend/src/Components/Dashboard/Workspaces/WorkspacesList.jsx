import { useState, useEffect, createContext, useContext } from "react";
import {getData, getDataWithId} from '../../../Services/FetchService.jsx';
import WorkspacesTable from './WorkspacesTable.jsx';
import { useParams } from "react-router-dom";
import DashboardErrorModal from "../DashboardErrorModal.jsx";
import { DashboardContext } from "../../../Pages/dashboard.jsx";

export const WorkspacesContext = createContext();

const WorkspacesList = () => { 
    const { userId } = useParams();
    const [workspaces, setWorkspaces] = useState(null); //lista e workspaces nga API
    const dashboardContext = useContext(DashboardContext);


    const getWorkspaces = async () => {
        try{
            if (userId) {
                const workspacesWithUserId = await getDataWithId('/backend/workspace/GetWorkspacesByMemberId?memberId', userId);
                setWorkspaces(workspacesWithUserId.data);
            } else {
                const allWorkspaces = await getData('/backend/workspace/GetAllWorkspaces');
                setWorkspaces(allWorkspaces.data);
            }
        }catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true); //shfaqe WorkspaceErrorModalin
        }
    };

    useEffect(()=> {getWorkspaces();}, [userId]);


    const contextValue = {workspaces, setWorkspaces, getWorkspaces};


    return(

        <WorkspacesContext.Provider value={contextValue}>

            <WorkspacesTable/> 
            {/*workspaces table dhe krejt femijet e tij kan me pas qasje ne krejt ato qe jon ne contextValue */
            /* Ne fillim WorkspacesErrorModal nuk shfaqet sepse showWorkspacesErrorModal default value e ka false */
            }
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}

       </WorkspacesContext.Provider>     
    );
}



export default WorkspacesList;