import React, { useState, useContext } from 'react';
import { postData } from './../../Services/FetchService'
import { MainContext } from '../../Pages/MainContext';

const CreateWorkspaceModal = ({open, onClose, onWorkspaceCreated, children}) => {
  const mainContext = useContext(MainContext);
  const [workspaceTitle, setWorkspaceTitle] = useState('');
  const [workspaceDescription, setWorkspacecDescription] = useState('');
  const [ownerId, setOwnerId] = useState(mainContext.userInfo.userId);

  const handleTitleChange = (e) => {
    setWorkspaceTitle(e.target.value);
  };

  const handleDescriptionChange = (e) => {
    setWorkspacecDescription(e.target.value);
  }

  const handleCreateWorkspace=  async () => {
    const newWorkspace = {
      title: workspaceTitle,
      description: workspaceDescription,
      ownerId: ownerId
    };

    console.log('Creating workspace with data: ',newWorkspace);

    try {
      const response = await postData('http://localhost:5157/backend/workspace/CreateWorkspace', newWorkspace);
      console.log('Workspace created successfully:', response.data);
      onWorkspaceCreated(response.data);
      onClose();
    } catch (error) {
      console.log('Failed to create board',error);
      console.log('Error response data:', error.message);
    }
  };

  return(
    <div className={`
      fixed inset-0 flex justify-center items-center transition-colors 
      ${open ? "visible bg-black/20" : "invisible"}
      `}>

        <div className={`bg-white rounded-xl shadow p-6 transition-all w-80 text-center
            ${open ? "scale-100 opacity-100" : "scale-125 opacity-0"}`}> 
            
            <button
                onClick={onClose}
                className="absolute top-1 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-50 hover:text-gray-600">
                X
            </button>
            <p className="w-full origin-left font-sans text-gray-400 font-bold text-l">Create Workspace</p>
            <hr className="w-full border-gray-400 mt-3"></hr>
            <br></br>
            
            <p className="w-full origin-left font-sans text-gray-400 font-semibold text-l">Workspace Title</p>
                <br></br>
                <input
                    type="text"
                    name="workspaceTitle"
                    id="workspaceTitle"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                    value={workspaceTitle}
                    onChange={handleTitleChange}
                />
                <br /><br />

                <p className="w-full origin-left font-sans text-gray-400 font-semibold text-l">Workspace Description</p>
                <br />
                <textarea 
                className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                placeholder='Describe your workspace...'
                value={workspaceDescription}
                onChange={handleDescriptionChange}></textarea>

                <br /><br />
                <button
                    className="bg-gray-800 font-bold text-white px-4 py-2 rounded-md w-[60%] hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out"
                    onClick={handleCreateWorkspace}
                >
                    Create
                </button>
                {children}

        </div>

    </div>
  );



}

export default CreateWorkspaceModal;