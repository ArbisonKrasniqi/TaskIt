import React, { useState, useEffect, useContext } from 'react'
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import { useNavigate } from 'react-router-dom';

const StarredDropdown = (props) => {

    const [width, setWidth] = useState(window.innerWidth);
    const { boards, starredBoards, workspace } = useContext(WorkspaceContext);
    const navigate = useNavigate();

    function handleResize() {
        setWidth(window.innerWidth);
    }

    useEffect(() => {
        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        };
    }, []);

    const dropdownContext = useContext(DropdownContext);

    const dynamicClassName = () => {
        if (width > 1070) {
            return 'absolute left-0 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else if (width > 880 && width <= 1070) {
            return 'absolute left-[120px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else {
            return 'absolute left-[155px] top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        }
    };

    return (
        <div className={`relative ${props.width <= 1070 && 'hidden'}`}>
            <button
                onClick={dropdownContext.toggleDropdownStarred}
                className={`bg-gray-800 px-4 py-2 rounded focus:outline-none hover:bg-gray-700 flex items-center  
                ${dropdownContext.StarredDropdownIsOpen ? 'text-blue-400' : 'text-gray-400'}`}>
                Starred <span className='h-[14px] mx-2'>{dropdownContext.StarredDropdownIsOpen ? <IoIosArrowUp /> : <IoIosArrowDown />}</span>
            </button>

            {dropdownContext.StarredDropdownIsOpen && (
                <div className={dynamicClassName()}>
                    {(boards.length === 0 && starredBoards.length === 0) ? (
                        <div className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg'>
                            No boards created
                        </div>
                    ) : starredBoards.length === 0 ? (
                        <div className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg'>
                            Star important boards to have an easier access to them
                        </div>
                    ) : (
                        starredBoards.map((board, index) => (
                            <div
                                key={index}
                                className='block w-full text-left px-4 py-2 bg-gray-800 text-gray-400 rounded-lg hover:bg-gray-700 cursor-pointer truncate'
                                onClick={() => navigate(`/main/board/${workspace.workspaceId}/${board.boardId}`)}>
                                {board.title}
                            </div>
                        ))
                    )}
                </div>
            )}
        </div>
    );
}

export default StarredDropdown;
