import { useState, useEffect } from "react";
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
import { useNavigate, useParams } from "react-router-dom";
import StarButton from "./StarButton.jsx";

const Boards = () =>{
    const { workspace,openClosedBoardsModal, showLimitModal, setShowLimitModal, 
        boardCount, setOpenClosedBoardsModal, boards, handleCreateBoard, openModal, 
        setOpenModal, setOpenCloseModal, handleStarBoard, handleSortChange, setOpenSortModal,
         openSortModal, selectedSort,hoveredBoardIndex, 
         setHoveredBoardIndex, hoveredBoardSIndex, setHoveredBoardSIndex, roli, starredBoards, backgroundUrls, closedBoards, fetchClosedBoards, 
         ALLBoardsCount, handleStarButtonClick} = useContext(WorkspaceContext);

         const [searchTerm, setSearchTerm] = useState('');
         // per me handle ndryshimet ne search input
         const handleSearchChange = (event) => {
          setSearchTerm(event.target.value);
      };
      const filteredBoards = boards.filter(board =>
        board.title.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const filteredStarredBoards = starredBoards.filter(board =>
        board.title.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const navigate = useNavigate();
    const {workspaceId} = useParams();

    const countClosedBoards = closedBoards.length;

    useEffect(()=>{
     
          fetchClosedBoards();

  }, [workspace]);

    if (workspace == null) {
        return <div>Loading...</div>;
    }
return (
    <div className="min-h-screen h-full overflow-y-auto" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
      <SideMenusHeader />
      <div className="font-semibold font-sans text-gray-400 flex justify-normal mt-10 flex-col ml-20 mr-20 flex-wrap overflow-y-auto">
        <h2 className="text-2xl ">Boards {boardCount}/10 (+{countClosedBoards} Closed Boards) </h2>

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
              <input type="search" id="searchBoard" name="searchBoard" 
              placeholder="Search boards..."
              className="p-2 border-none bg-transparent focus:outline-none"
            value={searchTerm}
            onChange={handleSearchChange}
          />
            </div>
          </div>

        </div>

        <div className="mt-5 flex flex-row max-w-[1200px] gap-3">


          <ul className="flex flex-wrap flex-row justify-between gap-10">
            <li
              className="w-[300px] h-[150px] flex justify-normal text-gray-400 text-lg font-semibold items-center mt-2 p-1 cursor-pointer hover:bg-gray-500 border border-solid border-gray-700">
              <button onClick={()=>{ALLBoardsCount>=10 ? setShowLimitModal(true) : setOpenModal(prev => !prev);
                setOpenClosedBoardsModal(false)}} className="w-full h-full flex items-center text-gray-400
                cursor-pointer gap-2 p-1">
                <FaPlus /> Create new Board
              </button>
              {showLimitModal && <LimitModal onClose={()=> setShowLimitModal(false)} />}
            </li>
            <CreateBoardModal open={openModal} onClose={()=> setOpenModal(false)} onBoardCreated={handleCreateBoard}>
            </CreateBoardModal>


             {
              filteredStarredBoards.map((board, index)=>(
                
            <li key={index} onMouseEnter={()=> setHoveredBoardSIndex(index)}
            onMouseLeave={() => setHoveredBoardSIndex(null)}
            style={{  width: '300px', 
                      height: '150px',}}
            className={`flex justify-normal text-white text-lg font-semibold items-center mt-3 cursor-pointer border
            border-solid border-gray-700 ${hoveredBoardSIndex===index ? ` bg-gray-400 opacity-50`: ''} `}>

            <div className="relative w-full h-full" style={{ 
                   backgroundImage: `url(${backgroundUrls[board.boardId] || '../Side/background.jpg'})`,  
                  backgroundSize: 'cover',
                  backgroundPosition: 'center',
                }}
                onClick={() => navigate(`/main/board/${workspaceId}/${board.boardId}`)}>
              <h2 className="ml-3" >{board.title}</h2>


              <button
                className="absolute right-2 top-2 text-white font-bold text-3xl transition ease-in-out duration-300"
                style={{textShadow: '0 0 10px rgba(255, 255, 255, 0.6), 0 0 20px rgba(255, 255, 255, 0.4)'}}
                onClick={(e) => handleStarButtonClick(e, board)}>
                <MdOutlineStarPurple500 /></button>


            </div>


          </li>
          ))}

            {filteredBoards.length===0 && filteredStarredBoards.length===0 ? (
            <li className="mt-10"> <span>No boards found</span> </li>
            ) : (
              filteredBoards.map((board, index)=>(

            <li key={index} onMouseEnter={()=> setHoveredBoardIndex(index)}
              onMouseLeave={() => setHoveredBoardIndex(null)}
              style={{  width: '300px', 
                        height: '150px',}}
              className={`flex justify-normal text-white text-lg font-semibold items-center mt-3 cursor-pointer border
              border-solid border-gray-700 ${hoveredBoardIndex===index ? ` bg-gray-400 opacity-50`: ''} `}>

              <div className="relative w-full h-full" style={{ 
                    backgroundImage: `url(purple.jpg)`, 
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                  }}
                  onClick={() => navigate(`/main/board/${workspaceId}/${board.boardId}`)}>

                <h2 key={board.boardId} className="ml-3">{board.title}</h2>


                <button
                  className="absolute right-2 top-2 text-white font-bold text-3xl transition ease-in-out duration-300"
                  style={{textShadow: '0 0 10px rgba(255, 255, 255, 0.6), 0 0 20px rgba(255, 255, 255, 0.4)'}}
                  onClick={(e) => handleStarButtonClick(e, board)}
                  >
                  {(hoveredBoardIndex===index ) ?
                  <MdOutlineStarPurple500 /> : <MdOutlineStarOutline /> }</button>


              </div>


            </li>
            )))
            }
          </ul>
        </div>
        {roli==="Owner" && (  
          <button
          className="flex justify-center text-black font-sans text-center font-semibold bg-blue-600 items-center border border-solid border-blue-700 rounded-lg  mt-10 hover:bg-blue-500 w-[200px] h-[40px]"
          onClick={()=> setOpenClosedBoardsModal(prev => !prev)}
          > View Closed Boards</button>
)}
      
        <ClosedBoardsModal open={openClosedBoardsModal} onClose={()=> {setOpenClosedBoardsModal(false);
          setOpenCloseModal(false);}}></ClosedBoardsModal>
         
      </div>
      <br/><br/><br/>
    </div>
   
);
}
export default Boards