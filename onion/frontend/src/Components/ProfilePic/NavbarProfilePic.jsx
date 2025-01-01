import React, { useContext, useRef, useEffect } from 'react';
import { DropdownContext } from '../Navbar/Navbar';
import { MainContext } from '../../Pages/MainContext';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useNavigate } from 'react-router-dom';
import { CiLogout } from "react-icons/ci";

const NavbarProfilePic = () => {
  const navigate = useNavigate();
  const dropdownContext = useContext(DropdownContext);
  const { ProfilePicIsOpen, toggleDropdownProfilePic } = useContext(DropdownContext);
  const { userInfo } = useContext(MainContext);
  const { getInitialsFromFullName } = useContext(WorkspaceContext);
  const name = userInfo.name;
  const email = userInfo.email;

  const dropdownRef = useRef(null);

  const handleSignOutClick = () => {
    document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
    document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
    navigate(`/login`);
  };

  const handleClickOutside = (event) => {
    if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
      toggleDropdownProfilePic(); // Close the dropdown if clicked outside
    }
  };

  useEffect(() => {
    if (ProfilePicIsOpen) {
      document.addEventListener('mousedown', handleClickOutside);
    } else {
      document.removeEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [ProfilePicIsOpen]);

  return (
    <div className="relative">
      <button
        className="flex items-center focus:outline-none relative mr-2"
        onClick={(e) => {
          e.stopPropagation();
          toggleDropdownProfilePic();
        }}
      >
        <div className="flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center text-white text-sm bg-gradient-to-r from-orange-400 to-orange-600">
          {getInitialsFromFullName(name)}
        </div>
      </button>

      {ProfilePicIsOpen && (
        <div ref={dropdownRef} className="absolute z-50 right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg p-2">
          {/* Dropdown content */}
          <div
            onClick={() => {
              dropdownContext.toggleDropdownProfilePic();
              navigate(`/main/profile`);
            }}
            className="flex items-start p-1 bg-gray-800 rounded-lg text-gray-400 hover:bg-gray-700 mb-2 hover:cursor-pointer"
          >
            <div className="flex-shrink-0 w-8 h-8 ml-1 mt-1 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
              {getInitialsFromFullName(name)}
            </div>
            <div className="ml-2 flex-1">
              <div className="text-Ms font-medium text-gray-200">{name}</div>
              <div className="text-xs text-gray-400">{email}</div>
            </div>
          </div>

          <button
            className="w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700 flex flex-row items-center"
            onClick={handleSignOutClick}
          >
            <CiLogout className="mr-1 mt-1" /> Logout
          </button>
        </div>
      )}
    </div>
  );
};

export default NavbarProfilePic;
