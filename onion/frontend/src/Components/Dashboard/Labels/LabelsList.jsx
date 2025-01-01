import { useState, createContext, useContext, useEffect } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import LabelsTable from "./LabelsTable";
import DashboardErrorModal from "../DashboardErrorModal";
import { getData, getDataWithId } from "../../../Services/FetchService";
import { useParams } from "react-router-dom";


export const LabelsContext = createContext();

const LabelsList = () => {
    const {boardId,taskId} = useParams();
    const [labels, setLabels] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const getLabels = async () => {
        try{
            if (boardId) {
                console.log('boardId:' +boardId);
                const labelsWithBoardId = await getDataWithId('/backend/label/GetLabelsByBoardId?boardId', boardId);
                setLabels (labelsWithBoardId.data);
            } else if (taskId) {
                console.log('taskId'+taskId);
                const labelsWithTaskId = await getDataWithId('/backend/label/GetLabelsByTaskId?taskId', taskId);
                setLabels(labelsWithTaskId.data);
            } else {
                console.log('ska id???');
                const allLabels = await getData("/backend/label/GetAllLabels");
                setLabels(allLabels.data);
            }
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };

    useEffect(()=>{
        getLabels();
    },[boardId]);

    const contextValue = {labels, setLabels, getLabels};
    return (
        <LabelsContext.Provider value={contextValue}>
            <LabelsTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/> }
        </LabelsContext.Provider>
    )
}

export default LabelsList;