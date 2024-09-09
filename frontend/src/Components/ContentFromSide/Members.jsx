import React,{ useContext, useState, useEffect } from "react";
import SideMenusHeader from "./SideMenusHeader";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { getDataWithId, deleteData } from "../../Services/FetchService";

const Members = () => {

        const { members, memberDetails, getInitials, roli, sentInvites,inviteeDetails, workspaceTitles, handleDeleteInvite, workspace, handleRemoveMember } = useContext(WorkspaceContext);
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
        const [searchTerm, setSearchTerm] = useState("");

        console.log("MEMBERS ",members);
        console.log("Member DETAILSS ",memberDetails)

        // Map members with details duke perdorur userId
        const filteredMembers = members.map((member) => {
            const memberDetail = memberDetails.find((detail) => detail.id === member.userId);
            if (!memberDetail) return null; 
            const fullName = `${memberDetail.firstName || ''} ${memberDetail.lastName || ''}`.toLowerCase();
            return fullName.includes(searchTerm.toLowerCase()) ? { ...member, memberDetail } : null;
        }).filter(Boolean); // Remove any null values from the array
    

    return(
        <div className="min-h-screen h-full" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
           <SideMenusHeader></SideMenusHeader>
            <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-20 mr-20 flex-wrap">
                <h2 className="text-2xl ">Collaborators {members.length}/10</h2>
                <div className='flex flex-wrap  mt-4 justify-between'>
                    
                    <div className='flex-wrap flex-col w-[290px] p-4 sm:my-[30px]'>
                        <button className=' text-blue-500 text-lg bg-blue-900 rounded-[7px]   text-start px-4 py-2 '>
                            Workspace members ({members.length}/10)
                        </button> 

                        {roli === "Owner" ? ( 
                            <div className="border border-gray-400 mt-10 p-2 rounded-md">
                          <h2 className="h-8 font-bold text-lg">Pending Invites</h2>
                          <div className="mt-4">
                            {sentInvites.length === 0 ? (
                                <p>No pending invites.</p>
                            ) : (
                                sentInvites.map((invite, index) => (
                                    <div key={invite.inviteId} className='flex items-start p-4 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700 mb-2'>
                                    <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                        {getInitials(inviteeDetails[index]?.firstName, inviteeDetails[index]?.lastName)}
                                    </div>
                                    <div className="ml-4 flex-1">
                                        <div className="text-sm font-medium text-gray-200">
                                            {inviteeDetails[index]?.firstName} {inviteeDetails[index]?.lastName}
                                        </div>
                                        <div className="text-xs text-gray-400">
                                            {inviteeDetails[index]?.email}
                                        </div>
                                        <p className="text-sm mt-2">
                                           <strong>Workspace: </strong>  <span> {workspaceTitles[index]}</span>.
                                           <br/>
                                           <span>{formatDateTime(invite.dateSent)}</span>
                                        </p>
                                        <button
                                            className="bg-red-500 text-white px-4 py-2 rounded-md"
                                            onClick={() => handleDeleteInvite(invite.inviteId)}
                                        >
                                            Delete Invite
                                        </button>
                                    </div>
                                    </div>
                                ))
                            )}
                        </div>
                          </div> 
                        )
                          : <> </> 
                        }
                       
                    </div>

                    <div className='w-2/3 p-4'>
                        <div> 
                            <h2 className=' h-8 font-bold text-lg  '>Workspace members ({members.length})</h2>
                            <p className='m-3'>Workspace members can vew and join all Workspace visible boards and create new boards in the Workspace.</p>
                            <hr className='border-gray-400'/>
                            <br />
                            <h2 className='h-8 font-bold text-lg'>Invite members to join you</h2>
                            <p className='m-3'>Inviting others to your workspace enhances productivity and collaboration. 
                                Working together, your team can share ideas, manage projects, and achieve goals faster. 
                                Collaboration boosts transparency and coordination, ensuring everyone is on the same page.</p>
                        </div>
                        <hr className='border-gray-400'/>
                        <br />
                    

                        <div>
                        <input 
                            className='rounded-md border border-gray-400 p-2 bg-gray-800' 
                            type="text"
                            placeholder='Filter by name' 
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)} 
                        />
                        </div>
                        <br />
                        <hr className='border-gray-400'/>
                        <div>
                        <table className='w-full'>
                                <tbody>
                                    {filteredMembers.map((member, index) => (
                                        <>
                                            <tr key={member.memberId} className='h-14'>
                                                <td className='w-10'>
                                                    <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                                        {getInitials(member.memberDetail?.firstName, member.memberDetail?.lastName)}
                                                    </div>
                                                </td>
                                                <td className='pl-3'>{member.memberDetail?.firstName} {member.memberDetail?.lastName}</td>
                                                <td>Role: {(member.memberDetail?.id === workspace.ownerId) ? "Owner" : "Member"}</td>
                                                <td>
                                                    <button
                                                        className={(member.memberDetail?.id === workspace.ownerId) ? 'bg-gray-500 text-gray-300 cursor-not-allowed px-4 py-2 rounded-md' : 'bg-red-500 text-white px-4 py-2 rounded-md'}
                                                        onClick={() => 
                                                            member.memberDetail?.id !== workspace.ownerId &&
                                                            handleRemoveMember(member.memberDetail?.id, workspace.workspaceId)}
                                                    >
                                                        Remove
                                                    </button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colSpan={4}>
                                                    <hr className='border-gray-600' />
                                                </td>
                                            </tr>
                                        </>
                                    ))}
                                </tbody>
                            </table>


                            
                        </div>
                    
                    </div>
                </div>
        
            </div>
        </div>
    );
}

export default Members;