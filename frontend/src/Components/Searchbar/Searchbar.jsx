import React, { useState, useContext, useRef } from 'react';
import { IoSearch } from 'react-icons/io5';
import { DropdownContext } from '../Navbar/Navbar';

const Searchbar = (props) => {

    const dropdownContext = useContext(DropdownContext);
    

    const [isModalOpen, setIsModalOpen] = useState(false);

  const toggleModal = () => {
    setIsModalOpen(!isModalOpen);
  };

    return (
        <div className='relative'>
          {props.width >= 660 ? (
            <div className="flex items-center rounded-md bg-gray-800 border border-gray-500 px-2 py-1">
              <IoSearch className="text-gray-300 text-sm" />
              <input
                type="text"
                placeholder="Search"
                className='px-2 py-1 bg-transparent focus:outline-none text-sm text-gray-400 ml-[6px]'
                onClick={dropdownContext.toggleDropdownSearch}
              />
            </div>
          ) : (
            <div 
              className="flex items-center rounded-md bg-gray-800 border border-gray-500 px-2 py-1 hover:bg-gray-700 cursor-pointer"
              onClick={toggleModal}
            >
              <button 
                className='py-1 bg-transparent focus:outline-none text-sm text-gray-400 ml-[px] text-center'
              >
                <IoSearch className="text-gray-300 text-sm" />
              </button>
            </div>
          )}
    
          {isModalOpen && (
            <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
              <div className="bg-white dark:bg-gray-800 p-4 rounded-lg">
                <div className="flex items-center rounded-md bg-gray-800 border border-gray-500 px-2 py-1">
                  <IoSearch className="text-gray-300 text-sm" />
                  <input
                    type="text"
                    placeholder="Search"
                    className='px-2 py-1 bg-transparent focus:outline-none text-sm text-gray-400 ml-[6px]'
                    onClick={dropdownContext.toggleDropdownSearch}
                  />
                </div>
                <button 
                  className="mt-2 bg-red-500 text-white px-4 py-2 rounded"
                  onClick={toggleModal}
                >
                  Close
                </button>
              </div>
            </div>
          )}
        </div>
      );
};

export default Searchbar;
