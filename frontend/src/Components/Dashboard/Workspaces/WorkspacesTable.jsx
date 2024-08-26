import React, { useState, useContext, createContext } from 'react';
import UpdateWorkspaceButton from "./Buttons/UpdateWorkspaceButton.jsx";
import { WorkspacesContext } from './WorkspacesList.jsx';
import CustomButton from '../Buttons/CustomButton.jsx';
import { deleteData } from '../../../Services/FetchService.jsx';
import { useNavigate } from 'react-router-dom';

export const UpdateContext = createContext();

const WorkspacesTable = () => {
    const navigate = useNavigate();
    const workspacesContext = useContext(WorkspacesContext);
    const [searchQuery, setSearchQuery] = useState('');

    const HandleWorkspaceDelete = (id) => {
        async function deleteWorkspace() {
            try {
                const data = { workspaceId: id };
                const response = await deleteData('/backend/workspace/DeleteWorkspace', data);
                console.log(response);
                // Update workspaces after deletion
                const updatedWorkspaces = (workspacesContext.workspaces || []).filter(workspace => workspace.workspaceId !== id);
                workspacesContext.setWorkspaces(updatedWorkspaces);
            } catch (error) {
                workspacesContext.setErrorMessage(error.message + id);
                workspacesContext.setShowWorkspacesErrorModal(true);
                workspacesContext.getWorkspaces();
            }
        }
        deleteWorkspace();
    }

    const handleWorkspaceRowClick = workspaceId => {
        console.log(workspaceId);
        navigate(`/dashboard/workspace/${workspaceId}`);
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    // Ensure workspacesContext.workspaces is always an array
    const workspaces = workspacesContext.workspaces || [];
    const filteredWorkspaces = workspaces.filter(workspace => {
        const workspaceIdMatch = workspace.workspaceId.toString().toLowerCase().includes(searchQuery);
        const titleMatch = workspace.title.toLowerCase().includes(searchQuery);
        const descriptionMatch = workspace.description.toLowerCase().includes(searchQuery);
        const ownerIdMatch = workspace.ownerId.toString().toLowerCase().includes(searchQuery);
        
        return workspaceIdMatch || titleMatch || descriptionMatch || ownerIdMatch;
    });

    return (
        <div className="overflow-x-auto">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for workspaces by ID, title, description, or owner ID"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            <div className="relative overflow-x-auto max-h-[500px]">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="sticky top-0 text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">Workspace ID</th>
                            <th className="px-6 py-3">Title</th>
                            <th className="px-6 py-3">Description</th>
                            <th className="px-6 py-3">Owner ID</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredWorkspaces.length > 0 ? (
                            filteredWorkspaces.map((workspace, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 cursor-pointer"
                                    onClick={() => handleWorkspaceRowClick(workspace.workspaceId)}
                                >
                                    <td className="px-6 py-4">{workspace.workspaceId}</td>
                                    <td className="px-6 py-4">{workspace.title}</td>
                                    <td className="px-6 py-4">{workspace.description}</td>
                                    <td className="px-6 py-4">{workspace.ownerId}</td>
                                    <td className="px-6 py-4">
                                        <UpdateContext.Provider value={workspace}>
                                            <UpdateWorkspaceButton onClick={(e) => e.stopPropagation()} />
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                HandleWorkspaceDelete(workspace.workspaceId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No workspaces found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default WorkspacesTable;
