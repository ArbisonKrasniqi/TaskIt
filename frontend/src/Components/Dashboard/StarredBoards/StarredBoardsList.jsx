import {useState, useEffect, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import StarredBoardsTable from "./StarredBoardsTable";
import StarredBoardsErrorModal from "./Modals/StarredBoardsErrorModal"
export const StarredBoardsContext = createContext();

const StarredBoardsList = () =>{
    const [StarredBoards, setStarredBoards] = useState([]);
    const [showStarredBoardsErrorModal, setShowStarredBoardsErrorModal] =useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");

    const getStarredBoards = async ()=>{
        try{
            const response = await getData('http://localhost:5157/backend/starredBoard/GetAllStarredBoards');
            setStarredBoards(response.data);
        }catch(error){
            setErrorMessage(error.message);
            setShowStarredBoardsErrorModal(true);
        }
    };
    useEffect(()=>{
        getStarredBoards();
    }, []);

    const contextValue = {StarredBoards, setStarredBoards, getStarredBoards, 
        showStarredBoardsErrorModal, setShowStarredBoardsErrorModal, errorMessage, setErrorMessage};

        return(
            <StarredBoardsContext.Provider value={contextValue}>
                <StarredBoardsTable/>
                {showStarredBoardsErrorModal && <StarredBoardsErrorModal/>}
            </StarredBoardsContext.Provider>
        );
}
export default StarredBoardsList