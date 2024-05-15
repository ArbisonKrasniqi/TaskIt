import { deleteData } from "../../../../Services/FetchService";
import React, {useContext} from 'react';
import { WorkspacesContext } from "../WorkspacesList";

const DeleteWorkspaceButton = (props) => {
    const workspacesContext = useContext(WorkspacesContext);

    const HandleWorkspaceDelete = (id) => {
        async function deleteWorkspace(){
            try{
                const data = {
                    workspaceId: id
                };
                const response = await deleteData('http://localhost:5157/backend/workspace/DeleteWorkspace', data);
                console.log(response);
                //e perdorim workspaceContext ku i kena krejt workspaces dhe e fshijna nga lista workspace-in qe e bonem delete
                const updatedWorkspaces = workspacesContext.workspaces.filter(workspace => workspace.workspaceId !==id);
                workspacesContext.setWorkspaces(updatedWorkspaces);
            }
            catch(error){
                workspacesContext.setErrorMessage(error.message + id); //vendose messazhin

                workspacesContext.setShowWorkspacesErrorModal(true); //shfaqe modalin e errorit

                workspacesContext.getWorkspaces(); //bej fetch workspaces per me perditsu listen
            }
        }
        deleteWorkspace();
    }

    return(
        <>
        
            <button onClick={()=> {HandleWorkspaceDelete(props.id)}} className="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900" >Delete</button>
        </>
    );
}
export default DeleteWorkspaceButton;