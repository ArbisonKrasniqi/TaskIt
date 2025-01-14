import React,{ useState, useEffect, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import BackroundsTable from "./BackgroundsTable";
import UpdateBackgroundModal from "./Modals/UpdateBackgroundModal";
import BackgroundCreationModal from "./Modals/BackgroundCreationModal";

export const BackgroundContext = createContext();

const BackgroundsList = () => {
    const [backgrounds, setBackgrounds] = useState([]);
    const [showBackgroundsErrorModal, setShowBackgroundsErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const [backgroundToUpdate, setBackgroundToUpdate] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const getBackgrounds = async () => {
        try {
            const data = await getData("http://localhost:5127/backend/background/GetAllBackgrounds");
            setBackgrounds(data.data);
        } catch (error) {
            setErrorMessage("Theres been an error fetching backgrounds");
            setShowBackgroundsErrorModal(true);
        }
    };

    const handleCreateBackgroundClick = () => {
        setIsModalOpen(true);
      };

      const handleCreateBackground = (newBackground) => {
        setBackgrounds([...backgrounds, newBackground]);
        setIsModalOpen(false);
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
        getBackgrounds,
        handleCreateBackground
    };

    return(
        <BackgroundContext.Provider value={contextValue}>
            <div onClick={handleCreateBackgroundClick} className="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group">
                <svg className="flex-shrink-0 w-5 h-5 text-gray-500 transition duration-75 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" >
                    <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" d="m2.25 15.75 5.159-5.159a2.25 2.25 0 0 1 3.182 0l5.159 5.159m-1.5-1.5 1.409-1.409a2.25 2.25 0 0 1 3.182 0l2.909 2.909m-18 3.75h16.5a1.5 1.5 0 0 0 1.5-1.5V6a1.5 1.5 0 0 0-1.5-1.5H3.75A1.5 1.5 0 0 0 2.25 6v12a1.5 1.5 0 0 0 1.5 1.5Zm10.5-11.25h.008v.008h-.008V8.25Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z" />
                </svg>
                <span className="flex-1 ms-3 whitespace-nowrap">Create Background</span>
            </div>
            <BackroundsTable/>
            {showUpdateInfoModal && <UpdateBackgroundModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
            <BackgroundCreationModal
                open={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onBackgroundCreated={handleCreateBackground}
            />
        </BackgroundContext.Provider>
    );
}

export default BackgroundsList;