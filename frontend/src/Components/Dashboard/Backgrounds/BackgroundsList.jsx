import React,{ useState, useEffect, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import BackroundsTable from "./BackgroundsTable";
import UpdateBackgroundModal from "./Modals/UpdateBackgroundModal";

export const BackgroundContext = createContext();

const BackgroundsList = () => {
    const [backgrounds, setBackgrounds] = useState([]);
    const [showBackgroundsErrorModal, setShowBackgroundsErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const [backgroundToUpdate, setBackgroundToUpdate] = useState(null);

    const getBackgrounds = async () => {
        try {
            const data = await getData("http://localhost:5157/backend/background/GetAllBackgrounds");
            setBackgrounds(data.data);
        } catch (error) {
            setErrorMessage(error.message);
            setShowBackgroundsErrorModal(true);
        }
    };
    
    useEffect(() => { getBackgrounds(); }, []);

    const contextValue = {
        backgrounds,
        setBackgrounds,
        showBackgroundsErrorModal,
        setShowBackgroundsErrorModal,
        errorMessage,
        setErrorMessage,
        showUpdateInfoModal,
        setShowUpdateInfoModal,
        backgroundToUpdate,
        setBackgroundToUpdate,
        getBackgrounds
    };

    return(
        <BackgroundContext.Provider value={contextValue}>
            <BackroundsTable/>
            {showUpdateInfoModal && <UpdateBackgroundModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </BackgroundContext.Provider>
    );
}

export default BackgroundsList;