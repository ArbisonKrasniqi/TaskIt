import React, { useContext } from 'react'
import { DropdownContext } from '../Navbar/Navbar';
import { MainContext } from '../../Pages/MainContext';
import { WorkspaceContext } from '../Side/WorkspaceContext';
const NavbarProfilePic = () => {

    const dropdownContext = useContext(DropdownContext);
    const {userInfo} = useContext(MainContext);
    const {getInitialsFromFullName} = useContext(WorkspaceContext);
    const name = userInfo.name;
    const email = userInfo.email;

    return(
        <div className="relative">

          <button
            className="flex items-center focus:outline-none relative mr-2"
            onClick={dropdownContext.toggleDropdownProfilePic}
          >
           <div className="flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-white text-sm bg-gradient-to-r from-orange-400 to-orange-600">
             {getInitialsFromFullName(name)}
           </div>

          </button>

          {/* Dropdown menu */}
          {dropdownContext.ProfilePicIsOpen && (
            <div className="absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg p-">

              <div className='flex items-start p-1 bg-gray-800 rounded-lg text-gray-400 hover:bg-gray-700 mb-2 hover:cursor-pointer'>
           <div className="flex-shrink-0 w-8 h-8 ml-1 mt-1 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
             {getInitialsFromFullName(name)}
           </div>
            <div className="ml-2 flex-1">
             <div className="text-Ms font-medium text-gray-200">
               {name}
            </div>
             <div className="text-xs text-gray-400">
            {email}
            </div>
            </div>
           </div>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                Profile
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                Settings
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                Logout
              </button>
            </div>
          )}
        </div>
    );
}

export default NavbarProfilePic;