import { createContext, useContext, useState } from "react";
import { useNavigate } from "react-router-dom"
import { DashboardContext } from "../../../Pages/dashboard";
import { deleteData } from "../../../Services/FetchService";
import CustomButton from "../Buttons/CustomButton";
import { ChecklistItemsContext } from "./ChecklistItemsList";
import UpdateChecklistItemButton from "./Buttons/UpdateChecklistItemButton";

export const UpdateContext = createContext();

const ChecklistItemsTable = () => {
    const checklistItemsContext = useContext(ChecklistItemsContext);
    const dashboardContext = useContext(DashboardContext);

    const [searchQuery, setSearchQuery] = useState('');

    const HandleChecklistItemDelete = (id) => {
        async function deleteChecklistItem(){
            try {
                const data = {
                    checklistItemId: id
                };
                const response = await deleteData(`/backend/checklistItems/DeleteChecklistItem`, data);
                console.log(response);
                const updatedChecklistItems = checklistItemsContext.checklistItems.filter(ci => ci.checklistItemId !== id);
                checklistItemsContext.setChecklistItems(updatedChecklistItems);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                checklistItemsContext.getChecklistItems();
            }
        }
        deleteChecklistItem();
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    const checklistItems = checklistItemsContext.checklistItems || [];
    const filteredChecklistItems = checklistItems.filter(ci => {
        const checklistItemIdMatch = ci.checklistItemId.toString().toLowerCase().includes(searchQuery);
        const contentMatch = ci.content.toString().toLowerCase().includes(searchQuery);
        const dateCreatedMatch = ci.dateCreated.toString().toLowerCase().includes(searchQuery);
        const checklistIdMatch = ci.checklistId.toString().toLowerCase().includes(searchQuery);
        const checkedMatch = ci.checked.toString().toLowerCase().includes(searchQuery);
        return checklistItemIdMatch || contentMatch || dateCreatedMatch || checklistIdMatch || checkedMatch;
    });

    return (
        <div className="flex flex-col h-full">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for checklist items by ID, content, checklistId, or date created"
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
                            <th className="px-6 py-3">ChecklistItem ID</th>
                            <th className="px-6 py-3">Content</th>
                            <th className="px-6 py-3">Checked</th>
                            <th className="px-6 py-3">Checklist ID</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredChecklistItems.length > 0 ? (
                            filteredChecklistItems.map((ci, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{ci.checklistItemId}</td>
                                    <td className="px-6 py-4">{ci.content}</td>
                                    <td className="px-6 py-4">{ci.checked+''}</td>
                                    <td className="px-6 py-4">{ci.checklistId}</td>
                                    <td className="px-6 py-4">{ci.dateCreated}</td>
                                    <td className="px-6 py-4">
                                        <UpdateContext.Provider value={ci}>
                                            <UpdateChecklistItemButton/>
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleChecklistItemDelete(ci.checklistItemId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No checklist items found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default ChecklistItemsTable;