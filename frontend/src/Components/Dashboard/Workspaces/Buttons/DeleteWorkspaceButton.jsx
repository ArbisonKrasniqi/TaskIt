import { deleteData } from "../../../../Services/FetchService";
import React, {useContext} from 'react';
import { WorkspacesContext } from "../WorkspacesList";
import DeleteButton from "../../Buttons/DeleteButton";
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
            <DeleteButton onClick={()=> {HandleWorkspaceDelete(props.id)}} type="button" name="Delete" />
        </>
    );
}
export default DeleteWorkspaceButton;