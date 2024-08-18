import React, {useContext, createContext} from 'react';
import {MembersContext} from "./MembersList"
import CustomButton from '../Buttons/CustomButton';
import { deleteData } from '../../../Services/FetchService';
import UpdateMemberButton from "./Buttons/UpdateMemberButton";

export const UpdateContext = createContext();

const MembersTable = () =>{
    const membersContext = useContext(MembersContext);
    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        return isNaN(date.getTime()) ? 'Date not available' : date.toLocaleDateString();
    };
    const HandleMemberDelete = (id)=>{
        async function deleteMember () {
            try{
                const data = {
                    MemberId: id
                };
                const response = await deleteData('/backend/Members/DeleteMember', data);
                console.log(response);
                //marrim listen e members dhe e largojna nga lista kete member
                const updatedMembers = membersContext.members.filter(member => member.memberId !==id);
                membersContext.setMembers(updatedMembers);
            }
            catch(error){
                membersContext.setErrorMessage(error.message + id);
                membersContext.setShowMembersErrorModal(true);
                membersContext.getMembers(); //per me perditsu listen
            }
        }
        deleteMember();
    }

    return(
        <div className="overflow-x-auto">
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
           <tr>
            
           <th className="px-6 py-3" >ID</th>
           <th className="px-6 py-3" >User Id</th>
           <th className="px-6 py-3" >Date Joined</th>
          {/*  <th className="px-6 py-3" >User</th>     */}
           <th className="px-6 py-3" >Workspace Id</th>       
           <th className="px-6 py-3" >Actions</th>
        </tr>     
        </thead>  

        <tbody>
        {membersContext.members? (membersContext.members.map((member, index)=>(
            <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                  <td className="px-6 py-4">{member.memberId}</td>
                  <td className="px-6 py-4">{member.userId}</td>
                  <td className="px-6 py-4">{formatDate(member.dateJoined)}</td>
                  <td className="px-6 py-4">{member.workspaceId}</td>
                  <td className="px-6 py-4"> <UpdateContext.Provider value={member}> <UpdateMemberButton/>  </UpdateContext.Provider> <CustomButton color="red" text="Delete" onClick={()=>{ HandleMemberDelete(member.memberId)}}/> </td>
       
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
export default MembersTable