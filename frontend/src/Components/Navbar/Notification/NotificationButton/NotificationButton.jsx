import React, { useContext } from 'react'
import { MdNotificationsNone } from "react-icons/md";
import { DropdownContext } from '../../Navbar';

const NotificationButton = (props) => {

    const dropdownContext = useContext(DropdownContext);

    return(
        <div className='relative'>
            <button
                className={`flex items-center justify-center text-gray-400 focus:outline-none w-8 h-8 mx-2 rounded-full  hover:bg-gray-700 
                ${dropdownContext.NotificationDropdownIsOpen && 'rotate-45'}`}
                onClick={dropdownContext.toggleDropdownNotification}
                >
                    <MdNotificationsNone className='w-6 h-auto'/>

                </button>

                {dropdownContext.NotificationDropdownIsOpen && (
            <div className="absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg ">
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
              
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
                
              </button>
              <button className="block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700">
               
              </button>
            </div>
          )}
        </div>
    );
}

export default NotificationButton