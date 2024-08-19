import { useContext, createContext } from "react";
import UpdateInviteButton from './Buttons/UpdateInviteButton';
import {InvitesContext} from './InvitesList';
import CustomButton from "../Buttons/CustomButton";
import { deleteData } from "../../../Services/FetchService";


export const UpdateContext = createContext();

const InvitesTable = ()=>{
    const invitesContext = useContext(InvitesContext);
    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        return isNaN(date.getTime()) ? 'Date not available' : date.toLocaleDateString();
    };
    const HandleInviteDelete = (id)=>{
        async function deleteInvite(){
            try{
                const data={
                    InviteId: id
                };
                const response = await deleteData('/backend/invite/DeleteInviteById', data);
                console.log(response.data);

                const updatedInvites = invitesContext.invites.filter(invite=> invite.inviteId !==id);
                invitesContext.setInvites(updatedInvites);
            }
            catch(error){
                invitesContext.setErrorMessage(error.message+ id);
                invitesContext.setShowInvitesErrorModal(true);
                invitesContext.getInvites();
            }
        }
        deleteInvite();
    }
    return(
        <div className="overflow-x-auto">
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
           <tr>
            
           <th className="px-6 py-3" >ID</th>
           <th className="px-6 py-3" >Workspace Id</th>
           <th className="px-6 py-3" >Inviter Id</th>
           <th className="px-6 py-3" >Invitee Id</th>       
           <th className="px-6 py-3" >Invite Status</th>
           <th className="px-6 py-3" >Date Sent</th>
           <th className="px-6 py-3" >Actions</th>
        </tr>     
        </thead>  

        <tbody>
        {invitesContext.invites? (invitesContext.invites.map((invite, index)=>(
            <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                  <td className="px-6 py-4">{invite.inviteId}</td>
                  <td className="px-6 py-4">{invite.workspaceId}</td>
                  <td className="px-6 py-4">{invite.inviterId}</td>
                  <td className="px-6 py-4">{invite.inviteeId}</td>
                  <td className="px-6 py-4">{invite.inviteStatus}</td>
                  <td className="px-6 py-4">{formatDate(invite.dateSent)}</td>
                  <td className="px-6 py-4"> <UpdateContext.Provider value={invite}> <UpdateInviteButton/>  </UpdateContext.Provider> <CustomButton color="red" text="Delete" onClick={()=>{ HandleInviteDelete(invite.inviteId)}}/> </td>
       
            </tr>
        ))) : (
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
export default InvitesTable