import React, { useState, useEffect, useContext } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';

const RecentDropdown = (props) => {

    const [width,setWidth] = useState(window.innerWidth);

    function handleResize(){
        setWidth(window.innerWidth);
    }

    useEffect(() => {
        window.addEventListener("resize", handleResize);
        
    }, [])

    const dynamicClassName = () => {
        if (width > 960) {
          return 'absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else if (width > 880 && width<=960) {
          return 'absolute left-[120px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else {
          return 'absolute left-[155px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        }
      };

    const dropdownContext = useContext(DropdownContext);

    return(
        <div className={`relative ${props.width<=960 && 'hidden'}`}>
        <button
            onClick={dropdownContext.toggleDropdownRecent}
            className={` bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center 
             ${dropdownContext.RecentDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}
        >
            Recent <span className='h-[14px] mx-2'>{dropdownContext.RecentDropdownIsOpen ? <IoIosArrowUp/> : <IoIosArrowDown/>}</span>
        </button>

            {dropdownContext.RecentDropdownIsOpen && (
                <div className={dynamicClassName()}>
                    <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Board 1
                    </button>
                    <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Board 2
                    </button>
                </div>
            )}
    </div>
    );
}

export default RecentDropdown;