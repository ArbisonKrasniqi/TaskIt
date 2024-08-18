import { useState, useEffect, createContext } from "react";
import {getData} from '../../../Services/FetchService.jsx';
import WorkspacesTable from './WorkspacesTable.jsx';
import WorkspacesErrorModal from './Modals/WorkspacesErrorModal.jsx';

export const WorkspacesContext = createContext();

const WorkspacesList = () => {

    const [workspaces, setWorkspaces] = useState(null); //lista e workspaces nga API
    const [showWorkspacesErrorModal, setShowWorkspacesErrorModal] = useState(false); //vleren fillestare false se ska asnje error ne fillim
    const [errorMessage, setErrorMessage] = useState("There has been a server error!"); //perdoret per te vendosur vlera te ndryshme ne showWorkspacesErrorModal


    const getWorkspaces = async () => {
        try{
            const data = await getData('/backend/workspace/GetAllWorkspaces');
            setWorkspaces(data.data);
        }catch(error){
            setErrorMessage(error.message);
            setShowWorkspacesErrorModal(true); //shfaqe WorkspaceErrorModalin
        }
    };

    useEffect(()=> {getWorkspaces();}, []);


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



export default WorkspacesList