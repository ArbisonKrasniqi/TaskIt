import React, { useContext } from 'react';
import { DropdownContext } from '../Navbar/Navbar';
import { MainContext } from '../../Pages/MainContext';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useNavigate } from "react-router-dom";
import { useState } from 'react';

const MemberProfilePic = () => {
    const navigate = useNavigate();
    const { ProfilePicIsOpen, toggleDropdownProfilePic } = useContext(DropdownContext);
    const { userInfo } = useContext(MainContext);
    const { getInitialsFromFullName, memberDetails, getInitials } = useContext(WorkspaceContext);
    // const name = userInfo.name;
    // const email = userInfo.email;

    const handleSignOutClick = () => {
        document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
        document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
        navigate(`/login`);
    };

    const [name ,setName] = useState();
    const [email, setEmail] = useState();
    return (
        <div className="relative flex items-baseline overflow-visible z-10 w-auto h-auto box-border">
            {memberDetails.map((member, key) => (
            <button
                className="flex items-center focus:outline-none relative mr-2 max-w-full max-h-full"
                onClick={(e) => {e.stopPropagation(); toggleDropdownProfilePic();}}
                key={key}
            >
                <div className="flex items-center" onClick={() => {setName(member.firstName+" "+member.lastName); setEmail(member.email); }}>
                        <div
                            key={member.id}
                            className="flex-shrink-0 w-8 h-8 rounded-full overflow-hidden mr-2 bg-gradient-to-r from-pink-400 to-purple-600 flex items-center justify-center text-white text-sm"
                        >
                            {getInitials(member.firstName, member.lastName)}
                        </div>
                </div>
            </button>
            ))}

            {/* Dropdown menu */}
            {ProfilePicIsOpen && (
                <div className="absolute right-0 mt-2 w-auto h-auto z-50 bg-gray-800 rounded-lg shadow-lg p-2">
                    <div
                        onClick={() => {
                            toggleDropdownProfilePic();
                            // navigate(`/main/profile`);
                        }}
                        className='flex items-start p-1 bg-gray-800 rounded-lg text-gray-400 hover:bg-gray-700 mb-2 hover:cursor-pointer'
                    >
                        <div className="flex-shrink-0 w-8 h-8 ml-1 mt-1 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-pink-400 to-purple-600">
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
                </div>
            )}
        </div>
    );
};

export default MemberProfilePic;
