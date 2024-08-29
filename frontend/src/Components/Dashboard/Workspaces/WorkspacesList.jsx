import { useState, useEffect, createContext } from "react";
import {getData, getDataWithId} from '../../../Services/FetchService.jsx';
import WorkspacesTable from './WorkspacesTable.jsx';
import WorkspacesErrorModal from './Modals/WorkspacesErrorModal.jsx';
import { useParams } from "react-router-dom";

export const WorkspacesContext = createContext();

const WorkspacesList = () => { 
    const { userId } = useParams();
    const [workspaces, setWorkspaces] = useState(null); //lista e workspaces nga API
    const [showWorkspacesErrorModal, setShowWorkspacesErrorModal] = useState(false); //vleren fillestare false se ska asnje error ne fillim
    const [errorMessage, setErrorMessage] = useState("There has been a server error!"); //perdoret per te vendosur vlera te ndryshme ne showWorkspacesErrorModal


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
            setErrorMessage(error.message);
            setShowWorkspacesErrorModal(true); //shfaqe WorkspaceErrorModalin
        }
    };

    useEffect(()=> {getWorkspaces();}, [userId]);


    const contextValue = {workspaces, setWorkspaces, getWorkspaces, showWorkspacesErrorModal, setShowWorkspacesErrorModal, errorMessage, setErrorMessage};


    return(

        <WorkspacesContext.Provider value={contextValue}>

            <WorkspacesTable/> 
            {/*workspaces table dhe krejt femijet e tij kan me pas qasje ne krejt ato qe jon ne contextValue */
            /* Ne fillim WorkspacesErrorModal nuk shfaqet sepse showWorkspacesErrorModal default value e ka false */
            }
            {showWorkspacesErrorModal && <WorkspacesErrorModal/>}

       </WorkspacesContext.Provider>     
    );
}



export default WorkspacesList;