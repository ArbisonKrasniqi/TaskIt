import { getDataWithId } from "../../../Services/FetchService";
import { useContext, useState, useEffect, createContext } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";
import { useParams } from "react-router-dom";
import BoardActivityTable from "./BoardActivityTable";

export const BoardActivityContext = createContext();

const BoardActivityList = () => {
    const [activities, setActivities] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const {boardId} = useParams();

    const getActivity = async () => {
        try {
            if (boardId) {
                console.log(boardId);
                const response = await getDataWithId("/GetBoardActivityByBoardId?BoardId", boardId);
                setActivities(response.data.sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate)));
            }
        }
        catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    }

    useEffect(()=>{
        getActivity();
    }, []);
    
    const contextValue= {activities, setActivities, getActivity}
    return(
        <BoardActivityContext.Provider value={contextValue}>
            <BoardActivityTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </BoardActivityContext.Provider>
    );
}

export default BoardActivityList;