import React, { useState, useEffect, useContext } from 'react'
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';

const StarredDropdown = (props) => {

    const [width,setWidth] = useState(window.innerWidth);

    function handleResize(){
        setWidth(window.innerWidth);
    }

    useEffect(() => {
        window.addEventListener("resize", handleResize);
        
    }, [])

    const dropdownContext = useContext(DropdownContext);

    const dynamicClassName = () => {
        if (width > 1070) {
          return 'absolute left-0 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else if (width > 880 && width<=1070) {
          return 'absolute left-[120px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else {
          return 'absolute left-[155px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        }
      };

    return(
        <div className={`relative ${props.width<=1070 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownStarred}
                className={`bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center  
                ${dropdownContext.StarredDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}>
                    Starred <span className='h-[14px] mx-2'>{dropdownContext.StarredDropdownIsOpen ? <IoIosArrowUp/> : <IoIosArrowDown/>}</span>
            </button>

            {dropdownContext.StarredDropdownIsOpen && (
                <div className={dynamicClassName()}
                  >
                    <button className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'>
                        Starred boards
                    </button>
                </div>
            )}
        </div>
    );
}

export default StarredDropdown;