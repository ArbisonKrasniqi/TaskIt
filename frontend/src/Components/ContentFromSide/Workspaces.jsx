import SideMenusHeader from "./SideMenusHeader.jsx";

import { useContext, useEffect, useState } from "react";
import { getDataWithId } from "../../Services/FetchService.jsx";
import { CiViewBoard } from "react-icons/ci";
import { IoPersonOutline } from "react-icons/io5";
import { IoIosSettings } from "react-icons/io";

const Workspaces = () =>{
//const {USERID} = useContext(WorkspaceContext);
const USERID = "96dd1b34-b03b-4255-ab26-1f29f0675755";
const [OwnedWorkspaces, setOwnedWorkspaces] = useState([]);  
const [MemberWorkspaces, setMemberWorkspaces] = useState([]);  

useEffect(()=>{
    const getOwnedWorkspaces = async ()=>{
        try{
            const response = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspacesByOwnerId?ownerId', USERID);
            const data = response.data;


            if(data && Array.isArray(data) && data.length>0){
                setOwnedWorkspaces(data);
            }
            else{
                console.error("Data is null, not an array or empty ",data);
                setOwnedWorkspaces([]); // trajtojm si te dhena te zbrazeta
            }

        } catch(error){
            console.error(error.message);
            setOwnedWorkspaces([]);
        }
    };

    const getMemberWorkspaces = async ()=>{
        try{
            const responseMember = await  getDataWithId('http://localhost:5157/backend/workspace/GetWorkspacesByMemberId?memberId', USERID);
            const dataMember = responseMember.data;

            if(dataMember && Array.isArray(dataMember) && dataMember.length>0){
                setMemberWorkspaces(dataMember);
            }
            else{
                console.error("Data is null, not an array or empty ",dataMember);
                setMemberWorkspaces([]);
            }
        }
        catch(error){
            console.error(error.message);
            setMemberWorkspaces([]);
        }
    };

    getOwnedWorkspaces();
    getMemberWorkspaces();
    console.log("Owned Workspaces fetched ", OwnedWorkspaces);
    console.log("Member Workspaces fetched ", MemberWorkspaces);
}, [USERID]);



return(
<div className={`h-full`} style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
<div className="font-semibold font-sans text-gray-400 flex justify-normal  flex-col ml-20 mr-20 flex-wrap">
  <h2 className="text-2xl mt-20">Your Workspaces</h2>

  <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-10 ">
    <h2 className="text-xl mb-3">Your owned workspaces</h2>
    
    <ul>
    {OwnedWorkspaces.length === 0 ? (
          <li className='text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2'>
          <span>No orkspaces found</span>
      </li>


    ) : (
    
        OwnedWorkspaces.map(workspace =>(
            
            <li key={workspace.workspaceId}>
                <div className="flex flex-row justify-between">
                    
            <div className="flex gap-x-3 items-center mt-2 mb-2 justify-start">
           
           <button className='w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800 '>
            {workspace.title? workspace.title.charAt(0): ''}
            </button>
           
           <h1 className='w-full origin-left font-sans text-gray-400 font-bold text-xl'>{workspace.title}</h1> 
          </div>

          <div className="flex flex-wrap flex-row justify-end gap-4">
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
            <CiViewBoard/> Boards
            </button>
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2" >
            <IoPersonOutline/> Members ({workspace.members.length}) 
            </button>
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
            <IoIosSettings/> Settings
            </button>
          </div>
        </div>
           <hr className="w-full border-gray-400 mt-2"></hr>
                </li>
        )
        ))}
    </ul>
    </div>

    <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-10 ">
    <h2 className="text-xl mb-3">Workspaces you are a member of</h2>


        <ul>
        {MemberWorkspaces.length === 0 ? (
          <li className='text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2'>
          <span>No workspaces found</span>
      </li>


    ) : (
            MemberWorkspaces.map(workspace=>(

                <li key={workspace.workspaceId}>
                <div className="flex flex-row justify-between">
                    
            <div className="flex gap-x-3 items-center mt-2 mb-2 justify-start">
           
           <button className='w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800 '>
            {workspace.title? workspace.title.charAt(0): ''}
            </button>
           
           <h1 className='w-full origin-left font-sans text-gray-400 font-bold text-xl'>{workspace.title}</h1> 
          </div>

          <div className="flex flex-wrap flex-row justify-end gap-4">
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
            <CiViewBoard/> Boards
            </button>
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2" >
            <IoPersonOutline/> Members   
            </button>
            <button className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
            <IoIosSettings/> Settings
            </button>
          </div>
        </div>
           <hr className="w-full border-gray-400 mt-2"></hr>
                </li>
            )))}
        </ul>

    </div>
  </div>
  </div>



);






}
export default Workspaces