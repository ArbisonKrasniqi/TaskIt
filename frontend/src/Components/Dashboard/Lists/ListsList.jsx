import { useState, useEffect, createContext } from "react";
import { getData } from "../../../Services/FetchService";
import ListsTable from "./ListsTable";
import ListsErrorModal from "./Modals/ListsErrorModal";

export const ListsContext = createContext();

const ListsList = () => {
    const [lists, setLists] = useState(null);
    const [showListsErrorModal, setShowListsErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");

    const getLists = async () => {
        try {
            const data = await getData('backend/list/getAllLists');
            setLists(data.data);
        } catch (error) {
            setErrorMessage(error.message);
            setShowListsErrorModal(true);
        }        
    };

    useEffect(() => {
        getLists();
    }, []);

    const contextValue = {lists, setLists, getLists, showListsErrorModal, setShowListsErrorModal, errorMessage, setErrorMessage};

    return(
        <ListsContext.Provider value={contextValue}>
            <ListsTable/>
            {showListsErrorModal && <ListsErrorModal/>}
        </ListsContext.Provider>
    );
}

export default ListsList