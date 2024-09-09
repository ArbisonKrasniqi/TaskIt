import { useContext, useState } from "react";
import SideMenusHeader from "./SideMenusHeader";
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { putData } from "../../Services/FetchService";
import DeleteWorkspaceModal from "./DeleteWorkspaceModal";
import MessageModal from "./MessageModal";

const WorkspaceSettings = () => {
    const { workspace, setWorkspace, roli, setShowDeleteWorkspaceModal, showDeleteWorkspaceModal, activities, getInitials } = useContext(WorkspaceContext);
    const [isEditing, setIsEditing] = useState(false);
    const [description, setDescription] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [isMessageModalOpen, setIsMessageModalOpen] = useState(false);
    const [message, setMessage] = useState('');
    const [visibleActivities, setVisibleActivities] = useState(10);
    const [searchTerm, setSearchTerm] = useState(''); // For search functionality

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        const formattedDate = date.toLocaleDateString('en-US');
        const formattedTime = date.toLocaleTimeString('en-US', {
            hour: '2-digit',
            minute: '2-digit',
            hour12: true 
        });
        return `${formattedDate} - ${formattedTime}`;
    };

    if (workspace == null) {
        return <div>Loading...</div>;
    }

    const loadMoreActivities = () => {
        setVisibleActivities(prev => prev + 10);
    };

    const handleDelete = () => {
        setMessage("Workspace deleted successfully!");
        setIsMessageModalOpen(true);
        setShowDeleteWorkspaceModal(false);
    };

    const handleLeave = () => {
        setMessage("Workspace left successfully!");
        setIsMessageModalOpen(true);
        setShowDeleteWorkspaceModal(false);
    };

    const handleEditClick = () => {
        setDescription(workspace.description || '');
        setIsEditing(true);
    };

    const handleInputChange = (e) => {
        setDescription(e.target.value);
    };

    const handleSaveClick = async () => {
        if (description.length < 10 || description.length > 280) {
            setErrorMessage('Workspace description must be between 10 and 280 characters.');
            return;
        }

        const updatedWorkspace = {
            WorkspaceId: workspace.workspaceId,
            Title: workspace.title,
            Description: description,
            OwnerId: workspace.ownerId,
        };

        try {
            const response = await putData('http://localhost:5157/backend/workspace/UpdateWorkspace', updatedWorkspace);
            setWorkspace(prevWorkspace => ({ ...prevWorkspace, description: description }));
            setIsEditing(false);
            setMessage("Workspace description updated successfully!");
            setIsMessageModalOpen(true);
        } catch (error) {
            console.log('Error: ', error.response?.data || error.message);
        }
    };

   //filtrimi i aktivitetit bazuar ne emrin ose mbiemrin e personit kush eka bo
    const filteredActivities = activities.filter((activity) => {
        const fullName = `${activity.userName} ${activity.userLastName}`.toLowerCase(); //search ne baz te emrit dhe mbiemrit
        return fullName.includes(searchTerm.toLowerCase());
    });

    return (
        <div className="min-h-screen h-full" style={{ backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)' }}>
            <SideMenusHeader />
            <div className="font-semibold font-sans text-gray-400 ml-20 mt-10">
                <h1 className="text-3xl">Workspace Settings</h1>
                <h1 className="text-xl mt-5">Workspace Description:</h1>
                {!isEditing ? (
                    <div>
                        <p className="text-l mt-3 mb-10">{workspace.description}</p>
                        {roli === "Owner" && (
                            <button className="text-blue-500 hover:text-blue-700" onClick={handleEditClick}>
                                Edit description
                            </button>
                        )}
                    </div>
                ) : (
                    <div>
                        <textarea 
                            type="text" 
                            value={description} 
                            onChange={handleInputChange}
                            className="text-xl mt-3 mb-10 p-2 border border-gray-500 rounded"
                        />
                        <button className="text-blue-500 hover:text-blue-700 ml-4" onClick={handleSaveClick}>
                            Save
                        </button>
                        {errorMessage && <p className="text-red-500 mt-2">{errorMessage}</p>}
                    </div>
                )}
            </div>
            <hr className="w-full border-gray-400"></hr>

            <h1 className="text-3xl mt-10 ml-20 mb-10 font-semibold font-sans text-gray-400">Workspace Activity</h1>

            {/* Search Bar */}
            <div className="ml-20 mb-6">
                <input
                    type="text"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Search by name..."
                    className="p-2 border border-gray-400 rounded w-1/3 bg-transparent"
                />
            </div>

            {/* Activity List */}
            <div className="mt-10 ml-10">
                {filteredActivities
                    .sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate))
                    .slice(0, visibleActivities)
                    .map((activity, index) => (
                        <div key={index} className="flex items-center text-gray-300 mb-4">
                            <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-m text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                {getInitials(activity.userName, activity.userLastName)}
                            </div>
                            <div className="ml-2">
                                <p><b>{activity.userName} {activity.userLastName} </b>{activity.actionType} {activity.entityName}</p>
                                <p className="text-sm text-gray-500">{formatDateTime(activity.actionDate)}</p>
                            </div>
                        </div>
                    ))
                }
                {visibleActivities < filteredActivities.length && (
                    <button
                        className="p-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                        onClick={loadMoreActivities}
                    >
                        Load More
                    </button>
                )}
            </div>
            <hr className="w-full border-gray-400 mt-5"></hr>

            {/* Modal for Deleting or Leaving the Workspace */}
            <div className="mt-10 ml-10">
                {roli === "Owner" ? (
                    <>
                        <button
                            className="p-1 text-red-700 border-none font-serif font-bold text-xl hover:text-red-600"
                            onClick={() => setShowDeleteWorkspaceModal(prev => !prev)}
                        >
                            Delete this workspace?
                        </button>
                        {showDeleteWorkspaceModal && (
                            <DeleteWorkspaceModal 
                                onClose={() => setShowDeleteWorkspaceModal(false)}
                                onDeleted={handleDelete}
                            />
                        )}
                    </>
                ) : (
                    <>
                        <button
                            className="p-1 text-red-700 border-none font-serif font-bold text-xl hover:text-red-600"
                            onClick={() => setShowDeleteWorkspaceModal(prev => !prev)}
                        >
                            Leave this workspace?
                        </button>
                        {showDeleteWorkspaceModal && (
                            <DeleteWorkspaceModal 
                                onClose={() => setShowDeleteWorkspaceModal(false)}
                                onDeleted={handleLeave}
                            />
                        )}
                    </>
                )}
                <MessageModal
                    isOpen={isMessageModalOpen}
                    message={message}
                    duration={2000}
                    onClose={() => setIsMessageModalOpen(false)}
                />
            </div>
        </div>
    );
};

export default WorkspaceSettings;
