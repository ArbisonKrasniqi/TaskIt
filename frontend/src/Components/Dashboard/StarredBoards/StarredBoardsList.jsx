import {useState, useEffect, useContext, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import StarredBoardsTable from "./StarredBoardsTable";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";

export const StarredBoardsContext = createContext();

const StarredBoardsList = () =>{
    const [StarredBoards, setStarredBoards] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const getStarredBoards = async ()=>{
        try{
            const response = await getData('http://localhost:5157/backend/starredBoard/GetAllStarredBoards');
            setStarredBoards(response.data);
        }catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowStarredBoardsErrorModal(true);
        }
    };
    useEffect(()=>{
        getStarredBoards();
    }, []);

    const contextValue = {StarredBoards, setStarredBoards, getStarredBoards};

        return(
            <StarredBoardsContext.Provider value={contextValue}>
                <StarredBoardsTable/>
                {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
            </StarredBoardsContext.Provider>
        );
}
export default StarredBoardsList