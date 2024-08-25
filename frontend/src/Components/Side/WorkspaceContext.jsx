import React, {createContext, useContext , useState, useEffect} from 'react';
import { getDataWithId, deleteData, postData, getDataWithIds } from '../../Services/FetchService';
import myImage from './background.jpg';
import { MainContext } from '../../Pages/MainContext';


export const WorkspaceContext = createContext();

export const WorkspaceProvider = ({ children }) => {
    const mainContext = useContext(MainContext);
   // const [WorkspaceId, setWorkspaceId] = useState(mainContext.workspaceId);
    const [userInfo, setUserInfo] = useState(mainContext.userInfo);
    
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
    const [tasks, setTasks] = useState([]);
    const [members, setMembers] = useState([]);
    const [roli, setRoli]=useState("Member");
    const [isInviteModalOpen, setIsInviteModalOpen]= useState(false);
    const userId = mainContext.userInfo.userId;
    const WorkspaceId = mainContext.workspaceId;
    useEffect(() => {

        const getWorkspace = async () => {
            try {
                const workspaceResponse = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspaceById?workspaceId', WorkspaceId);
                const workspaceData = workspaceResponse.data;
                console.log('Workspace data: ', workspaceData);
                setWorkspace(workspaceData);

            } catch (error) {
                console.error(error.message);
            }
        };
        getWorkspace();
        console.log("Workspace fetched", workspace);
        
    }, [WorkspaceId]);

 
    useEffect(()=>{
        const ownerId = workspace? workspace.ownerId : '';
        if (userId === ownerId) {
        setRoli("Owner");
    } else {
        setRoli("Member");
    }
    console.log("OwnerId: ",ownerId," UserId:" ,userId);
    console.log("Roli ", roli);
}, [WorkspaceId, userId, workspace]);
 
    
const workspaceTitle = workspace ? workspace.title : 'Workspace';
    useEffect(() => {
        const getBoards = async () => {
            try {
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
            } catch (error) {
                console.error(error.message);
                setBoards([]);
                setStarredBoards([]);
            }
        };
        getBoards();
        console.log("Starred boards: ",starredBoards);
        console.log("All boards",boards);
    }, [WorkspaceId, userId, selectedSort]);




    useEffect(() => {
        const getMembers = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/Members/getAllMembersByWorkspace?workspaceId', WorkspaceId);
                const data = response.data;
                if (data && Array.isArray(data) && data.length>0) {
                    setMembers(data);
                } else {
                    console.log('Data is null, not as an array or empty: ',data);
                }
            } catch (error) {
                console.error(error.message);
                setMembers([]);
            }
        };
        getMembers();
        console.log('Members fetched: ',members);
    },[WorkspaceId, workspace, userId]);

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
          window.location.href = '/workspaces';
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
            window.location.href = '/workspaces';
        }
        catch(error){
            console.error('Error deleting workspace:', error.message);
        }
     };

     const getTasks =async ()=>{

        try{
            const tasksResponse = await getDataWithId('http://localhost:5157/backend/task/GetTasksByWorkspaceId?workspaceId', WorkspaceId);
            const tasksData = tasksResponse.data;
            console.log("Tasks data: ",tasksData);
            setTasks(tasksData);
        }catch (error) {
            console.error(error.message);
        }
    };


    useEffect(()=>{
            getTasks();
            console.log("Workspace id ",WorkspaceId);
            console.log("Tasks fetched: ",tasks);
        }, [WorkspaceId]);
    
        const getInitials = (firstName, lastName) => {
            if (!firstName || !lastName) {
                return '';
            }
            return `${firstName.charAt(0)}${lastName.charAt(0)}`;
        };

        const getInitialsFromFullName = (fullName) => {
            if (!fullName) {
                return '';
            }
        
            // Split the full name by spaces
            const nameParts = fullName.trim().split(' ');
       
            const firstName = nameParts[0];
            const lastName = nameParts[nameParts.length - 1];
        
            // Return the initials
            return `${firstName.charAt(0).toUpperCase()}${lastName.charAt(0).toUpperCase()}`;
        };
        
        const openInviteModal = () => setIsInviteModalOpen(true);
        const closeInviteModal = () => setIsInviteModalOpen(false);

        const [sentInvites, setSentInvites] = useState([]);
        const [inviteeDetails, setInviteeDetails] = useState([]);
        const [workspaceTitles, setWorkspaceTitles] = useState([]);
    
        const getSentInvites = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/invite/GetInvitesByWorkspace?workspaceId', WorkspaceId);
                const data = response.data;
                console.log("Sent Invites fetched: ", data);
    
                const pendingInvites = data.filter(invite =>invite.inviteStatus === "Pending");
                console.log("Pending invites: ",pendingInvites);
                setSentInvites(pendingInvites);
    
                // Fetch inviter details for each invite
                const invited = await Promise.all(pendingInvites.map(async invite => {
                    const responseInvitee = await getDataWithId('http://localhost:5157/backend/user/adminUserID?userId', invite.inviteeId);
                    return responseInvitee.data;
                }));
                const workspaceTitlesData = await Promise.all(pendingInvites.map(async invite => {
                    const responseWorkspace = await getDataWithId('http://localhost:5157/backend/workspace/getWorkspaceById?workspaceId', invite.workspaceId);
                    return responseWorkspace.data.title; // Assuming the workspace object has a 'title' field
                }));
    
                setInviteeDetails(invited);
                setWorkspaceTitles(workspaceTitlesData);
            } catch (error) {
                console.log("Error fetching invites: ", error.message);
            }
        };
    
        useEffect(() => {
            getSentInvites();
        }, [WorkspaceId, workspace]);
    
    
        const handleDeleteInvite = async(inviteId) => {
            console.log("Deleting invite with id: ", inviteId);
            try{
                const response = await deleteData(`http://localhost:5157/backend/invite/DeleteInviteById?InviteId=${inviteId}`);
                console.log("Deleting invite response: ",response);
                getSentInvites();
            }
            catch(error){
                console.error("Error deleting invite ",error.message);
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
            tasks,
            setTasks,
            getTasks,
            openInviteModal,
            closeInviteModal,
            isInviteModalOpen,
            getInitials,
            handleDeleteInvite,
            getSentInvites,
            sentInvites, 
            setSentInvites,
            inviteeDetails, 
            setInviteeDetails, 
            workspaceTitles, 
            setWorkspaceTitles,
            getInitialsFromFullName
        }}>
            {children}
        </WorkspaceContext.Provider>
    );
};
