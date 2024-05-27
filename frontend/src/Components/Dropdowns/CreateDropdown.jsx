import React, { useContext } from 'react';
import { DropdownContext } from '../Navbar/Navbar';

const CreateDropdown = (props) => {

    const dropdownContext = useContext(DropdownContext);

    return (
        <div className='relative'>
            <button
                onClick={dropdownContext.toggleDropdownCreate}
                className={` px-4 py-2 rounded focus:outline-none hover:bg-blue-300 flex items-center 
                ${dropdownContext.CreateDropdownIsOpen ? 'text-blue-400 font-bold bg-blue-600 bg-opacity-20 hover:bg-blue-900 hover:bg-opacity-80' : 'text-gray-800 font-bold bg-blue-400'}`}>
                Create
            </button>

            {dropdownContext.CreateDropdownIsOpen && (
                <div className='absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg p-3'>
                   <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Create Boards
                    </button>
                    <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Create Workspaces
                    </button>
                </div>
            )}
        </div>
    );
}

export default CreateDropdown;
