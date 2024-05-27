import React, { useContext } from 'react';
import WorkspaceDropdown from './WorkspaceDropdown';
import RecentDropdown from './RecentDropDown';
import StarredDropdown from './StarredDropdown';
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';

const MoreDropdown = (props) => {

    const dropdownContext = useContext(DropdownContext);

    return(
        <div className={`relative ${props.width>=1070 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownMore}
                className='bg-gray-800 text-gray-400 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center'>
                    More <span className='h-[14px] mx-2'>{dropdownContext.MoreDropdownIsOpen ? <IoIosArrowUp/> : <IoIosArrowDown/>}</span>
            </button>

            {dropdownContext.MoreDropdownIsOpen && (
                <div className='absolute left-0 mt-2  bg-gray-800 rounded-lg p-2 shadow-lg flex-col items-stretch'>
                    <button className={`${props.width>880 && 'hidden'}`}><WorkspaceDropdown/></button>
                    <button className={`${props.width>960 && 'hidden'}`}><RecentDropdown /></button>
                    <button className={`${props.width>1070 && 'hidden'}`}><StarredDropdown /></button>
                </div>
            )}
        </div>
    );
}

export default MoreDropdown;