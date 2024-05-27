import React, { useContext } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from 'react-icons/io';
import { DropdownContext } from '../Navbar/Navbar';

const WorkspaceDropdown = (props) => {

    const dropdownContext = useContext(DropdownContext)

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
        <div className={`${props.width>880 ? 'absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg' : 'absolute left-[155px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg' }`}>
          <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
            TaskIt Workspace
          </button>
          
        </div>
      )}
    </div>
  );
};

export default WorkspaceDropdown;
