import React, { useState, useContext, createContext } from 'react';
import UpdateListButton from './Buttons/UpdateListButton';
import CustomButton from '../Buttons/CustomButton';
import { ListsContext } from './ListsList';
import { deleteData } from '../../../Services/FetchService';
import { useNavigate } from 'react-router-dom';
import { DashboardContext } from '../../../Pages/dashboard';

export const UpdateContext = createContext();

const ListsTable = () => {
    const navigate = useNavigate();
    const listsContext = useContext(ListsContext);
    const [searchQuery, setSearchQuery] = useState('');
    const dashboardContext = useContext(DashboardContext);

    const HandleListDelete = (id) => {
        async function deleteList() {
            try {
                const data = {
                    listId: id
                };
                const response = await deleteData('/backend/list/DeleteList', data);
                console.log(response);
                const updatedLists = listsContext.lists.filter(list => list.listId !== id);
                listsContext.setLists(updatedLists);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                listsContext.getLists();
            }
        }
        deleteList();
    }

    const handleListRowClick = listId => {
        console.log(listId);
        navigate(`/dashboard/tasks/${listId}`);
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    // Ensure listsContext.lists is always an array
    const lists = listsContext.lists || [];
    const filteredLists = lists.filter(list => {
        const listIdMatch = list.listId.toString().toLowerCase().includes(searchQuery);
        const titleMatch = list.title.toLowerCase().includes(searchQuery);
        const boardIdMatch = list.boardId.toString().toLowerCase().includes(searchQuery);
        const dateCreatedMatch = list.dateCreated.toLowerCase().includes(searchQuery);
        
        return listIdMatch || titleMatch || boardIdMatch || dateCreatedMatch;
    });

    return (
        <div className="flex flex-col h-full">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for lists by ID, title, board ID, or date created"
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
                            <th className="px-6 py-3">List ID</th>
                            <th className="px-6 py-3">Title</th>
                            <th className="px-6 py-3">Board ID</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredLists.length > 0 ? (
                            filteredLists.map((list, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{list.listId}</td>
                                    <td className="px-6 py-4">{list.title}</td>
                                    <td className="px-6 py-4">{list.boardId}</td>
                                    <td className="px-6 py-4">{list.dateCreated}</td>
                                    <td className="px-6 py-4">
                                    <CustomButton 
                                            onClick={() => handleListRowClick(list.listId)}
                                            type="button"
                                            text="Open"
                                        />
                                        <UpdateContext.Provider value={list}>
                                            <UpdateListButton/>
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleListDelete(list.listId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No lists found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default ListsTable;
