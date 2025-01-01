import {useState, useEffect, useContext, createContext} from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import StarredBoardsTable from "./StarredBoardsTable";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";
import { useParams } from "react-router-dom";

export const StarredBoardsContext = createContext();

const StarredBoardsList = () =>{
    const {userId} = useParams();
    const [StarredBoards, setStarredBoards] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const getStarredBoards = async ()=>{
        try{
            if (userId) {
                const userStarredBoards = await getDataWithId('/backend/starredBoard/GetStarredBoardsByUserId?userId', userId);
                setStarredBoards(userStarredBoards.data);
            } 
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
                {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
                <StarredBoardsTable/>
            </StarredBoardsContext.Provider>
        );
}
export default StarredBoardsList