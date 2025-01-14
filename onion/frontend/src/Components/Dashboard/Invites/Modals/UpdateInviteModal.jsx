import React, { useState , useContext } from 'react';
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../InvitesTable';
import {InvitesContext} from "../InvitesList";
import CustomButton from '../../Buttons/CustomButton';
import { DashboardContext } from '../../../../Pages/dashboard';
const UpdateInviteModal = (props)=>{
    const updateContext = useContext(UpdateContext);
    const invitesContext = useContext(InvitesContext);
    const dashboardContext = useContext(DashboardContext);

    const [workspaceId, setWorkspaceId]= useState(updateContext.workspaceId);
    const [inviterId, setInviterId]= useState(updateContext.inviterId);
    const [inviteeId, setInviteeId]= useState(updateContext.inviteeId);
    const [inviteStatus, setInviteStatus]= useState(updateContext.inviteStatus);

    const handleSubmit = async (e) => {

        e.preventDefault();
    
        try{
            const data = {
                inviteId: updateContext.inviteId, //nuk mundet mu ndryshu id
                //te dhenat e tjera i ndryshojme
                workspaceId: workspaceId,
                inviterId: inviterId,
                inviteeId: inviteeId,
                inviteStatus: inviteStatus
            };
            const response = await putData('http://localhost:5127/backend/invite/UpdateInvite', data);
            console.log(response.data);
            const updatedInvites = invitesContext.invites.map(invite => {
                if(invite.inviteId ===updateContext.inviteId){
                    return{
                        ...invite,
                        workspaceId: workspaceId,
                        inviterId: inviterId,
                        inviteeId: inviteeId,
                        inviteStatus: inviteStatus
                    };
                }else{
                    return invite; 
                }
            });

            invitesContext.setInvites(updatedInvites);
            props.setShowUpdateInfoModal(false);
        }catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            invitesContext.getInvites();
            props.setShowUpdateInfoModal(false);
        }
    }
    
    
    return(   <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
        <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
            <div className="grid md:grid-cols-2 md:gap-6">
                <div className="mb-6 col-span-2">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Inviter Id</label>
                    <input value={inviterId}
                           onChange={(e) => setInviterId(e.target.value)}
                           type="text"
                           id="inviterId"
                           className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                </div>
        
                 <div className="mb-6 col-span-2">
                     <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Invitee Id</label>
                    <input value={inviteeId}
                           onChange={(e) => setInviteeId(e.target.value)}
                           type="text"
                           id="inviteeId"
                           className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
    </div>

                <div className="mb-6">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Invite Status</label>
                    <input value={inviteStatus}
                           onChange={(e) => setInviteStatus(e.target.value)}
                           type="text"
                           id="inviteStatus"
                           className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                </div>
            </div>
            <div className="flex justify-around">
                {/*Nese shtypet butoni close, atehere mbyll modal duke vendosur vleren false*/}
                <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed"/>
                <CustomButton type="submit" text="Update" color="longGreen"/>
            </div>
        </form>
    </div>
);

}
export default UpdateInviteModal