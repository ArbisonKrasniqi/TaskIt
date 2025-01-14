import React, { useState, useContext, useEffect } from 'react';
import { getData, postData } from '../../Services/FetchService';
import { WorkspaceContext } from '../Side/WorkspaceContext';



const InviteModal = ({ isOpen, onClose, onInviteSent  }) => {
   
    const {members,WorkspaceId, userId, getInitials, getSentInvites} = useContext(WorkspaceContext);
    const memberCount = members.length;

    const [query, setQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [selectedUsers, setSelectedUsers] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');
    const [existingInvites, setExistingInvites] = useState([]);
   
    
    useEffect(()=>{
        if(query.length>1){
            const searchUsers = async()=>{
                try{
                    const response = await getData(`http://localhost:5127/backend/user/search?query=${query}`);
                    setSearchResults(response.data);
                }
                catch(error){
                    console.error("Error searching users");
                }
            };
            searchUsers();
        }else{
            setSearchResults([]);
            setErrorMessage('');
        }
    },[query]);

    const checkPendingInviteExists = async (inviterId, inviteeId, workspaceId) => {
        try {
            const response = await getData(`http://localhost:5127/backend/invite/Check-pending-invite?inviterId=${inviterId}&inviteeId=${inviteeId}&workspaceId=${workspaceId}`);
            return response.data.exists; 
        } catch (error) {
            console.error("Error checking pending invites");
            setErrorMessage("Failed to check for existing invites. Please try again.");
            return false;
        }
    };

    const handleInvite = async () => {
        try{
           const invites = selectedUsers.map(user=> ({
            workspaceId: WorkspaceId,
            inviterId: userId,
            inviteeId: user.id,
           }));

           for(const invite of invites){

            const inviteExists = await checkPendingInviteExists(invite.inviterId, invite.inviteeId, invite.workspaceId);
            if (inviteExists) {
                console.error(`Invite already exists.`);
                setErrorMessage(`You already invited this user.`);
                continue;
            }
            const response = await postData('http://localhost:5127/backend/invite/Invite', invite);
            }
            setSelectedUsers([]);
            setErrorMessage('');
            onInviteSent();
            onClose();
            getSentInvites();
        }catch (error) {
            console.error("Error sending invites");
            setErrorMessage("Failed to send invites. Please try again.");
        }
};
const handleSelectUser =async (user) => {
    const inviteExists = await checkPendingInviteExists(userId, user.id, WorkspaceId);
   
    if (inviteExists) {
        setExistingInvites(prev => [...prev, user.id]);
        setErrorMessage(`You already invited this user.`);
        return;
    }
   
    if (selectedUsers.some(selectedUser => selectedUser.id === user.id)) {
        setSelectedUsers(selectedUsers.filter(u => u.id !== user.id)); //nese eshte i selektuar largo
        setExistingInvites(existingInvites.filter(id => id !== user.id));
        setErrorMessage('');
    } else if (selectedUsers.length < 5) {
        setSelectedUsers([...selectedUsers, user]); //nese nuk eshte i selektuar dhe ka vend selekto
        setErrorMessage('');
    }else{
        setErrorMessage('You cannot invite more than 5 people at a time.');
    }
};
    const handleRemoveUser = (user) => {
           setSelectedUsers(selectedUsers.filter(u => u !== user));
           setExistingInvites(existingInvites.filter(id => id !== user.id));
           setErrorMessage(''); 
    };

    const handleClose = ()=>{
        onClose();
        setQuery('');
        setSelectedUsers([]);
        setExistingInvites([]);
        setErrorMessage('');
    }
    return (
        <div
            className={`fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center transition-opacity ${isOpen ? 'opacity-100' : 'opacity-0 pointer-events-none'}`}
            aria-hidden={!isOpen}
        >
            { memberCount>=10 ? (
                 <div className="bg-white p-6 rounded-lg shadow-lg w-80 text-center">
                 <h2 className="text-lg font-semibold mb-4">Limit Reached</h2>
                 <p className="text-sm mb-4">
                     You cannot have more than 10 members in a workspace!
                 </p>
                 <button
                     onClick={onClose}
                     className="bg-blue-500 text-white px-4 py-2 rounded"
                 >
                     OK
                 </button>
             </div>
            )
            : (
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <h2 className="text-lg font-semibold mb-4">Invite To Workspace</h2>
                <input
                    type="text"
                    placeholder="Email or Username"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    className="w-full p-2 border border-gray-400 rounded mb-4"
                />
              {searchResults.length > 0 && (
                    <ul className="bg-white border border-gray-400 rounded mt-2 max-h-40 overflow-y-auto">
                        {searchResults.map(user => (
                            <li
                                key={user.id}
                                onClick={() => 
                                    members.some(member => member.userId === user.id) 
                                    ? null 
                                    : (selectedUsers.some(selectedUser => selectedUser.id === user.id) ? handleRemoveUser(user) : handleSelectUser(user))}
                                className={`p-2 flex flex-row items-center gap-1 cursor-pointer hover:bg-gray-200 ${selectedUsers.some(selectedUser => selectedUser.id === user.id) ? 'bg-blue-200' : ''} ${members.some(member => member.userId === user.id) ? 'opacity-50 cursor-not-allowed' : ''}`}
                           
                            >
                            <div className="flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                        {getInitials(user.firstName, user.lastName)}
                                    </div>
                                    <div className="ml-2">
                                        {user.firstName} {user.lastName} - {user.email}
                                    </div>
                            </li>
                        ))}
                    </ul>
                )}
                 {errorMessage && (
                    <div className="text-red-500 text-sm mt-2">
                        {errorMessage}
                    </div>
                 )}
                 {selectedUsers.length > 0 && (
                    <div className="mt-4">
                        <h3 className="text-md font-medium mb-2">Selected Users:</h3>
                        <ul className="bg-gray-100 border border-gray-400 rounded p-2">
                            {selectedUsers.map(user => (
                                <li key={user.id} className="p-2 flex justify-between items-center">
                                    <div className='flex flex-row items-center'>
                                    <div className="flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                        {getInitials(user.firstName, user.lastName)}
                                    </div>
                                    <div className="ml-2">
                                        {user.firstName} {user.lastName} - {user.email}
                                    </div>
                                    </div>

                                    <button
                                        onClick={() => handleRemoveUser(user)}
                                        className="text-red-500 hover:text-red-700"
                                    >
                                        x
                                    </button>
                                </li>
                            ))}
                        </ul>
                    </div>
                )}
                <div className="flex justify-end mt-4">
                    <button
                        onClick={handleClose}
                        className="bg-gray-300 text-gray-800 px-4 py-2 rounded mr-2"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={handleInvite}
                        className="bg-blue-500 text-white px-4 py-2 rounded"
                        disabled={selectedUsers.length === 0} // Disable button if no users selected
                    >
                        Send Invite
                    </button>
                </div>

            </div>
             )}
        </div>
           
    );
};

export default InviteModal;
