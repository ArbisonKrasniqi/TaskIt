import { useState, useEffect, createContext } from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import ListsTable from "./ListsTable";
import ListsErrorModal from "./Modals/ListsErrorModal";
import { useParams } from "react-router-dom";

export const ListsContext = createContext();

const ListsList = () => {
    const { boardId } = useParams(); 
    const [lists, setLists] = useState(null);
    const [showListsErrorModal, setShowListsErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");

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
            setErrorMessage(error.message);
            setShowListsErrorModal(true);
        }        
    };

    useEffect(() => {
        getLists();
    }, [boardId]);

    const contextValue = {lists, setLists, getLists, showListsErrorModal, setShowListsErrorModal, errorMessage, setErrorMessage};

    return(
        <ListsContext.Provider value={contextValue}>
            <ListsTable/>
            {showListsErrorModal && <ListsErrorModal/>}
        </ListsContext.Provider>
    );
}

export default ListsList