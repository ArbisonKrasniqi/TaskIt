import React, {useContext, createContext} from 'react';
import {StarredBoardsContext} from "./StarredBoardsList"
import CustomButton from '../Buttons/CustomButton';
import { deleteData } from '../../../Services/FetchService';
import UpdateStarredBoardButton from "./Buttons/UpdateStarredBoardButton";
import { DashboardContext } from '../../../Pages/dashboard';

export const UpdateContext = createContext();
const StarredBoardsTable = () => {
    const { StarredBoards, setStarredBoards, getStarredBoards } = useContext(StarredBoardsContext);
    const dashboardContext = useContext(DashboardContext);

    const HandleStarredBoardDelete = (id) => {
        async function deleteStarredBoard() {
            try {
                const data = {
                    StarredId: id
                };
                const response = await deleteData('/backend/starredBoard/DeleteStarredBoardById', data);
                console.log(response.data);

                const updatedStarredBoards = StarredBoards.filter(starredBoard => starredBoard.starredBoardId !== id);
                setStarredBoards(updatedStarredBoards);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setDashboardErrorModal(true);
                getStarredBoards(); // për të përditësuar listën
            }
        }
        deleteStarredBoard();
    }

    return (
        <div className="overflow-x-auto">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th className="px-6 py-3">ID</th>
                        <th className="px-6 py-3">Board Id</th>
                        <th className="px-6 py-3">User Id</th>
                        <th className="px-6 py-3">Workspace Id</th>
                        <th className="px-6 py-3">Actions</th>
                    </tr>
                </thead>

                <tbody>
                    {StarredBoards ? (StarredBoards.map((starredBoard, index) => (
                        <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <td className="px-6 py-4">{starredBoard.starredBoardId}</td>
                            <td className="px-6 py-4">{starredBoard.boardId}</td>
                            <td className="px-6 py-4">{starredBoard.userId}</td>
                            <td className="px-6 py-4">{starredBoard.workspaceId}</td>
                            <td className="px-6 py-4">
                                <UpdateContext.Provider value={starredBoard}>
                                    <UpdateStarredBoardButton />
                                </UpdateContext.Provider>
                                <CustomButton color="red" text="Delete" onClick={() => { HandleStarredBoardDelete(starredBoard.starredBoardId) }} />
                            </td>
                        </tr>
                    ))) : (
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <td className="px-6 py-4"></td>
                            <td className="px-6 py-4"></td>
                            <td className="px-6 py-4"></td>
                            <td className="px-6 py-4"></td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}
export default StarredBoardsTable;

