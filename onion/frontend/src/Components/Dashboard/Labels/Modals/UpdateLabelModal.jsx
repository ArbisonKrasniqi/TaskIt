import { useContext, useState } from "react";
import { UpdateContext } from "../LabelsTable";
import { LabelsContext } from "../LabelsList";
import { DashboardContext } from "../../../../Pages/dashboard";
import { putData } from "../../../../Services/FetchService";
import CustomButton from "../../Buttons/CustomButton";

const UpdateLabelModal = (props) => {

    const updateContext = useContext(UpdateContext);
    const labelsContext = useContext(LabelsContext);
    const dashboardContext = useContext(DashboardContext);

    const [name, setName] = useState(updateContext.name);
    const [boardId, setBoardId] = useState(updateContext.boardId);

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const data = {
                labelId: updateContext.labelId,
                name: name,
            }

            const response = await putData('/backend/label/UpdateLabel', data);
            console.log(response.data);

            const updatedLabels = labelsContext.labels.map(label => {
                if (label.labelId === updateContext.labelId) {
                    return {
                        ...label,
                        name: name,
                    };
                } else {
                    return label;
                }
            });

            labelsContext.setLabels(updatedLabels);
            props.setShowUpdateInfoModal(false);
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            labelsContext.getLabels();

            props.setShowUpdateInfoModal(false);
        }
    }

    return (
        <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
                <div className="mb-6">
                    <label htmlFor="name" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Name</label>
                    <input
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        type="text"
                        id="name"
                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                    />
                </div>
                <div className="flex justify-around">
                    <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed" />
                    <CustomButton type="submit" text="Update" color="longGreen" />
                </div>
            </form>
        </div>
    );
}

export default UpdateLabelModal;
