import React, { useContext, useEffect, useState } from 'react';
import { LuArchive } from "react-icons/lu";
import {deleteData, getDataWithId, postData } from "../../Services/FetchService.jsx";
import { IoTrashOutline } from "react-icons/io5";
import { WorkspaceContext } from '../Side/WorkspaceContext.jsx';
import { MainContext } from '../../Pages/MainContext.jsx';




const ClosedBoardsModal = ({ open, onClose }) => {
    const mainContext = useContext(MainContext);
    const [WorkspaceId, setWorkspaceId]=useState(mainContext.workspaceId);
    const {boards, setBoards } = useContext(WorkspaceContext);
    const [closedBoards, setClosedBoards] = useState([]);



    useEffect(()=>{
        if(open){
            fetchClosedBoards();
        }
    }, [open]);

    const fetchClosedBoards=async ()=>{
        try{
            const response = await getDataWithId('http://localhost:5157/backend/board/GetClosedBoards?workspaceId', WorkspaceId);
            setClosedBoards(response.data);
        }catch(error){
            console.error("Error fetching closed boards: ",error);
        }
    };

    const handleReopenBoard = async(boardId)=>{
        try{
            const board = {
                boardId: boardId,
            };
            const response = await postData('http://localhost:5157/backend/board/Reopen', board);
            console.log("Board reopened: ",response.data);
            
            
            
           
                 
            setClosedBoards((prevBoards) => 
                prevBoards.filter(board => board.boardId !== boardId) 
            );

            const reopenedBoard = await getDataWithId('http://localhost:5157/backend/board/GetBoardByID?id', boardId);
            const reopenedBoardData = reopenedBoard.data;
            console.log("Reopened board ",reopenedBoardData);
            setBoards((prevBoards) => [...prevBoards, reopenedBoardData]);
           
            console.log("Updated boards:", [...boards, reopenedBoard]);
            console.log("Updated closedBoards:", closedBoards);
        }catch(error){
            console.error("Error reopening board:", error);
        }

    }
    const handleDeleteBoard = async (boardId) => {
        console.log('Deleting board with ID:', boardId);
        try {
          const response = await deleteData('http://localhost:5157/backend/board/DeleteBoardByID', { BoardId: boardId });
          console.log('Deleting board response:', response);
      
          // Filter the board out from the local state
          setBoards((prevBoards) => prevBoards.filter((board) => board.boardId !== boardId));
      
          // Temporarily refresh the list of closed boards
          setClosedBoards((prevBoards) => prevBoards.filter(board => board.boardId !== boardId));
      
          // Fetch the updated list of boards by workspace ID
          const dataResponse = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId', { workspaceId: WorkspaceId });
          setBoards(dataResponse.data);
      
        } catch (error) {
          console.error('Error deleting board:', error);
        }
      };
      
    if(!open) return null;
    return (
        
        <div className={`fixed top-10 left-1/2 transform -translate-x-1/2 z-10 bg-gray-700 text-white p-5 rounded-lg  shadow-lg w-[600px]  
        ${open ? "visible bg-black/20" : "invisible"}
        `}>
            <div className='flex justify-between items-center'>
                <div className='flex items-center gap-3'>
                    <LuArchive />
                    <h1 className='text-lg font-semibold'>Closed Boards</h1>
                </div>
                <button onClick={onClose} className='text-white font-bold'>X</button>
            </div>
            <div className='mt-4'>
                {closedBoards.length === 0 ? (
                    <p>No closed boards found.</p>
                ) : (
                    <ul>
                        {closedBoards.map((board) => (
                            <li key={board.boardId} className='p-2 border-b border-gray-600 flex justify-between items-center'>
                               
                                <h3 className='flex flex-grow'>{board.title}</h3>
                                <div className='flex gap-3'>
                                <button className='bg-blue-600 text-black p-2 rounded-md hover:bg-blue-500' onClick={()=>{handleReopenBoard(board.boardId)}}>Reopen</button>
                                <button className='bg-red-700 text-black p-2 rounded-md w-[90px] flex flex-row items-center gap-1 hover:bg-red-600' onClick={()=>{handleDeleteBoard(board.boardId)}}><IoTrashOutline />Delete</button>
                                </div>
                             
                                </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );



}
export default ClosedBoardsModal