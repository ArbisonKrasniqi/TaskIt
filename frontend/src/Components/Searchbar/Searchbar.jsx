import React, { useContext } from 'react';
import { IoSearch } from 'react-icons/io5';
import { DropdownContext } from '../Navbar/Navbar';

const Searchbar = (props) => {

    const dropdownContext = useContext(DropdownContext);

  return (
    <div className='relative'>

        {props.width>=660 ? (
            <div className="flex items-center rounded-md bg-gray-800 border border-gray-500 px-2 py-1">
                <IoSearch className="text-gray-300 text-sm" />
                <input
                    type="text"
                    placeholder="Search"
                    className='px-2 py-1 bg-transparent focus:outline-none text-sm text-gray-400 ml-[6px]'
                    onClick={dropdownContext.toggleDropdownSearch}
                />
            </div>
        ) :( 
            <div className="flex items-center rounded-md bg-gray-800 border border-gray-500 px-2 py-1 hover:bg-gray-700 cursor-pointer">
                <button className=' py-1 bg-transparent focus:outline-none text-sm text-gray-400 ml-[px] text-center'
                    onClick={dropdownContext.toggleDropdownSearch}
                >
                <IoSearch className="text-gray-300 text-sm" />
                </button>
            </div>
        )}
      </div>
  );
};

export default Searchbar;
