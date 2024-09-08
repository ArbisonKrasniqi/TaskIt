import { createContext, useContext, useState } from "react";
import { useNavigate } from "react-router-dom"
import { ChecklistsContext } from "./ChecklistsList";
import { DashboardContext } from "../../../Pages/dashboard";
import { deleteData } from "../../../Services/FetchService";
import CustomButton from "../Buttons/CustomButton";
import UpdateChecklistButton from "./Buttons/UpdateChecklistButton";

export const UpdateContext = createContext();

const ChecklistsTable = () => {
    const navigate = useNavigate();
    const checklistsContext = useContext(ChecklistsContext);
    const dashboardContext = useContext(DashboardContext);
    
    const [searchQuery, setSearchQuery] = useState('');

    const HandleChecklistDelete = (id) => {
        async function deleteChecklist(){
            try {
                const data = {
                    checklistId: id
                };
                const response = await deleteData(`backend/checklist/DeleteChecklist`, data);
                console.log(response);
                const updatedChecklists = checklistsContext.checklists.filter(cl => cl.checklistId !== id);
                checklistsContext.setChecklists(updatedChecklists);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                checklistsContext.getChecklists();
            }
        }
        deleteChecklist();
    }

    const handleChecklistRowClick = checklistId => {
        console.log(checklistId);
        navigate(`/dashboard/checklistItems/${checklistId}`);
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    const checklists = checklistsContext.checklists || [];
    const filteredChecklists = checklists.filter(cl => {
        const checklistIdMatch = cl.checklistId.toString().toLowerCase().includes(searchQuery);
        const titleMatch = cl.title.toString().toLowerCase().includes(searchQuery);
        const dateCreatedMatch = cl.dateCreated.toString().toLowerCase().includes(searchQuery);
        const taskIdMatch = cl.taskId.toString().toLowerCase().includes(searchQuery);

        return checklistIdMatch || titleMatch || dateCreatedMatch || taskIdMatch;
    });

    return (
        <div className="flex flex-col h-full">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for checklists by ID, title, task ID, or date created"
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
                            <th className="px-6 py-3">Checklist ID</th>
                            <th className="px-6 py-3">Title</th>
                            <th className="px-6 py-3">Task ID</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredChecklists.length > 0 ? (
                            filteredChecklists.map((cl, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{cl.checklistId}</td>
                                    <td className="px-6 py-4">{cl.title}</td>
                                    <td className="px-6 py-4">{cl.taskId}</td>
                                    <td className="px-6 py-4">{cl.dateCreated}</td>
                                    <td className="px-6 py-4">
                                    <CustomButton 
                                            onClick={() => handleChecklistRowClick(cl.checklistId)}
                                            type="button"
                                            text="Open"
                                        />
                                        <UpdateContext.Provider value={cl}>
                                            <UpdateChecklistButton/>
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleChecklistDelete(cl.checklistId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No checklists found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default ChecklistsTable;