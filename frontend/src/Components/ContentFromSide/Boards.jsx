
import { useEffect, useState, useCallback  } from "react";
import { GoPencil } from "react-icons/go";
import { IoPersonAddOutline } from "react-icons/io5";
import { CiSearch } from "react-icons/ci";
import SortModal from "../Side/SortModal.jsx";
import CreateBoardModal from "../Side/CreateBoardModal.jsx";
import { FaPlus } from "react-icons/fa";
import React, { useContext } from 'react';
import { WorkspaceContext } from '../Side/WorkspaceContext';
import ClosedBoardsModal from "./ClosedBoardsModal.jsx";
import { MdOutlineStarOutline } from "react-icons/md";
import { MdOutlineStarPurple500 } from "react-icons/md";
import LimitModal from "./LimitModal.jsx";
import SideMenusHeader from "./SideMenusHeader.jsx";


const Boards = () =>{
    const { workspace,openClosedBoardsModal, showLimitModal, setShowLimitModal, 
        boardCount, setOpenClosedBoardsModal, boards, handleCreateBoard, openModal, 
        setOpenModal, setOpenCloseModal, handleStarBoard, handleSortChange, setOpenSortModal,
         openSortModal, selectedSort, getBackgroundImageUrl,hoveredBoardIndex, 
         setHoveredBoardIndex } = useContext(WorkspaceContext);

  

    if (workspace == null) {
        return <div>Loading...</div>;
    }
return (
    <div className={`duration-100 min-h-screen`} style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
      <SideMenusHeader />
      <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-20 mr-20 flex-wrap">
        <h2 className="text-2xl ">Boards {boardCount}/10</h2>

        <div className="flex flex-row flex-wrap gap-10">

          <div className="flex flex-col mt-5 flex-wrap">
            <label>Sort </label>
            <button onClick={()=>{setOpenSortModal(prev => !prev); setOpenModal(false);
              setOpenClosedBoardsModal(false)}} className="bg-transparent border border-solid border-gray-500 flex
              flex-row mt-2 rounded-md pl-5 pr-5 pt-2 pb-2 w-[180px]">{selectedSort}</button>

            <SortModal open={openSortModal} onClose={()=> setOpenSortModal(false)} selectedSort={selectedSort}
              onSortChange={handleSortChange}></SortModal>
          </div>

          <div className="flex flex-col mt-5">
            <label htmlFor="searchBoard">Search</label>
            <div className="bg-transparent border border-solid border-gray-500 flex flex-row mt-2 rounded-md ">
              <CiSearch className="mt-3 ml-1 font-bold" />
              <input type="search" id="searchBoard" name="searchBoard" placeholder="Search boards..."
                className="p-2 border-none bg-transparent focus:outline-none"></input>
            </div>
          </div>

        </div>

        <div className="mt-5 flex flex-row max-w-[1200px] gap-3">


          <ul className="flex flex-wrap flex-row justify-between gap-10">
            <li
              className="w-[300px] h-[150px] flex justify-normal text-gray-400 text-lg font-semibold items-center mt-2 p-1 cursor-pointer hover:bg-gray-500 border border-solid border-gray-700">
              <button onClick={()=>{boardCount>=10 ? setShowLimitModal(true) : setOpenModal(prev => !prev);
                setOpenClosedBoardsModal(false)}} className="w-full h-full flex items-center text-gray-400
                cursor-pointer gap-2 p-1">
                <FaPlus /> Create new Board
              </button>
              {showLimitModal && <LimitModal onClose={()=> setShowLimitModal(false)} />}
            </li>
            <CreateBoardModal open={openModal} onClose={()=> setOpenModal(false)} onBoardCreated={handleCreateBoard}>
            </CreateBoardModal>

            {boards.length===0? (
            <li className="mt-10"> <span>No boards found</span> </li>
            ) : (
            boards.map((board, index)=>(

            <li key={index} onMouseEnter={()=> setHoveredBoardIndex(index)}
              onMouseLeave={() => setHoveredBoardIndex(null)}
              style={{  width: '300px', 
                        height: '150px',}}
              className={`flex justify-normal text-white text-lg font-semibold items-center mt-3 cursor-pointer border
              border-solid border-gray-700 ${hoveredBoardIndex===index ? ` bg-gray-400 opacity-50`: ''} `}>

              <div className="relative w-full h-full" style={{ 
                    backgroundImage: `url(${getBackgroundImageUrl(board)})`, 
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                  }}>
                <h2 className="ml-3">{board.title}</h2>


                <button
                  className="absolute right-2 top-2 text-white font-bold text-3xl transition ease-in-out duration-300"
                  style={{textShadow: '0 0 10px rgba(255, 255, 255, 0.6), 0 0 20px rgba(255, 255, 255, 0.4)'}}
                  onClick={()=>handleStarBoard(board)}
                  >{(board.starred) ?
                  <MdOutlineStarPurple500 />: (hoveredBoardIndex===index ) ?
                  <MdOutlineStarOutline /> : ''}</button>


              </div>


            </li>
            )))
            }
          </ul>
        </div>
        <button
          className="flex justify-center text-black font-sans text-center font-semibold bg-blue-600 items-center border border-solid border-blue-700 rounded-lg  mt-10 hover:bg-blue-500 w-[200px] h-[40px]"
          onClick={()=> setOpenClosedBoardsModal(prev => !prev)}
          > View Closed Boards</button>

        <ClosedBoardsModal open={openClosedBoardsModal} onClose={()=> {setOpenClosedBoardsModal(false);
          setOpenCloseModal(false);}}></ClosedBoardsModal>
      </div>
    </div>
   
);
}
export default Boards