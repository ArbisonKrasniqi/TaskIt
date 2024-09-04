import { createContext, useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard";
import { getData, getDataWithId } from "../../../Services/FetchService";
import ChecklistItemsTable from "./ChecklistItemsTable";
import DashboardErrorModal from "../DashboardErrorModal";

export const ChecklistItemsContext = createContext();


const ChecklistItemsList = () => {
    const { checklistId } = useParams();
    const [checklistItems, setChecklistItems] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const getChecklistItems = async () => {
        try {
            if (checklistId) {
                const checklistItemsWithChecklistId = await getDataWithId('/backend/checklistItems/GetChecklistItemByChecklistId?checklistId', checklistId);
                setChecklistItems(checklistItemsWithChecklistId.data);
            } else {
                const allChecklistItems = await getData('/backend/checklistItems/GetAllChecklistItems');
                setChecklistItems(allChecklistItems.data);
            }
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };

    useEffect(() => {
        getChecklistItems();
    }, [checklistId]);

    const contextValue = {checklistItems, setChecklistItems, getChecklistItems};

    return (
        <ChecklistItemsContext.Provider value={contextValue}>
            <ChecklistItemsTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </ChecklistItemsContext.Provider>
    );
    
}

export default ChecklistItemsList;