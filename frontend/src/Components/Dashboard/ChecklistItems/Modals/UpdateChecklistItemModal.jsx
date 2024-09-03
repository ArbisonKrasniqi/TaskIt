import React, { useState, useContext} from 'react';
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../ChecklistItemsTable';
import CustomButton from '../../Buttons/CustomButton';
import { DashboardContext } from '../../../../Pages/dashboard';
import { ChecklistItemsContext } from '../ChecklistItemsList';

const UpdateChecklistModal = (props) => {
    const updateContext = useContext(UpdateContext);
    const checklistItemsContext = useContext(ChecklistItemsContext);
    const dashboardContext = useContext(DashboardContext);

    const [content, setContent] = useState(updateContext.content);
    const [checklistId, setChecklistId] = useState(updateContext.checklistId);
    const [checked, setChecked] = useState(updateContext.checked);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const data = {
                checklistItemId: updateContext.checklistItemId,
                content: content,
                checked: checked,
                checklistId: checklistId,
            };

            const response = await putData('/backend/checklistItems/UpdateChecklistItem', data);
            console.log(response.data);
            const updatedChecklistItems = checklistItemsContext.checklistItems.map(ci => {
                if(ci.checklistItemId === updateContext.checklistItemId){
                    return {
                        ...ci,
                        content: content,
                        checklistId: checklistId,
                        checked: checked
                    };
                } else {
                    return ci;
                }
            });

            checklistItemsContext.setChecklistItems(updatedChecklistItems);
            props.setShowUpdateInfoModal(false);
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            checklistItemsContext.getChecklistItems();

            props.setShowUpdateInfoModal(false);
        }
    }

    return(
        <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
                <div className="mb-6">
                    <label htmlFor="content" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Content</label>
                    <input value={content}
                        onChange={(e) => setContent(e.target.value)}
                        type="text"
                        id="content"
                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                </div>
                <div className="grid md:grid-cols-2 md:gap-6">
                    <div className="mb-6">
                        <label htmlFor="checklistId" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">ChecklistId</label>
                        <input value={checklistId}
                            onChange={(e) => setChecklistId(e.target.value)}
                            type="text"
                            id="checklistId"
                            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                    </div>
                    <div className="mb-6 flex items-center">
                        <input 
                            type="checkbox" 
                            id="checked" 
                            checked={checked} 
                            onChange={(e) => setChecked(e.target.checked)} 
                            className="mr-2 bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:focus:ring-blue-500 dark:focus:border-blue-500" 
                        />
                        <label htmlFor="checked" className="text-sm font-medium text-gray-900 dark:text-white">Checked</label>
                    </div>
                </div>
                <div className="flex justify-around">
                    <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed"/>
                    <CustomButton type="submit" text="Update" color="longGreen"/>
                </div>
            </form>
        </div>    
    );
}

export default UpdateChecklistModal;
