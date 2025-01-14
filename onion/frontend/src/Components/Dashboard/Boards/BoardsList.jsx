import { useState, useEffect, createContext, useContext } from 'react';
import { getData, getDataWithId } from '../../../Services/FetchService.jsx';
import BoardsTable from './BoardsTable.jsx';
import DashboardErrorModal from '../DashboardErrorModal.jsx';
import UpdateBoardModal from './Modals/UpdateBoardModal.jsx';
import { useParams } from 'react-router-dom';
import { DashboardContext } from '../../../Pages/dashboard.jsx';


export const BoardsContext = createContext();

const BoardsList = (workspaceIdParam) => {
    const { workspaceId } = useParams();
    const [boards, setBoards] = useState(null);
    const dashboardContext = useContext(DashboardContext);

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const [boardToUpdate, setBoardToUpdate] = useState(null);

    const getBoards = async () => {
        try {
            if (workspaceId) {
                const boardsWithWorkspaceId = await getDataWithId("/backend/board/GetBoardsByWorkspaceId?workspaceId", workspaceId);
                setBoards(boardsWithWorkspaceId.data);
            } else {
                const allBoards = await getData("http://localhost:5127/backend/board/GetAllBoards");
                setBoards(allBoards.data);
            }
            
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };

    useEffect(() => { getBoards(); }, [workspaceId]);

    const contextValue = { 
        boards, setBoards, 
        getBoards, 
        showUpdateInfoModal, setShowUpdateInfoModal, 
        boardToUpdate, setBoardToUpdate 
    };

    return (
        <BoardsContext.Provider value={contextValue}>
            <BoardsTable />
            {dashboardContext.ShowDashboardErrorModal && <DashboardErrorModal />}
            {showUpdateInfoModal && <UpdateBoardModal setShowUpdateInfoModal={setShowUpdateInfoModal} />}
        </BoardsContext.Provider>
    );
};

export default BoardsList;
