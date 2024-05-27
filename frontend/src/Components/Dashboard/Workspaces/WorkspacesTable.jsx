import React, {useContext, createContext} from 'react';
import DeleteWorkspaceButton from "./Buttons/DeleteWorkspaceButton.jsx";
import UpdateWorkspaceButton from "./Buttons/UpdateWorkspaceButton.jsx";
import { WorkspacesContext} from './WorkspacesList.jsx';

export const UpdateContext = createContext();

const WorkspacesTable = () => {

    const workspacesContext = useContext(WorkspacesContext); //thirret konteksti qe me mujt me ju qas variablave/funksioneve


return(

    <div className="overflow-x-auto">
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
           <tr>
            
           <th className="px-6 py-3" >ID</th>
           <th className="px-6 py-3" >Title</th>
           <th className="px-6 py-3" >Description</th>
           <th className="px-6 py-3" >Owner Id</th>    
           <th className="px-6 py-3" >Actions</th>       
        </tr>     
        </thead>    

        <tbody>

    {/* 
        Krijojm nga nje rresht per secilin Workspace me atributet perkatese
        dhe krijojme butonin delete per te fshire ne baze te id te cilen e merr nga workspace ne rreshtin perkates
        dhe butoni edit ku per secilin workspace krijohet kontekst i ri ne menyre qe me editu secilin ne menyre te veqante
    */}

      {workspacesContext.workspaces ? (workspacesContext.workspaces.map((workspace, index) => (
        <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                  <td className="px-6 py-4">{workspace.workspaceId}</td>
                  <td className="px-6 py-4">{workspace.title}</td>
                  <td className="px-6 py-4">{workspace.description}</td>
                  <td className="px-6 py-4">{workspace.ownerId}</td>
                  <td className="px-6 py-4"> <UpdateContext.Provider value={workspace}> <UpdateWorkspaceButton/>  </UpdateContext.Provider> <DeleteWorkspaceButton id={workspace.workspaceId}/> </td>
        </tr>
      ))): (

        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
              <td className="px-6 py-4"></td>
              <td className="px-6 py-4"></td>
              <td className="px-6 py-4"></td>
              <td className="px-6 py-4"></td>
        </tr>
      )}

        </tbody>
        </table>
    </div>
);
}


export default WorkspacesTable