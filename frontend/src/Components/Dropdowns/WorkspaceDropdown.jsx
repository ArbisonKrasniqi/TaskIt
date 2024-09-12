import React, { useContext, useRef } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from 'react-icons/io';
import { DropdownContext } from '../Navbar/Navbar';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useNavigate } from 'react-router-dom';

const WorkspaceDropdown = (props) => {
  const navigate = useNavigate();
  const { workspaces } = useContext(WorkspaceContext);
  const dropdownContext = useContext(DropdownContext);
  const dropdownRef = useRef(null);

  const handleWorkspaceClick = (workspaceId) => {
    navigate(`/main/boards/${workspaceId}`);
  };

  const handleClickOutside = (event) => {
    if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
      dropdownContext.toggleDropdownWorkspace();
    }
  };

  React.useEffect(() => {
    if (dropdownContext.WorkspaceDropdownIsOpen) {
      document.addEventListener('mousedown', handleClickOutside);
    } else {
      document.removeEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [dropdownContext.WorkspaceDropdownIsOpen]);

  return (
    <div className={`relative ${props.width <= 880 && 'hidden'}`} ref={dropdownRef}>

      <button
        onClick={dropdownContext.toggleDropdownWorkspace}
        className={`bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center 
        ${dropdownContext.WorkspaceDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}
      >
        Workspaces <span className='h-[14px] mx-2'>{dropdownContext.WorkspaceDropdownIsOpen ? <IoIosArrowUp /> : <IoIosArrowDown />}</span>
      </button>

      {dropdownContext.WorkspaceDropdownIsOpen && (
        <>
          {/* Overlay */}
          <div
            className="fixed inset-0 z-10 bg-black opacity-25"
            onClick={() => dropdownContext.toggleDropdownWorkspace()}
          ></div>

          <div className={`${props.width > 880 ? 'absolute left-0 z-20 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg' : 'absolute left-[155px] top-[-7px] z-20 w-48 bg-gray-800 rounded-lg p-2 shadow-lg'}`}>
            {workspaces.length > 0 ? (
              workspaces.map((workspace) => (
                <button
                  key={workspace.workspaceId}
                  className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'
                  onClick={() => handleWorkspaceClick(workspace.workspaceId)}
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
        </>
      )}
    </div>
  );
};

export default WorkspaceDropdown;
