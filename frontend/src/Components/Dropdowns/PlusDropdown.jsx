import React, { useContext } from 'react'
import CreateDropdown from './CreateDropdown'
import { FaPlus } from "react-icons/fa6";
import { DropdownContext } from '../Navbar';

const PlusDropdown = (props) => {

    const dropdownContext = useContext(DropdownContext);

    return(
        <div className={`relative ${props.width>1200 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownPlus}
                className={`bg-blue-400 text-gray-800 text-xl p-2 ml-2 rounded focus:outline-none hover:bg-blue-600 flex items-center text-center  
                ${dropdownContext.PlusDropdownIsOpen ? 'bg-blue-600' : 'bg-blue-400'}`}
                >
                    <FaPlus/>
                </button>

            {dropdownContext.PlusDropdownIsOpen && (
                <div className='absolute left-0 mt-2 bg-gray-800 rounded-lg shadow-lg'>
                    <button className={`${(props.width>=1200) && 'hidden'} `}><CreateDropdown /></button>
                </div>
            )}
        </div>
    );
}

export default PlusDropdown;