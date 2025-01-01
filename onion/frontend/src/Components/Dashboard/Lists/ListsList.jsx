import { useState, useEffect, createContext, useContext } from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import ListsTable from "./ListsTable";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";

export const ListsContext = createContext();

const ListsList = () => {
    const { boardId } = useParams(); 
    
    const [lists, setLists] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const getLists = async () => {
        try {
            if (boardId) {
                const listsWithBoardId = await getDataWithId('/backend/list/GetListByBoardId?BoardId', boardId);
                setLists(listsWithBoardId.data);
            } else {
                const allLists = await getData('backend/list/getAllLists');
                setLists(allLists.data);
            }
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }        
    };

    useEffect(() => {
        getLists();
    }, [boardId]);

    const contextValue = {lists, setLists, getLists};

    return(
        <ListsContext.Provider value={contextValue}>
            <ListsTable/>
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </ListsContext.Provider>
    );
}

export default ListsList