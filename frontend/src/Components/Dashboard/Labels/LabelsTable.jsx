import { createContext, useContext, useState } from "react";
import { LabelsContext } from "./LabelsList";
import { DashboardContext } from "../../../Pages/dashboard";
import { deleteData } from "../../../Services/FetchService";
import UpdateLabelButton from "./Buttons/UpdateLabelButton";
import CustomButton from "../Buttons/CustomButton";

export const UpdateContext = createContext();

const LabelsTable = () => {
    const labelsContext = useContext(LabelsContext);
    const dashboardContext = useContext(DashboardContext);
    const [searchQuery, setSearchQuery] = useState('');

    const HandleLabelDelete = (id) => {
        async function deleteLabel() {
            try {
                const data = {
                    labelId: id
                };
                const response = await deleteData('/backend/label/DeleteLabel?LabelId', data);
                console.log(response);
                const updatedLabels = labelsContext.labels.filter(label => label.labelId !== id);
                labelsContext.setLabels(updatedLabels);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                labelsContext.getLabels();
            }
        }
        deleteLabel();
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }
    
    const labels = labelsContext.labels || [];
    const filteredLabels = labels.filter(label => {
        const labelIdMatch = label.labelId.toString().toLowerCase().includes(searchQuery);
        const nameMatch = label.name.toString().toLowerCase().includes(searchQuery);
        const colorMatch = label.color.toString().toLowerCase().includes(searchQuery);
        const dateAddedMatch = label.dateAdded.toString().toLowerCase().includes(searchQuery);
        const boardIdMatch = label.boardId.toString().toLowerCase().includes(searchQuery);
        return labelIdMatch || nameMatch || colorMatch || dateAddedMatch || boardIdMatch;
    });

    return (
        <div className='flex flex-col h-full'>
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for labels by ID, title, description, list ID, due date, or date added"
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
                            <th className="px-6 py-3">Label ID</th>
                            <th className="px-6 py-3">Name</th>
                            <th className="px-6 py-3">Color</th>
                            <th className="px-6 py-3">Board Id</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredLabels.length > 0 ? (
                            filteredLabels.map((label, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{label.labelId}</td>
                                    <td className="px-6 py-4">{label.name}</td>
                                    <td className="px-6 py-4">{label.color}</td>
                                    <td className="px-6 py-4">{label.boardId}</td>
                                    <td className="px-6 py-4">{label.dateAdded}</td>
                                    <td className="px-6 py-4">
                                        <UpdateContext.Provider value={label}>
                                            <UpdateLabelButton/>
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleLabelDelete(label.labelId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={6}>No labels found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default LabelsTable;