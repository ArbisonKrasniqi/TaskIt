import SideMenusHeader from "./SideMenusHeader";
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useContext, useState } from "react";
import { putData,deleteData } from "../../Services/FetchService";
const WorkspaceSettings = () =>{

    const { workspace, setWorkspace } = useContext(WorkspaceContext);
    const [isEditing, setIsEditing] = useState(false);
    const[description, setDescription] =useState('');
    const[errorMessage, setErrorMessage] = useState('');

    if (workspace == null) {
        return <div>Loading...</div>;
    }

    
    const handleEditClick = () =>{
        setDescription(workspace.description || ''); 
        setIsEditing(true);
    }
  

    const handleInputChange = (e) =>{
        setDescription(e.target.value);
    }

    const  handleSaveClick = async () =>{
        if(description<10 || description>280){
            setErrorMessage('Workspace description must be between 10 and 280 characters.');
            return;
        }
        const updatedWorkspace = {
            WorkspaceId: workspace.workspaceId,
            Title: workspace.title,
            Description: description,
            OwnerId: workspace.ownerId,
        }
        console.log('Updating workspace with data: ',updatedWorkspace);
        try{
            const response = await putData('http://localhost:5157/backend/workspace/UpdateWorkspace', updatedWorkspace);
        console.log('Workspace updated successfully! ',response.data);

        setWorkspace(prevWorkspace => ({
            ...prevWorkspace, description: description
        }));
        
        setIsEditing(false);
        }catch (error) {
            console.log('Error response data: ', error.response?.data || error.message);
        }
    };
     const handleDeleteWorkspace = async(workspaceId) =>{
        console.log('Deleting workspace with Id: ', workspaceId);
        try{
            const response = await deleteData('http://localhost:5157/backend/workspace/DeleteWorkspace', { workspaceId: workspaceId });
            console.log('Deleting workspace response:', response);

        }
        catch(error){
            console.error('Error deleting workspace:', error.message);
        }

     };
return(
    <div className={`duration-100 h-full`} style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
 <SideMenusHeader/>
    <div className="font-semibold font-sans text-gray-400 ml-20 mt-10">
    <h1 className="text-3xl">
       Workspace Settings</h1>
       <h1 className="text-xl mt-5">Workspace Description:</h1>
        {!isEditing ? (
            <div>
                <p className="text-l mt-3 mb-10">{workspace.description}</p>
                <button className="text-blue-500 hover:text-blue-700" onClick={handleEditClick}>
                    Edit description
                </button>
                </div>
        ): (
            <div>
            <input 
                type="text" 
                value={description} 
                onChange={handleInputChange}
                className="text-xl mt-3 mb-10 p-2 border border-gray-500 rounded"
            />
            <button 
                className="text-blue-500 hover:text-blue-700 ml-4"
                onClick={handleSaveClick}
            >
                Save
            </button>
            {errorMessage && <p className="text-red-500 mt-2">{errorMessage}</p>}
        </div>
        )}

    </div>
    <hr className="w-full border-gray-400"></hr>
    <div className="mt-10 ml-10">
        
    <button className="p-1 text-red-700 border-none font-serif font-bold text-xl hover:text-red-600">Delete this workspace?</button>

    </div>
    </div>



);





}
export default WorkspaceSettings