import React, { useContext } from 'react'
import { DropdownContext } from '../Navbar/Navbar';
import { useNavigate } from 'react-router-dom';

const NavbarProfilePic = () => {

    const navigate = useNavigate();
    const dropdownContext = useContext(DropdownContext);

    const handleSignOutClick = () => {
      document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
      document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
      navigate(`/login`);
    }

    return(
        <div className="relative">

          <button
            className="flex items-center focus:outline-none relative mr-2"
            onClick={dropdownContext.toggleDropdownProfilePic}
          >
            <img
              src="https://via.placeholder.com/40"
              alt="Profile"
              className="w-8 h-8 rounded-full"
            />
          </button>

          {/* Dropdown menu */}
          {dropdownContext.ProfilePicIsOpen && (
            <div className="absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg p-">
              {/* Dropdown items */}
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                Profile
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                Settings
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700"
                      onClick={() => {handleSignOutClick()}}>
                Logout
              </button>
            </div>
          )}
        </div>
    );
}

export default NavbarProfilePic;