import React, { useContext, useEffect, useState } from 'react';
import { MdNotificationsNone } from "react-icons/md";
import { DropdownContext } from '../Navbar';
import { getDataWithId, putData } from '../../../Services/FetchService';
import { WorkspaceContext } from '../../Side/WorkspaceContext';
import { useNavigate } from 'react-router-dom';
const NotificationButton = (props) => {
    const dropdownContext = useContext(DropdownContext);
    const { userId } = useContext(WorkspaceContext);
    const [invites, setInvites] = useState([]);
    const [inviterDetails, setInviterDetails] = useState([]);
    const [workspaceTitles, setWorkspaceTitles] = useState([]);
    const navigate = useNavigate();
    const getInitials = (firstName, lastName) => {
        if (!firstName || !lastName) {
            return '';
        }
        return `${firstName.charAt(0)}${lastName.charAt(0)}`;
    };
    
    const getInvites = async () => {
        try {
            const response = await getDataWithId('http://localhost:5157/backend/invite/GetPendingInvitesByInvitee?inviteeId', userId);
            const data = response.data;
            console.log("Invites fetched: ", data);

            setInvites(data);

            // Fetch inviter details for each invite
            const inviters = await Promise.all(data.map(async invite => {
                const responseInviter = await getDataWithId('http://localhost:5157/backend/user/adminUserID?userId', invite.inviterId);
                return responseInviter.data;
            }));
            const workspaceTitlesData = await Promise.all(data.map(async invite => {
                const responseWorkspace = await getDataWithId('http://localhost:5157/backend/workspace/getWorkspaceById?workspaceId', invite.workspaceId);
                return responseWorkspace.data.title; // Assuming the workspace object has a 'title' field
            }));

            setInviterDetails(inviters);
            setWorkspaceTitles(workspaceTitlesData);
        } catch (error) {
            console.log("Error fetching invites: ", error.message);
        }
    };

    useEffect(() => {
        if (dropdownContext.NotificationDropdownIsOpen) {
            getInvites();
        }
    }, [dropdownContext.NotificationDropdownIsOpen]);

    const handleInviteAction = async(inviteId, status,workspaceId)=>{
        try{
            const updateDto = {
                inviteId: inviteId,
                inviteStatus: status
            };

            const response = await putData("http://localhost:5157/backend/invite/UpdateInviteStatus", updateDto);
            console.log("Invite status updated ",response.data);
            getInvites();
            if(status === "Accepted"){
                navigate(`/boards/${workspaceId}`);
            }
        }catch(error){
            console.error("Error updating invite status: ", error.message);
        }
   };

    return (
        <div className='relative'>
            <button
                className={`flex items-center justify-center text-gray-400 focus:outline-none w-8 h-8 mx-2 rounded-full  hover:bg-gray-700 
                ${dropdownContext.NotificationDropdownIsOpen && 'rotate-45'}`}
                onClick={dropdownContext.toggleDropdownNotification}
            >
                <MdNotificationsNone className='w-6 h-auto' />
                {invites.length > 0 && (
                    <span className="absolute top-0 right-0 inline-flex items-center justify-center w-4 h-4 text-xs font-bold text-white bg-red-500 rounded-full">
                        {invites.length}
                    </span>
                )}
            </button>

            
            {dropdownContext.NotificationDropdownIsOpen && (

                <>
                                {/* Overlay */}
                        <div 
                        className="fixed inset-0 z-10"
                        onClick={dropdownContext.toggleDropdownNotification}
                        ></div>


                <div className="absolute right-0 mt-2 w-80 bg-gray-800 rounded-lg shadow-lg z-20">
                    {invites.length > 0 ? (
                        invites.map((invite, index) => (
                            <div key={invite.inviteId} className='flex items-start p-4 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700 mb-2'>
                                <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                    {getInitials(inviterDetails[index]?.firstName, inviterDetails[index]?.lastName)}
                                </div>
                                <div className="ml-4 flex-1">
                                    <div className="text-sm font-medium text-gray-200">
                                        {inviterDetails[index]?.firstName} {inviterDetails[index]?.lastName}
                                    </div>
                                    <div className="text-xs text-gray-400">
                                        {inviterDetails[index]?.email}
                                    </div>
                                    <p className="text-sm mt-2">
                                        invited you to join the workspace  <span className="font-bold"> {workspaceTitles[index]}</span>.
                                    </p>
                                    <div className="mt-3 flex space-x-2">
                                        <button
                                            onClick={() => handleInviteAction(invite.inviteId, "Accepted", invite.workspaceId)}
                                            className="text-sm bg-green-500 text-white py-1 px-3 rounded-lg hover:bg-green-600"
                                        >
                                            Accept
                                        </button>
                                        <button
                                            onClick={() => handleInviteAction(invite.inviteId, "Declined")}
                                            className="text-sm bg-red-500 text-white py-1 px-3 rounded-lg hover:bg-red-600"
                                        >
                                            Decline
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg">
                            No new invites
                        </p>
                    )}
                </div>
                </>
            )}
        </div>
        
    );
};

export default NotificationButton;
