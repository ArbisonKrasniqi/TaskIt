import React, { useContext, useEffect } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from 'react-icons/io';
import { DropdownContext } from '../Navbar/Navbar';
import { getDataWithId } from '../../Services/FetchService';
import { WorkspaceContext } from '../Side/WorkspaceContext';

const WorkspaceDropdown = (props) => {

  const { workspaces, setWorkspaces} = useContext(WorkspaceContext);
  
    const dropdownContext = useContext(DropdownContext)


    useEffect (() => {
      const getWorkspaces = async () => {
        try {
          
          const response = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspacesByMemberId?memberId', 'asdajsdanlkdjad'); //static id for now
          const data = response.data;

          console.log('Fetched data: ', data);

          if (data && Array.isArray(data) && data.length > 0) {
            setWorkspaces(data);
          }
          else{
            console.log('Data is null, not an array, or empty:', data);
            setWorkspaces([]);
          }
        } catch (error) {
          setWorkspaces([]);
          
        }
      };
      getWorkspaces();
      console.log(workspaces);
    });

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
                  key={workspace.id}
                  className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'
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
