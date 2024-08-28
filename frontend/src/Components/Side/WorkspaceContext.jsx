import React, {createContext, useContext , useState, useEffect} from 'react';
import { getDataWithId, deleteData, postData, getDataWithIds } from '../../Services/FetchService';
import myImage from './background.jpg';
import { MainContext } from '../../Pages/MainContext';
import { useNavigate } from 'react-router-dom';

export const WorkspaceContext = createContext();

export const WorkspaceProvider = ({ children }) => {
    const mainContext = useContext(MainContext);
    const navigate = useNavigate();

    const [open, setOpen] = useState(true);
    const [workspace, setWorkspace] = useState(null);
    const [workspaces, setWorkspaces] = useState([]);
    const [boards, setBoards] = useState([]);
    const [starredBoards, setStarredBoards]=useState([]);
    const [selectedSort, setSelectedSort] = useState('Alphabetically');
    const [openModal, setOpenModal] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState(null);
    const [hoveredSIndex, setHoveredSIndex] = useState(null);
    const [hoveredBoardIndex, setHoveredBoardIndex] =useState(null);
    const [hoveredBoardSIndex, setHoveredBoardSIndex] =useState(null);
    const [hover, setHover] = useState(false);
    const [openSortModal, setOpenSortModal] = useState(false);
    const [selectedBoardTitle, setSelectedBoardTitle] = useState("");
    const [openCloseModal, setOpenCloseModal] = useState(false);
    const [openClosedBoardsModal, setOpenClosedBoardsModal] = useState(false);
    const [showLimitModal, setShowLimitModal] = useState(false);
    const [showDeleteWorkspaceModal, setShowDeleteWorkspaceModal]= useState(false);
    const boardCount = boards.length+starredBoards.length;
   
    const [members, setMembers] = useState([]);
    const [roli, setRoli]=useState("Member");
    
    const userId = mainContext.userInfo.userId;
    const WorkspaceId = mainContext.workspaceId;

    useEffect(() => {
        const getWorkspaces = async () => {
            try {
                if (userId) {
                    const workspacesResponse = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspacesByMemberId?memberId', userId);
                    const workspacesData = workspacesResponse.data;
                    if (workspacesData && Array.isArray(workspacesData) && workspacesData.length > 0) {
                        setWorkspaces(workspacesData);
                    } else {
                        setWorkspaces([]);
                        console.log("There are no workspaces");
                    }
                }
                //Waiting for userId
            } catch (error) {
                console.error("There has been an error fetching workspaces")
                setWorkspaces([]);
            }
        };
        getWorkspaces();
        
        // const interval = setInterval(getWorkspaces, 5 * 1000);
        // return () => clearInterval(interval); //Get workspaces every 5 seconds
    }, [userId, mainContext.userInfo.accessToken]);
    useEffect(() => {
        const getWorkspace = async () => {
            try {
                if (WorkspaceId) {
                    const workspaceResponse = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspaceById?workspaceId', WorkspaceId);
                    const workspaceData = workspaceResponse.data;
                    //console.log('Workspace data: ', workspaceData);
                    setWorkspace(workspaceData);
                }
            } catch (error) {
                console.log(error.response.data);
                navigate('/main/workspaces'); //Nese ska qasje, shko tek workspaces
            }
        };
        getWorkspace();

        // const interval = setInterval(getWorkspace, 5 * 1000);
        // return () => clearInterval(interval); //Get workspace every 5 seconds
    }, [WorkspaceId, userId, mainContext.userInfo.accessToken]);//userid

 
    useEffect(()=>{
        if (workspace && WorkspaceId && userId) {
            const ownerId = workspace.ownerId;
            if (userId === ownerId) {
                setRoli("Owner");
                //console.log("Set as owner with id", ownerId);
            } else {
                setRoli("Member");
                //console.log("Set as member with id", userId);
            }
        }
    }, [WorkspaceId, userId, workspace, mainContext.userInfo.accessToken]);
 
    
    const workspaceTitle = workspace ? workspace.title : 'Workspace';
    useEffect(() => {
        const getBoards = async () => {
            try {
                if (WorkspaceId && workspace && userId) {
                const boardsResponse = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId?workspaceId', WorkspaceId);
                const allBoards = boardsResponse.data;
            
                const starredResponse = await getDataWithId('http://localhost:5157/backend/starredBoard/GetStarredBoardsByWorkspaceId?workspaceId', WorkspaceId);
                const starredBoards = starredResponse.data;

                const starredBoardsIds = new Set(starredBoards.map(board=>board.boardId));

                const nonStarred = allBoards.filter(board=> !starredBoardsIds.has(board.boardId));
                const starred = allBoards.filter(board=>starredBoardsIds.has(board.boardId));

                let sortedNonStarred = nonStarred;
                if (selectedSort === 'Alphabetically') {
                    sortedNonStarred = sortAlphabetically(nonStarred);
                }
                    setBoards(sortedNonStarred);
                    setStarredBoards(starred);
                }
                } catch (error) {
                    console.error(error.response.data);
                    setBoards([]);
                    setStarredBoards([]);
                }
        };
        getBoards();
        // console.log("Starred boards: ",starredBoards);
        // console.log("All boards",boards);
    }, [WorkspaceId, userId, workspace, selectedSort, mainContext.userInfo.accessToken]);




    useEffect(() => {
        const getMembers = async () => {
            try {
                if (workspace && WorkspaceId && userId) {
                    const response = await getDataWithId('/backend/Members/getAllMembersByWorkspace?workspaceId', WorkspaceId);
                    const data = response.data;
                    if (data && Array.isArray(data) && data.length>0) {
                        setMembers(data);
                    } else {
                        console.log("There are no members");
                    }
                }
                //Loading
            } catch (error) {
                console.error(error.message);
                setMembers([]);
            }
        };
        getMembers();
        //console.log('Members fetched: ',members);
    },[WorkspaceId, workspace, userId, mainContext.userInfo.accessToken]);

    const handleCreateBoard = (newBoard) => {
        setBoards((prevBoards) => [...prevBoards, newBoard]);
    };

    
    const[updateWorkspaceModal, setUpdateWorkspaceModal] = useState(false);

    const handleWorkspaceUpdate = (updatedWorkspace) => {
        setWorkspace((prev) => ({
          ...prev,
          title: updatedWorkspace.Title,
          description: updatedWorkspace.Description,
        }));
      };




    const handleCloseBoard = async (boardId) => {
        try{
            const closedBoard = {
                boardId: boardId,
                userId: mainContext.userInfo.userId,
                
            };
            const response = await postData('http://localhost:5157/backend/board/Close', closedBoard);
            console.log("Board closed ",response.data);
           
            
            setBoards((prevBoards) => prevBoards.filter((b)=> b.boardId !== boardId));
            setStarredBoards((prevStarredBoards) => prevStarredBoards.filter((b) => b.boardId !== boardId));
            // const closedBoardData = response.data;
            // setClosedBoards((prevClosedBoards)=> [...prevClosedBoards, closedBoardData]);
        }
        catch(error){
            console.error("Error closing board:",error.response?.data || error.message);
        }
    }

    


    
    const handleCreateWorkspace = (newWorkspace) => {
      setWorkspaces((prevWorkspaces) => [...prevWorkspaces, newWorkspace]);
  }

    const sortAlphabetically = (boards) => {
        return boards.slice().sort((a, b) => a.title.localeCompare(b.title));
    };

    const sortByRecent = async () => {
        const dataResponse = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId?workspaceId', WorkspaceId);
        return dataResponse.data;
    };

    const handleSortChange = async (sortType) => {
        setSelectedSort(sortType);
        let sortedBoards = boards;
        if (sortType === 'Alphabetically') {
            sortedBoards = sortAlphabetically(boards);
        } else {
            sortedBoards = await sortByRecent();
        }
        setBoards(sortedBoards);
    };
    const handleStarBoard = async (board) => {
        const isStarred = starredBoards.some(b => b.boardId === board.boardId);
        const data = {
            BoardId: board.boardId,
            UserId: userId,
            WorkspaceId: WorkspaceId,
        };
        try {
            if (isStarred) {
    
                    // Unstar the board
                    await deleteData('http://localhost:5157/backend/starredBoard/UnstarBoard', data);
                } else {
                    // Star the board
                    await postData('http://localhost:5157/backend/starredBoard/StarBoard', data);
                }
                
                // Re-fetch boards and starred boards
                const boardsResponse = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId?workspaceId', WorkspaceId);
                const allBoards = boardsResponse.data;
        
                const starredResponse = await getDataWithId('http://localhost:5157/backend/starredBoard/GetStarredBoardsByWorkspaceId?workspaceId', WorkspaceId);
                const updatedStarredBoards = starredResponse.data;
        
                const starredBoardsIds = new Set(updatedStarredBoards.map(board => board.boardId));
        
                const nonStarred = allBoards.filter(board => !starredBoardsIds.has(board.boardId));
                const starred = allBoards.filter(board => starredBoardsIds.has(board.boardId));
        
                let sortedNonStarred = nonStarred;
                if (selectedSort === 'Alphabetically') {
                    sortedNonStarred = sortAlphabetically(nonStarred);
                }
        
                setStarredBoards(starred);
                setBoards(sortedNonStarred);
        } catch (error) {
            console.error("Error starring/unstarring the board:", error.message);
        }
    };
    const getBackgroundImageUrl = (board) => {
        // const background = backgrounds.find(b=>b.backgroundId === board.backgroundId);
        // return background? background.imageUrl : '';
        return myImage;
    };


    
    const handleDeleteWorkspace = async(workspaceId) =>{
        console.log('Deleting workspace with Id: ', workspaceId);
        try{
            const response = await deleteData('http://localhost:5157/backend/workspace/DeleteWorkspace', { workspaceId: workspaceId });
            console.log('Deleting workspace response:', response);
            navigate('/main/workspaces');
        }
        catch(error){
            console.error('Error deleting workspace:', error.message);
        }

     };
     const handleLeaveWorkspace = async(workspaceId, userId)=>{
        console.log('Leaving workspace with Id: ',workspaceId);
        try{
            const response = await deleteData('http://localhost:5157/backend/Members/RemoveMember', {UserId: userId, WorkspaceId: workspaceId});
            console.log('Leaving workspace response:', response);
            navigate(`/main/workspaces`);
        }
        catch(error){
            console.error('Error deleting workspace:', error.message);
        }
     };

    return (
        <WorkspaceContext.Provider value={{
            WorkspaceId,
            workspace,
            workspaces,
            setWorkspaces,
            boards,
            selectedSort,
            open,
            setOpen,
            workspaceTitle,
            setHover,
            hover,
            openCloseModal,
            setOpenSortModal,
            setOpenCloseModal,
            openSortModal,
            setOpenModal,
            openModal,
            setHoveredIndex,
            hoveredIndex,
            hoveredSIndex,
            setHoveredSIndex,
            setSelectedBoardTitle,
            selectedBoardTitle,
            roli,
            hoveredBoardIndex,
            setHoveredBoardIndex,
            hoveredBoardSIndex,
            setHoveredBoardSIndex,
            sortAlphabetically,
            sortByRecent,
            handleSortChange,
            handleStarBoard,
            getBackgroundImageUrl,
            handleCloseBoard,
            setOpenClosedBoardsModal,
            openClosedBoardsModal,
            setBoards,
            showLimitModal,
            setShowLimitModal,
            boardCount,
            setWorkspace,
            handleCreateWorkspace,
            members,
            updateWorkspaceModal,
            setUpdateWorkspaceModal,
            handleWorkspaceUpdate,
            handleCreateBoard,
            starredBoards,
            handleDeleteWorkspace,
            showDeleteWorkspaceModal,
            setShowDeleteWorkspaceModal,
            userId,
            handleLeaveWorkspace,
        }}>
            {children}
        </WorkspaceContext.Provider>
    );
};
