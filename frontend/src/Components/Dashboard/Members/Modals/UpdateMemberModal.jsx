import React, { useState , useContext } from 'react';
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../MembersTable';
import {MembersContext} from "../MembersList";
import CustomButton from '../../Buttons/CustomButton';
const UpdateMemberModal = (props)=>{
    const updateContext = useContext(UpdateContext);
    const membersContext = useContext(MembersContext);

    const [userId, setUserId]= useState(updateContext.userId);
    const [dateJoined, setDateJoined]= useState(updateContext.dateJoined);
    const [workspaceId, setWorkspaceId]= useState(updateContext.workspaceId);

    const handleSubmit = async (e) => {

        e.preventDefault();
    
        try{
            const data = {
                memberId: updateContext.memberId, //nuk mundet mu ndryshu id
                //te dhenat e tjera i ndryshojme
                userId: userId,
                dateJoined: dateJoined,
                workspaceId: workspaceId
            };
            const response = await putData('http://localhost:5157/backend/Members/UpdateMember', data);
            console.log(response.data);
            const updatedMembers = membersContext.members.map(member => {
                if(member.memberId ===updateContext.memberId){
                    return{
                        ...member,
                        userId: userId,
                        dateJoined: dateJoined,
                        workspaceId: workspaceId
                    };
                }else{
                    return member; 
                }
            });

            membersContext.setMembers(updatedMembers);
            props.setShowUpdateInfoModal(false);
        }catch(error){
            membersContext.setErrorMessage(error.message);
            membersContext.setShowMembersErrorModal(true);
            membersContext.getMembers();
            props.setShowUpdateInfoModal(false);
        }
    }
    
    return(   <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
        <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
            <div className="mb-6">
                <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">User Id</label>
                <input value={userId}
                       onChange={(e) => setUserId(e.target.value)}
                       type="text"
                       id="userId"
                       className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
            </div>
            <div className="grid md:grid-cols-2 md:gap-6">
                <div className="mb-6">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Workspace Id</label>
                    <input value={workspaceId}
                           onChange={(e) => setWorkspaceId(e.target.value)}
                           type="text"
                           id="workspaceId"
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
export default UpdateMemberModal