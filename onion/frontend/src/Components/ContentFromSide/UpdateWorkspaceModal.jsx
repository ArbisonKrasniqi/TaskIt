import { useState, useEffect } from "react";
import { putData } from "../../Services/FetchService";

const UpdateWorkspaceModal = ({open, onClose, workspace, onWorkspaceUpdated})=>{

    const[workspaceTitle, setWorkspaceTitle] = useState('');
    const[workspaceDescription, setWorkspaceDescription] = useState('');
    const [errorMessage, setErrorMessage] =useState('');
    useEffect(() => {
        if (workspace) {
          setWorkspaceTitle(workspace.title || '');
          setWorkspaceDescription(workspace.description || '');
        }
      }, [workspace])

    const handleTitleChange = (e) => {
        setWorkspaceTitle(e.target.value);
        setErrorMessage('');
    };

    const handleDescriptionChange = (e) => {
        setWorkspaceDescription(e.target.value);
        setErrorMessage('');
    };
    
    
    const handleUpdateWorkspace = async () =>{

        if(workspaceTitle.length < 2 || workspaceTitle.length > 25 ){
            setErrorMessage('Workspace title must be between 2 and 20 characters.');
            return;
        }
        if(workspaceDescription<10 || workspaceDescription>280){
            setErrorMessage('Workspace description must be between 10 and 280 characters.');
            return;
        }

    const updatedWorkspace = {
        WorkspaceId: workspace.workspaceId,
        Title: workspaceTitle,
        Description: workspaceDescription,
        OwnerId: workspace.ownerId,
    };
    console.log('Updating workspace with data: ',updatedWorkspace);

    try{
        const response = await putData('http://localhost:5157/backend/workspace/UpdateWorkspace', updatedWorkspace);
        console.log('Workspace updated successfully! ',response.data);
        onWorkspaceUpdated(updatedWorkspace);
        onClose();
    }catch (error) {
        if (error.response && error.response.data && error.response.data.errors) {
            const serverErrors = error.response.data.errors;
            if (serverErrors.Title) {
                setErrorMessage(serverErrors.Title[0]);
            } else if (serverErrors.Description) {
                setErrorMessage(serverErrors.Description[0]);
            } else {
                setErrorMessage('An unknown error occurred.');
            }
        } else {
            setErrorMessage('An error occurred while updating the workspace.');
        }
        console.log('Error response data: ', error.response?.data || error.message);
    }
};
if(!open)return null;
   
    return (
        <div
            className={`
        fixed z-30 inset-0 flex justify-center items-center transition-colors 
        ${open ? "visible bg-black/20" : "invisible"}
        `}>
            <div
                className={`bg-white rounded-xl shadow p-6 transition-all w-80 text-center
            ${open ? "scale-100 opacity-100" : "scale-125 opacity-0"}`}>
                <button
                    onClick={onClose}
                    className="absolute top-1 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-50 hover:text-gray-600">
                    X
                </button>
                <p className="w-full origin-left font-sans text-gray-500 font-bold text-l">Update Workspace</p>
                <hr className="w-full border-gray-400 mt-3"></hr>
                <br></br>
                
                <p className="w-full origin-left font-sans text-gray-500 font-semibold text-l">Title</p>
                <br></br>
                <input
                    type="text"
                    name="workspaceTitle"
                    id="workspaceTitle"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                    value={workspaceTitle}
                    onChange={handleTitleChange}
                />
                  
                
                <br></br>
                <p className="w-full origin-left font-sans text-gray-500 font-semibold text-l">Description</p>
                <br></br>
                <textarea
                    type="text"
                    name="workspaceDescription"
                    id="workspaceDescription"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                    value={workspaceDescription}
                    onChange={handleDescriptionChange}
                />
                   {errorMessage && <p className="text-red-500 text-sm">{errorMessage}</p>}
                <br /><br />
              
                <button
                    className="bg-gray-800 font-bold text-white px-4 py-2 rounded-md w-[60%] hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out"
                    onClick={handleUpdateWorkspace}
                >
                    Update
                </button>
               
            </div>
        </div>
    );
}
export default UpdateWorkspaceModal;