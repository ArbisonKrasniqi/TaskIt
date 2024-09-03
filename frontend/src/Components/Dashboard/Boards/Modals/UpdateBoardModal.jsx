import React, { useState, useContext, useEffect } from 'react';
import { putData } from '../../../../Services/FetchService';
import { BoardsContext } from '../BoardsList';
import CustomButton from '../../Buttons/CustomButton';
import { DashboardContext } from '../../../../Pages/dashboard';

const UpdateBoardModal = (props) => {
    const boardsContext = useContext(BoardsContext);
    const boardToUpdate = boardsContext.boardToUpdate;
    const dashboardContext = useContext(DashboardContext);

    const [title, setTitle] = useState(boardToUpdate.title);
    const [backgroundId, setBackgroundId] = useState(boardToUpdate.backgroundId);
    const [isClosed, setIsClosed] = useState(boardToUpdate.isClosed);

    useEffect(() => {
        setTitle(boardToUpdate.title);
        setBackgroundId(boardToUpdate.backgroundId);
        setIsClosed(boardToUpdate.isClosed);
    }, [boardToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const data = {
                boardId: boardToUpdate.boardId,
                title: title,
                backgroundId: backgroundId,
                isClosed: isClosed
            };

            console.log('Sending data:', data);

            const response = await putData('http://localhost:5157/backend/board/UpdateBoard', data);
            console.log('Response:', response);

            const updatedBoards = boardsContext.boards.map(board => {
                if (board.boardId === data.boardId) {
                    return {
                        ...board,
                        title: title,
                        backgroundId: backgroundId,
                    };
                }
                return board;
            });

            boardsContext.setBoards(updatedBoards);
            props.setShowUpdateInfoModal(false);
        } catch (error) {
            console.error('Error updating board:', error);
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            boardsContext.getBoards();
            props.setShowUpdateInfoModal(false);
        }
    };

    return (
        <div className='fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50'>
            <form onSubmit={handleSubmit} className='bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto'>
                <div className='mb-6'>
                    <label htmlFor="title" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Title</label>
                    <input value={title}
                           onChange={(e) => setTitle(e.target.value)}
                           type='text'
                           id='title'
                           className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                </div>

                <div className='grid md:grid-cols-2 md:gap-6'>
                    <div className="mb-6">
                        <label htmlFor="backgroundId" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Background ID</label>
                        <input value={backgroundId}
                               onChange={(e) => setBackgroundId(e.target.value)}
                               type="text"
                               id='backgroundId'
                               className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                    </div>
                </div>
                <div className="mb-6 flex items-center">
                        <input 
                            type="checkbox" 
                            id="isClosed" 
                            checked={isClosed} 
                            onChange={(e) => setIsClosed(e.target.checked)} 
                            className="mr-2 bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:focus:ring-blue-500 dark:focus:border-blue-500" 
                        />
                        <label htmlFor="isClosed" className="text-sm font-medium text-gray-900 dark:text-white">isClosed</label>
                    </div>

                <div className='flex justify-around'>
                    <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed" />
                    <CustomButton type="submit" text="Update" color="longGreen" />
                </div>
            </form>
        </div>
    );
};

export default UpdateBoardModal;
