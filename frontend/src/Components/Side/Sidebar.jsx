import { useEffect, useState,useContext } from "react";
import { SlArrowLeft } from "react-icons/sl";
import { CiViewBoard } from "react-icons/ci";
import { IoPersonOutline } from "react-icons/io5";
import { IoIosSettings } from "react-icons/io";
import { PiTable } from "react-icons/pi";
import { LuCalendarDays } from "react-icons/lu";
import { FaPlus } from "react-icons/fa6";
import CreateBoardModal from "./CreateBoardModal.jsx";
import { FaEllipsisH } from "react-icons/fa";
import SortModal from "./SortModal.jsx";
import CloseBoardModal from "./CloseBoardModal.jsx";
import { MdOutlineStarOutline } from "react-icons/md";
import { MdOutlineStarPurple500 } from "react-icons/md";
import { FiSquare } from "react-icons/fi";
import { WorkspaceContext } from './WorkspaceContext';
import LimitModal from "../ContentFromSide/LimitModal.jsx";
const Sidebar = () => {




    const { 
       USERID, workspace, open, setOpen, workspaceTitle, setHover, hover, openCloseModal,
        setOpenSortModal, setOpenCloseModal, openSortModal, setOpenModal, openModal, 
        handleCreateBoard, setHoveredIndex, hoveredIndex, setSelectedBoardTitle, 
         selectedBoardTitle, roli, boards, selectedSort, handleSortChange, 
        handleStarBoard, getBackgroundImageUrl, handleCloseBoard,setOpenClosedBoardsModal, 
        showLimitModal, setShowLimitModal, boardCount } = useContext(WorkspaceContext);

        const [boardToClose, setBoardToClose] = useState(null);

const Menus = [
    {title: "Boards", tag: "CiViewBoard"},
    {title: "Members", tag: "IoPersonOutline"},
    {title: "Workspace settings", tag: "IoIosSettings"},
]
const tagComponents = {
    CiViewBoard: <CiViewBoard/>,
    IoPersonOutline: <IoPersonOutline/>,
    IoIosSettings: <IoIosSettings/>,
    PiTable: <PiTable/>,
    LuCalendarDays: <LuCalendarDays/>,
    };
const WorkspaceViews = [
    {title: "Table", tag: "PiTable"},
    {title: "Calendar", tag: "LuCalendarDays"},
]




    return(
        <div className="flex">
            <div className={`${open ? 'w-72' : 'w-8'} duration-100 h-screen p-5 pt-8 relative border-r border-r-solid border-r-gray-500`} style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
            
        

        <SlArrowLeft className={`absolute cursor-pointer rounded-full h-6 px-2 right-1 w-8 mt-2 border-2 text-center border-gray-700 bg-gray-800 text-gray-400 font-extrabold ${!open && "rotate-180 -right-3"}`}
           onClick={()=>setOpen(!open)}
            ></SlArrowLeft>

           <div className="flex gap-x-3 items-center">
           <button className={`w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800 duration-200 ${!open && "scale-0"}`}>{workspaceTitle? workspaceTitle.charAt(0): ''}</button>
           <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-xl duration-200 ${!open && "scale-0"}`}>{workspaceTitle}</h1> 
           </div> 
           
           <hr className="w-full border-gray-400 mt-3"></hr>

            <ul className="pt-4">
            {Menus.map((menu,index)=>(
                <li key={index} className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500 ${!open && "scale-0"}`}>
                {tagComponents[menu.tag]}
                <span>{menu.title}</span>
                </li>
            ))}

            </ul>
           <br></br>
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Workspace views</h1>
           
            <ul>
            {WorkspaceViews.map((views,index)=>(
                <li key={index} className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500 ${!open && "scale-0"}`}>
                {tagComponents[views.tag]}
                <span>{views.title}</span>
                </li>
            ))}

            </ul>
            <br/>
            <div className="flex justify-between" 
            onMouseEnter={() => setHover(true)}
            onMouseLeave={() => setHover(false)}>
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Your boards</h1>
            <br/>
            {hover && (      
                <button onClick={()=>{setOpenSortModal(prev => !prev); setOpenCloseModal(false)}} className={`text-gray-400 cursor-pointer mr-4 ${!open && "scale-0"}`}><FaEllipsisH /></button>
                
            )}
            <SortModal open={openSortModal} onClose={()=> setOpenSortModal(false)} selectedSort={selectedSort} onSortChange={handleSortChange}></SortModal>
                  
                   <button onClick={()=>{boardCount>=10 ? setShowLimitModal(true) : setOpenModal(true); setOpenClosedBoardsModal(false)}} className={`text-gray-400 cursor-pointer p-1 ${!open && "scale-0"}`}><FaPlus/></button>
                   {showLimitModal && <LimitModal onClose={() => setShowLimitModal(false)} />}
           <CreateBoardModal open={openModal} onClose={()=> setOpenModal(false)} onBoardCreated={handleCreateBoard}></CreateBoardModal>
            </div>

            

            <ul>
    {boards.length === 0 ? (
          <li className={`text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2 ${!open && "scale-0"}`}>
          <span>No boards found</span>
      </li>


    ) : (
        boards.map((board, index) => (
            <li key={index} 
            className={`flex justify-between text-gray-400 text-lg font-semibold items-center mt-2 p-1 cursor-pointer hover:bg-gray-500 ${!open && "scale-0"}`}
             onMouseEnter={() => setHoveredIndex(index)}
            onMouseLeave={() => setHoveredIndex(null)}>
                <div className="flex items-center gap-x-2 ">
          
                <div 
        className="relative flex items-center justify-center" 
        style={{ 
          width: '1.5em', 
          height: '1.5em', 
          backgroundImage: `url(${getBackgroundImageUrl(board)})`, 
          backgroundSize: 'cover', 
          backgroundPosition: 'center' 
        }}
      >
        <FiSquare className="text-4xl absolute" />
      </div>
                
                <span>{board.title}</span>
                </div>
                <div className="flex justify-between">

                {hoveredIndex===index && (      
                <button onClick={()=> {setBoardToClose(board); setOpenCloseModal(prev=> !prev); setSelectedBoardTitle(board.title); setOpenSortModal(false); setOpenClosedBoardsModal(false)}} className={`text-gray-400 cursor-pointer ${!open && "scale-0"} mr-3`}><FaEllipsisH /></button>
                
            )}
                    <CloseBoardModal open={openCloseModal} boardTitle={selectedBoardTitle} onClose={()=> setOpenCloseModal(false)} role={roli} boardId={boardToClose?.boardId} userId={USERID} onBoardClosed={handleCloseBoard}></CloseBoardModal>

                     
                          
                <button className={`text-gray-400 cursor-pointer text-lg ${!open && "scale-0"}`} onClick={()=>handleStarBoard(board)} >{(board.starred) ? <MdOutlineStarPurple500 />: (hoveredIndex===index ) ? <MdOutlineStarOutline/>  : ''}</button>
                
                
           

                   

                 
                  </div>
            </li>
        ))
    )}
</ul>

            </div>
          
        </div>


    );

}


export default Sidebar