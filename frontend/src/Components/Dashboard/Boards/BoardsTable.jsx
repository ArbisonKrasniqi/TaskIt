import React, { useState, useContext } from 'react';
import { BoardsContext } from './BoardsList';
import CustomButton from '../Buttons/CustomButton.jsx';
import { deleteData } from '../../../Services/FetchService';
import { useNavigate } from 'react-router-dom';
import { DashboardContext } from '../../../Pages/dashboard.jsx';

const BoardsTable = () => {
    const navigate = useNavigate();
    const boardsContext = useContext(BoardsContext);
    const [searchQuery, setSearchQuery] = useState('');
    const dashboardContext = useContext(DashboardContext);

    const handleBoardDelete = (id) => {
        async function deleteBoard() {
            try {
                const data = { boardId: id };
                const response = await deleteData('http://localhost:5157/backend/board/DeleteBoardByID', data);
                console.log(response);

                const updateBoards = (boardsContext.boards || []).filter(board => board.boardId !== id);
                boardsContext.setBoards(updateBoards);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                boardsContext.getBoards();
            }
        }
        deleteBoard();
    };

    const handleBoardEdit = (board) => {
        boardsContext.setBoardToUpdate(board);
        boardsContext.setShowUpdateInfoModal(true);
    };

    const handleBoardRowClick = boardId => {
        console.log(boardId);
        navigate(`/dashboard/board/${boardId}`);
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    // Ensure boardsContext.boards is always an array
    const boards = boardsContext.boards || [];
    const filteredBoards = boards.filter(board => {
        const boardIdMatch = board.boardId.toString().toLowerCase().includes(searchQuery);
        const titleMatch = board.title.toLowerCase().includes(searchQuery);
        const backgroundIdMatch = board.backgroundId.toString().toLowerCase().includes(searchQuery);
        const workspaceIdMatch = board.workspaceId.toString().toLowerCase().includes(searchQuery);
        const dateCreatedMatch = board.dateCreated ? board.dateCreated.toLowerCase().includes(searchQuery) : false;

        return boardIdMatch || titleMatch || backgroundIdMatch || workspaceIdMatch || dateCreatedMatch;
    });

    return (
        <div className="flex flex-col h-full">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for boards by ID, title, background ID, workspace ID, or date created"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            {/* Scrollable Table Container */}
            <div className="flex-grow overflow-x-auto">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">Board ID</th>
                            <th className="px-6 py-3">Title</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Background ID</th>
                            <th className="px-6 py-3">IsClosed</th>
                            <th className="px-6 py-3">Workspace ID</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredBoards.length > 0 ? (
                            filteredBoards.map((board, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{board.boardId}</td>
                                    <td className="px-6 py-4">{board.title}</td>
                                    <td className="px-6 py-4">{board.dateCreated}</td>
                                    <td className="px-6 py-4">{board.backgroundId}</td>
                                    <td className="px-6 py-4">{board.isClosed+""}</td>
                                    <td className="px-6 py-4">{board.workspaceId}</td>
                                    <td className="px-6 py-4">
                                    <CustomButton 
                                            onClick={() => handleBoardRowClick(board.boardId)}
                                            type="button"
                                            text="Open"
                                        />
                                        <CustomButton
                                            color="orange"
                                            type="button"
                                            text="Edit"
                                            onClick={() => {
                                                handleBoardEdit(board);
                                            }}
                                        />
                                        <CustomButton
                                            color="red"
                                            type="button"
                                            text="Delete"
                                            onClick={() => {
                                                handleBoardDelete(board.boardId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={6}>No boards found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default BoardsTable;
