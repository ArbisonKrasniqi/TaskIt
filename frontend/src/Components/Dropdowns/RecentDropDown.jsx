import React, { useState, useEffect, useContext } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';
import { WorkspaceContext } from '../Side/WorkspaceContext.jsx';

const RecentDropdown = (props) => {
    const [width, setWidth] = useState(window.innerWidth);

    const dropdownContext = useContext(DropdownContext);
    const { boards } = useContext(WorkspaceContext);

    // Resize event listener to adjust dropdown positioning based on window width
    function handleResize() {
        setWidth(window.innerWidth);
    }

    useEffect(() => {
        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        };
    }, []);

    const dynamicClassName = () => {
        if (width > 960) {
            return 'absolute left-0 z-20 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else if (width > 880 && width <= 960) {
            return 'absolute left-[120px] z-20 top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else {
            return 'absolute left-[155px] z-20 top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        }
    };

    return (
        <div className={`relative ${props.width <= 960 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownRecent}
                className={`bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center  
                ${dropdownContext.RecentDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}
            >
                Recent <span className='h-[14px] mx-2'>{dropdownContext.RecentDropdownIsOpen ? <IoIosArrowUp /> : <IoIosArrowDown />}</span>
            </button>

            {dropdownContext.RecentDropdownIsOpen && (
                <>
                    <div
                        className="fixed inset-0 z-10 bg-black opacity-25"
                        onClick={dropdownContext.toggleDropdownRecent}
                    ></div>

                    <div className={dynamicClassName()}>
                        {boards.length > 0 ? (
                            boards.map((board) => (
                                <button
                                    key={board.boardId}
                                    className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700'
                                >
                                    {board.title}
                                </button>
                            ))
                        ) : (
                            <div className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg'>
                                No boards available
                            </div>
                        )}
                    </div>
                </>
            )}
        </div>
    );
};

export default RecentDropdown;
