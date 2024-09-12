import { useState,useContext, useEffect } from "react";
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
import { MainContext } from "../../Pages/MainContext.jsx";
import { useNavigate, useLocation } from 'react-router-dom';
const Sidebar = () => {



    const mainContext = useContext(MainContext);
    const navigate = useNavigate();
    const location = useLocation();
    const [hoveredBoardIndex, setHoveredBoardIndex]=useState('');
    const [hoveredSBoardIndex, setHoveredSBoardIndex]=useState('');

    const { 
        workspace, open, setOpen, workspaceTitle, setHover, hover, openCloseModal,
        setOpenSortModal, setOpenCloseModal, openSortModal, setOpenModal, openModal, 
        handleCreateBoard, setHoveredIndex, hoveredIndex, hoveredSIndex, setHoveredSIndex, setSelectedBoardTitle, 
         selectedBoardTitle, boards, selectedSort, handleSortChange, 
        handleStarBoard,handleStarButtonClick,backgroundUrls,handleCloseBoard,setOpenClosedBoardsModal, 
        showLimitModal, setShowLimitModal, boardCount, roli, starredBoards,ALLBoardsCount,setSelectedBoardId, selectedBoardId } = useContext(WorkspaceContext);

        const [boardToClose, setBoardToClose] = useState(null);
     

const Menus = [
    {title: "Boards", tag: "CiViewBoard", path: `/main/boards/${workspace?.workspaceId}`},
    {title: "Members", tag: "IoPersonOutline", path: `/main/members/${workspace?.workspaceId}`},
    {title: "Workspace settings", tag: "IoIosSettings", path: `/main/workspaceSettings/${workspace?.workspaceId}`},
 ]
const tagComponents = {
    CiViewBoard: <CiViewBoard/>,
    IoPersonOutline: <IoPersonOutline/>,
    IoIosSettings: <IoIosSettings/>,
    PiTable: <PiTable/>,
    LuCalendarDays: <LuCalendarDays/>,
    };
const WorkspaceViews = [
    {title: "Table", tag: "PiTable", path: `/main/table/${workspace?.workspaceId}`},
    {title: "Calendar", tag: "LuCalendarDays", path: `/main/calendar/${workspace?.workspaceId}`},
  ]

  const handleMenuClick = (path) => {
    navigate(path); 
    setSelectedBoardId(null); 
  };

  const handleBoardClick = (board) => {
    setSelectedBoardId(board.boardId); 
    navigate(`/main/board/${workspace.workspaceId}/${board.boardId}`);
};
    return(
        <div className="flex min-h-screen">
            <div className={`${open ? 'w-72' : 'w-8'} duration-100 h-full p-5 pt-8 relative border-r border-r-solid border-r-gray-500 overflow-y-auto`}  
            style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)', 
            scrollbarWidth: 'thin', 
             scrollbarColor: 'rgba(0, 0, 0, 0.2) transparent', 
            }}>
            
        

        <SlArrowLeft className={`absolute cursor-pointer rounded-full h-6 px-2 right-1 w-8 mt-2 border-2 text-center border-gray-700 bg-gray-800 text-gray-400 font-extrabold ${!open && "rotate-180 -right-3"}`}
           onClick={()=>setOpen(!open)}
            ></SlArrowLeft>

           <div className="flex gap-x-3 items-center">
           <button className={`w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800 duration-200 ${!open && "scale-0"}`}
            onClick={() => navigate(`/main/boards/${workspace.workspaceId}`)}
           >{workspaceTitle? workspaceTitle.charAt(0): ''}</button>
           <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-xl duration-200 ${!open && "scale-0"}`}>{workspaceTitle}</h1> 
           </div> 
           
           <hr className="w-full border-gray-400 mt-3"></hr>

            <ul className="pt-4">
            {Menus.map((menu,index)=>(
                <li key={index} 
                className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500 ${location.pathname === menu.path ? 'bg-gray-600' : ''} ${!open && "scale-0"}`}
                onClick={() => handleMenuClick(menu.path)}>
                {tagComponents[menu.tag]}
                <span>{menu.title}</span>
                </li>
            ))}

            </ul>
           <br></br>
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Workspace views</h1>
           
            <ul>
            {WorkspaceViews.map((views,index)=>(
                <li key={index} 
                className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500  ${location.pathname === views.path ? 'bg-gray-600' : ''} ${!open && "scale-0"}`}
                onClick={() => handleMenuClick(views.path)}>
                {tagComponents[views.tag]}
                <span>{views.title}</span>
                </li>
            ))}

            </ul>
            <div className="flex justify-between" 
            onMouseEnter={() => setHover(true)}
            onMouseLeave={() => setHover(false)}>
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Your boards</h1>
            <br/>
            {hover && (      
                <button onClick={()=>{setOpenSortModal(prev => !prev); setOpenCloseModal(false)}} className={`text-gray-400 cursor-pointer mr-4 ${!open && "scale-0"}`}><FaEllipsisH /></button>
                
            )}
            <SortModal open={openSortModal} onClose={()=> setOpenSortModal(false)} selectedSort={selectedSort} onSortChange={handleSortChange}></SortModal>
                  
                   <button onClick={()=>{ALLBoardsCount>=10 ? setShowLimitModal(true) : setOpenModal(true); setOpenClosedBoardsModal(false)}} className={`text-gray-400 cursor-pointer p-1 ${!open && "scale-0"}`}><FaPlus/></button>
                   {showLimitModal && <LimitModal onClose={() => setShowLimitModal(false)} />}
           <CreateBoardModal 
           open={openModal} 
           onClose={()=> setOpenModal(false)} 
           onBoardCreated={handleCreateBoard}></CreateBoardModal>
            </div>

            <ul>
    
                {
                    starredBoards.map((board, index) => (
                        <li key={index} 
                        onClick={() => handleBoardClick(board)}
                        onMouseEnter={() => setHoveredSBoardIndex(index)}
                        onMouseLeave={() => setHoveredSBoardIndex(null)}
                        className={`flex justify-between text-gray-400 text-lg font-semibold items-center mt-2 p-1 cursor-pointer  ${selectedBoardId === board.boardId ? 'bg-gray-600' : ''} ${!open && "scale-0"}`}
                        >
                            <div className="flex items-center overflow-hidden gap-x-2">
                                <div
                                    className="relative flex items-center ml-1 justify-center" 
                                    style={{ 
                                        width: '1.5em', 
                                        height: '1.5em', 
                                        backgroundImage: `url(${backgroundUrls[board.boardId] || 'background.jpg'})`,  
                                        backgroundSize: 'cover', 
                                        backgroundPosition: 'center' 
                                    }}
                                >
                                    <FiSquare className="text-4xl absolute" />
                                </div>
                                <span className="truncate overflow-hidden flex flex-1">{board.title}</span>
                            </div>
                          
                            <div className="flex justify-between">
                            {(hoveredSBoardIndex === index) ?   <button 
                                        onClick={(e) => {
                                            e.stopPropagation(); 
                                            setBoardToClose(board);
                                            setOpenCloseModal(prev => !prev);
                                            setSelectedBoardTitle(board.title);
                                            setSelectedBoardId(board.boardId); 
                                            setOpenSortModal(false);
                                            setOpenClosedBoardsModal(false);
                                        
                                        }} 
                                        className={`text-gray-400 cursor-pointer ${!open && "scale-0"} p-1 mr-1 border border-transparent hover:bg-gray-500 rounded-full `}><FaEllipsisH />
                                    
                                       
                                    </button> :<></>}
                                   
                               
                           
                                <CloseBoardModal open={openCloseModal} boardTitle={selectedBoardTitle} onClose={() => setOpenCloseModal(false)} role={roli} boardId={boardToClose?.boardId} userId={mainContext.userInfo.userId} onBoardClosed={handleCloseBoard} />
                                <button    
                                onMouseEnter={() => setHoveredSIndex(index)}
                                onMouseLeave={() => setHoveredSIndex(null)}
                                className={`text-gray-400 cursor-pointer text-lg transition-transform duration-200 ${hoveredSIndex === index ? "scale-125" : ""} ${!open && "scale-0"}`}   
                                onClick={(e) => handleStarButtonClick(e, board)}>
                               {(hoveredSIndex === index) ? 
                                 <MdOutlineStarOutline className="text-xl ml-1" /> : 
                                 <MdOutlineStarPurple500 className="text-xl ml-1" />
                                    }
                                </button>
                            </div>
                        </li>
                    ))
}
            </ul>
            <ul>
    {boards.length === 0 && starredBoards.length===0  ? (
          <li className={`text-gray-400 text-l font-semibold flex items-center gap-x-3 cursor-pointer p-2 ${!open && "scale-0"}`}>
          <span>No boards found</span>
      </li>


    ) : (
        boards.map((board, index) => (
            <li key={index} 
            onClick={() => handleBoardClick(board)}
            onMouseEnter={() => setHoveredBoardIndex(index)}
            onMouseLeave={() => setHoveredBoardIndex(null)}
            className={`flex justify-between text-gray-400 text-lg font-semibold items-center mt-2 p-1 cursor-pointer ${selectedBoardId === board.boardId ? 'bg-gray-600' : ''} ${!open && "scale-0"}`}
         
          >
                <div className="flex items-center overflow-hidden gap-x-2 ">
          
                <div 
        className="relative flex items-center justify-center ml-1" 
        style={{ 
          width: '1.5em', 
          height: '1.5em', 
          backgroundImage: `url(${backgroundUrls[board.boardId] || 'background.jpg'})`, 
          backgroundSize: 'cover', 
          backgroundPosition: 'center' 
        }}
      >
        <FiSquare className="text-4xl absolute" />
      </div>
                
      <p className="truncate flex flex-1">{board.title}</p>
                </div>
             {(hoveredBoardIndex===index) ? 
              <div className="flex justify-between items-center relative">

                
              <button 
            onClick={(e) => {
              e.stopPropagation(); // Prevent event from propagating
              setBoardToClose(board);
              setOpenCloseModal(prev => !prev);
              setSelectedBoardTitle(board.title);
              setSelectedBoardId(board.boardId); 
              setOpenSortModal(false);
              setOpenClosedBoardsModal(false);
        
          }} 
              className={`text-gray-400 cursor-pointer flex-shrink-0 ${!open && "scale-0"} p-1 border border-transparent hover:bg-gray-500 rounded-full `}><FaEllipsisH />
              </button>
              
      
                  <CloseBoardModal open={openCloseModal} boardTitle={selectedBoardTitle} onClose={()=> setOpenCloseModal(false)} role={roli} boardId={boardToClose?.boardId} userId={mainContext.userInfo.userId} onBoardClosed={handleCloseBoard}></CloseBoardModal>

                   
                        
              <button    
              onMouseEnter={() => setHoveredIndex(index)}
              onMouseLeave={() => setHoveredIndex(null)}
              className={`text-gray-400 cursor-pointer flex flex-shrink-0 text-lg transition-transform duration-200 ${hoveredIndex === index ? "scale-125" : ""}`} 
              onClick={(e) => handleStarButtonClick(e, board)} >
                 <MdOutlineStarOutline className="text-xl ml-1" />
              </button>
              
                          
                </div> 
           : <></> }
              
            </li>
        ))
    )}
</ul>
<br/><br/>
            </div>
        </div>


    );

}


export default Sidebar