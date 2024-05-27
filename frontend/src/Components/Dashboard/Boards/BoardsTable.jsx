import React, { useContext } from 'react';
import { BoardsContext } from './BoardsList';
import CustomButton from '../Buttons/CustomButton.jsx';
import { deleteData } from '../../../Services/FetchService';

const BoardsTable = () => {
    const boardsContext = useContext(BoardsContext);

    const handleBoardDelete = (id) => {
        async function deleteBoard() {
            try {
                const data = { boardId: id };
                const response = await deleteData('http://localhost:5157/backend/board/DeleteBoardByID', data);
                console.log(response);

                const updateBoards = boardsContext.boards.filter(board => board.boardId !== id);
                boardsContext.setBoards(updateBoards);
            } catch (error) {
                boardsContext.setErrorMessage(error.message + id);
                boardsContext.setShowBoardsErrorModal(true);
                boardsContext.getBoards();
            }
        }
        deleteBoard();
    };

    const handleBoardEdit = (board) => {
        boardsContext.setBoardToUpdate(board);
        boardsContext.setShowUpdateInfoModal(true);
    };

    return (
        <div className='overflow-x-auto'>
            <table className='w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400'>
                <thead className='text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400'>
                    <tr>
                        <th className='px-6 py-3'>ID</th>
                        <th className='px-6 py-3'>Title</th>
                        <th className='px-6 py-3'>Date Created</th>
                        <th className='px-6 py-3'>Background ID</th>
                        <th className='px-6 py-3'>Workspace ID</th>
                        <th className='px-6 py-3'>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {boardsContext.boards ? (boardsContext.boards.map((board, index) => (
                        <tr key={index} className='bg-white border-b dark:bg-gray-800 dark:border-gray-700'>
                            <td className='px-6 py-4'>{board.boardId}</td>
                            <td className='px-6 py-4'>{board.title}</td>
                            <td className='px-6 py-4'>{board.dateCreated}</td>
                            <td className='px-6 py-4'>{board.backgroundId}</td>
                            <td className='px-6 py-4'>{board.workspaceId}</td>
                            <td className='px-6 py-4'>
                                 <CustomButton color="orange" type='button' text="Edit" onClick={() => handleBoardEdit(board)} />
                                 <CustomButton color="red" type='button' text="Delete" onClick={() => handleBoardDelete(board.boardId)} />
                               
                            </td>
                        </tr>
                    ))) : (
                        <tr className='bg-white border-b dark:bg-gray-800 dark:border-gray-700'>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default BoardsTable;
