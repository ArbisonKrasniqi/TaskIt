import React, { useContext, useEffect } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from 'react-icons/io';
import { DropdownContext } from '../Navbar/Navbar';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useNavigate } from 'react-router-dom';

const WorkspaceDropdown = (props) => {
  const navigate = useNavigate();
  const { workspaces } = useContext(WorkspaceContext);
  const dropdownContext = useContext(DropdownContext);

  const handleWorkspaceClick = (workspaceId) => {
      navigate(`/main/boards/${workspaceId}`);
  }
  return (
    <div className={`relative ${props.width <= 880 && 'hidden'}`}>
      <button
        onClick={dropdownContext.toggleDropdownWorkspace}
        className={`bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center 
        ${dropdownContext.WorkspaceDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}
      >
        Workspaces <span className='h-[14px] mx-2'>{dropdownContext.WorkspaceDropdownIsOpen ? <IoIosArrowUp /> : <IoIosArrowDown />}</span>
      </button>
      {dropdownContext.WorkspaceDropdownIsOpen && (
        <div className={`${props.width > 880 ? 'absolute left-0 z-10 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg' : 'absolute left-[155px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg'}`}>
          {workspaces.length > 0 ? (
            workspaces.map((workspace) => (
              <button
                key={workspace.workspaceId}
                className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'
                onClick={ () => {handleWorkspaceClick(workspace.workspaceId)}}
              >
                {workspace.title}
              </button>
            ))
          ) : (
            <div className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg'>
              No workspaces available
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default WorkspaceDropdown;
