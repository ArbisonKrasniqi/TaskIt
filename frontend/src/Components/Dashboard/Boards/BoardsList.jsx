import { useState, useEffect, createContext } from 'react';
import { getData } from '../../../Services/FetchService.jsx';
import BoardsTable from './BoardsTable.jsx';
import BoardErrorModal from './Modals/BoardErrorModal.jsx';
import UpdateBoardModal from './Modals/UpdateBoardModal.jsx';

export const BoardsContext = createContext();

const BoardsList = () => {
    const [boards, setBoards] = useState(null);
    const [showBoardsErrorModal, setShowBoardsErrorModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const [boardToUpdate, setBoardToUpdate] = useState(null);

    const getBoards = async () => {
        try {
            const data = await getData("http://localhost:5157/backend/board/GetAllBoards");
            setBoards(data.data);
        } catch (error) {
            setErrorMessage(error.message);
            setShowBoardsErrorModal(true);
        }
    };

    useEffect(() => { getBoards(); }, []);

    const contextValue = { 
        boards, setBoards, 
        getBoards, 
        showBoardsErrorModal, setShowBoardsErrorModal, 
        errorMessage, setErrorMessage, 
        showUpdateInfoModal, setShowUpdateInfoModal, 
        boardToUpdate, setBoardToUpdate 
    };

    return (
        <BoardsContext.Provider value={contextValue}>
            <BoardsTable />
            {showBoardsErrorModal && <BoardErrorModal />}
            {showUpdateInfoModal && <UpdateBoardModal setShowUpdateInfoModal={setShowUpdateInfoModal} />}
        </BoardsContext.Provider>
    );
};

export default BoardsList;
