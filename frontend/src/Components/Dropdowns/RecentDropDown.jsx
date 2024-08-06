import React, { useState, useEffect, useContext } from 'react';
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import { DropdownContext } from '../Navbar/Navbar';
import { getDataWithId } from '../../Services/FetchService';
import { useBoards } from '../Side/WorkspaceContext.jsx';

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
          return 'absolute left-0 z-10 mt-2 w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else if (width > 880 && width<=960) {
          return 'absolute left-[120px] z-10 top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        } else {
          return 'absolute left-[155px] z-10 top-[-7px] w-48 bg-gray-800 rounded-lg p-2 shadow-lg';
        }
      };

      

    const dropdownContext = useContext(DropdownContext);
    const { boards, setBoards, handleCreateBoard } = useBoards();

    // useEffect(() => {
    //     const getRecentBoards = async () => {
    //         try {
                
    //             const response = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByMemberId', 'MEMBER ID');
    //             const data = response.data;

    //             console.log('Fetched data: ', data);

    //             if (data && Array.isArray(data) && data.length > 0) {
    //                 setBoards(data);
    //             }else{
    //                 console.log('Data is null, not an array, or empty:', data);
    //                 setBoards([]);
    //             }
    //         } catch (error) {
    //             setBoards([]);
    //         }
    //     };
    //     getRecentBoards();
    //     console.log(boards);
    //   },[]);


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
                    {boards.length > 0 ? (
                        boards.map((board) => (
                            <button 
                            key={board.id}
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
            )}
    </div>
    );
}

export default RecentDropdown;