import { useContext, useEffect, useState } from "react";
import { CiViewBoard } from "react-icons/ci";
import { IoPersonOutline } from "react-icons/io5";
import { IoIosSettings } from "react-icons/io";
import { WorkspaceContext } from "../Side/WorkspaceContext.jsx";
import { useNavigate } from "react-router-dom";

const Workspaces = () => {
    const { userId, workspaces } = useContext(WorkspaceContext);
    const [OwnedWorkspaces, setOwnedWorkspaces] = useState([]);  
    const [MemberWorkspaces, setMemberWorkspaces] = useState([]);  
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        if (userId && workspaces) {
            const ownedWorkspaces = workspaces.filter(workspace => workspace.ownerId === userId);
            const memberWorkspaces = workspaces.filter(workspace => workspace.ownerId !== userId);
        
            setOwnedWorkspaces(ownedWorkspaces);
            setMemberWorkspaces(memberWorkspaces);
        }
    }, [userId, workspaces]);

    const filteredOwnedWorkspaces = OwnedWorkspaces.filter(workspace =>
        workspace.title.toLowerCase().includes(searchTerm.toLowerCase())
    );
    
    const filteredMemberWorkspaces = MemberWorkspaces.filter(workspace =>
        workspace.title.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="min-h-screen h-full" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
            <div className="font-semibold font-sans text-gray-400 flex justify-normal flex-col ml-20 mr-20 flex-wrap">
                <h2 className="text-2xl mt-20">Your Workspaces</h2>

                <div className="flex flex-col mt-5">
                    <label htmlFor="searchWorkspace">Search</label>
                    <div className="bg-transparent border border-solid border-gray-500 flex flex-row mt-2 rounded-md w-full md:w-2/3 lg:w-1/4 xl:w-1/4">
                        <input 
                            type="search" 
                            id="searchWorkspace" 
                            name="searchWorkspace" 
                            placeholder="Search workspaces..."
                            className="p-2 border-none bg-transparent focus:outline-none w-full text-sm md:text-base" 
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    </div>
                </div>

                <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-10">
                    <h2 className="text-xl mb-3">Your owned workspaces</h2>
                    <ul>
                        {filteredOwnedWorkspaces.length === 0 ? (
                            <li className='text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2'>
                                <span>No workspaces found</span>
                            </li>
                        ) : (
                            filteredOwnedWorkspaces.map(workspace => (
                                <li key={workspace.workspaceId}>
                                    <div className="flex flex-row justify-between">
                                        <div className="flex gap-x-3 items-center mt-2 mb-2 justify-start"
                                            onClick={() => navigate(`/main/boards/${workspace.workspaceId}`)}>
                                            <button className='w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800'>
                                                {workspace.title ? workspace.title.charAt(0) : ''}
                                            </button>
                                            <h1 className='w-full origin-left font-sans text-gray-400 font-bold text-xl'>{workspace.title}</h1>
                                        </div>
                                        <div className="flex flex-wrap flex-row justify-end gap-4">
                                            <button onClick={() => navigate(`/main/boards/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <CiViewBoard /> Boards
                                            </button>
                                            <button onClick={() => navigate(`/main/members/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <IoPersonOutline /> Members ({workspace.members?.length})
                                            </button>
                                            <button onClick={() => navigate(`/main/workspaceSettings/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <IoIosSettings /> Settings
                                            </button>
                                        </div>
                                    </div>
                                    <hr className="w-full border-gray-400 mt-2"></hr>
                                </li>
                            ))
                        )}
                    </ul>
                </div>

                <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-10">
                    <h2 className="text-xl mb-3">Workspaces you are a member of</h2>
                    <ul>
                        {filteredMemberWorkspaces.length === 0 ? (
                            <li className='text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2'>
                                <span>No workspaces found</span>
                            </li>
                        ) : (
                            filteredMemberWorkspaces.map(workspace => (
                                <li key={workspace.workspaceId}>
                                    <div className="flex flex-row justify-between">
                                        <div className="flex gap-x-3 items-center mt-2 mb-2 justify-start"
                                            onClick={() => navigate(`/main/boards/${workspace.workspaceId}`)}>
                                            <button className='w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800'>
                                                {workspace.title ? workspace.title.charAt(0) : ''}
                                            </button>
                                            <h1 className='w-full origin-left font-sans text-gray-400 font-bold text-xl'>{workspace.title}</h1>
                                        </div>
                                        <div className="flex flex-wrap flex-row justify-end gap-4">
                                            <button onClick={() => navigate(`/main/boards/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <CiViewBoard /> Boards
                                            </button>
                                            <button onClick={() => navigate(`/main/members/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <IoPersonOutline /> Members ({workspace.members?.length})
                                            </button>
                                            <button onClick={() => navigate(`/main/workspaceSettings/${workspace.workspaceId}`)}
                                                className="flex flex-row items-center border border-gray-400 hover:bg-gray-600 mb-0 gap-x-0.5 px-1 rounded-md mt-2">
                                                <IoIosSettings /> Settings
                                            </button>
                                        </div>
                                    </div>
                                    <hr className="w-full border-gray-400 mt-2"></hr>
                                </li>
                            ))
                        )}
                    </ul>
                </div>
            </div>
        </div>
    );
}

export default Workspaces;