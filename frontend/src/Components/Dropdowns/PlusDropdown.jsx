import React, { useContext, useState } from 'react'
import { FaPlus } from "react-icons/fa6";
import { DropdownContext } from '../Navbar/Navbar';
import CreateBoardModal from '../Side/CreateBoardModal';
import CreateWorkspaceModal from '../Side/CreateWorkspaceModal';
import { WorkspaceContext } from '../Side/WorkspaceContext.jsx';

const PlusDropdown = (props) => {

    const dropdownContext = useContext(DropdownContext);
    const { handleCreateBoard } = useContext(WorkspaceContext);

    const [ setWorkspaces] = useState([]);
    const [openBoardModal, setOpenBoardModal] = useState(false);
    const [openWorkspaceModal, setOpenWorkspaceModal] = useState(false);

  

    const handleCreateWorkspace = (newWorkspace) => {
        setWorkspaces((prevWorkspaces) => [...prevWorkspaces, newWorkspace]);
    }

    return(
        <div className={`relative ${props.width>1200 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownPlus}
                className={`bg-blue-400 text-gray-800 text-xl p-2 ml-2 rounded focus:outline-none hover:bg-blue-600 flex items-center text-center  
                ${dropdownContext.PlusDropdownIsOpen ? 'bg-blue-600' : 'bg-blue-400'}`}
                >
                    <FaPlus/>
                </button>

                {dropdownContext.CreateDropdownIsOpen && (
                <div className='absolute left-0 z-10 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg p-3'>
                   <button onClick={()=>setOpenBoardModal(true)} className={`block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700 `}>
                        Create Boards
                    </button>
                    <CreateBoardModal open={openBoardModal} onClose={()=> setOpenBoardModal(false)} onBoardCreated={handleCreateBoard}></CreateBoardModal>

                    <button onClick={()=>setOpenWorkspaceModal(true)} className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Create Workspaces
                    </button>
                    <CreateWorkspaceModal open={openWorkspaceModal} onClose={() => setOpenWorkspaceModal(false)} onWorkspaceCreated={handleCreateWorkspace} ></CreateWorkspaceModal>
                </div>
            )}
        </div>
    );
}

export default PlusDropdown;