import React,{ useContext } from "react";
import SideMenusHeader from "./SideMenusHeader";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const Members = () => {

    const { members } = useContext(WorkspaceContext);

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
                            <input className=' rounded-md border border-gray-400 p-2 bg-gray-800 ' 
                            type="text"
                            placeholder='Filter by name' />
                        </div>
                        <br />
                        <hr className='border-gray-400'/>
                        <div>
                            <table className='w-full'>
                                <tbody>
                                    {members.map((member, index) => (
                                        <>
                                            <tr key={index} className='h-10'>
                                                <td className='w-10'>
                                                    <img
                                                        src="https://via.placeholder.com/40"
                                                        alt="Profile"
                                                        className="w-8 h-8 rounded-full"
                                                    />
                                                </td>
                                                <td className='pl-3'>{member.firstName} {member.lastName}</td>
                                                <td>Role: "Admin"</td>
                                                <td className='px-3 w-6'>Remove</td>
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