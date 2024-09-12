import React, {createContext, useContext , useState, useEffect} from 'react';
import { getDataWithId, deleteData, postData, getDataWithIds } from '../../Services/FetchService';
import myImage from './background.jpg';
import { MainContext } from '../../Pages/MainContext';
import { useNavigate, useParams } from 'react-router-dom';
import { getData } from '../../Services/FetchService';

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
    const [tasks, setTasks] = useState([]);
    const [members, setMembers] = useState([]);
    const [roli, setRoli]=useState("Member");
    const [isInviteModalOpen, setIsInviteModalOpen]= useState(false);
    const userId = mainContext.userInfo.userId;
    const WorkspaceId = mainContext.workspaceId;
    const [board, setBoard] = useState(null);
    const {boardId, listId, taskId }= useParams();
    // const boardId = mainContext.boardId;
    const [lists, setLists] = useState([]); 
    const [list, setList] = useState(null);
    const{} = useParams();
    const [checklists, setChecklists] = useState([]);
    const [activities, setActivities]= useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [recentBoards, setRecentBoards] = useState([]);


        const getWorkspaces = async () => {
            try {
                setIsLoading(true);
                    const workspacesResponse = await getDataWithId('http://localhost:5157/backend/workspace/GetWorkspacesByMemberId?memberId', userId);
                    const workspacesData = workspacesResponse.data;
                    if (workspacesData && Array.isArray(workspacesData) && workspacesData.length > 0) {
                        setWorkspaces(workspacesData);

                        const recentBoards = workspacesData
                        .flatMap(workspace => workspace.boards)
                        .sort((a, b) => new Date(b.dateCreated) - new Date(a.dateCreated))
                        .slice(0, 5);

                        console.log("PARAAA RECENTTTT: ",recentBoards);
                        

                        setRecentBoards(recentBoards);
                    } else {
                        setWorkspaces([]);
                        console.log("There are no workspaces");
                    }
                //Waiting for userIdn
            } catch (error) {
                console.error("There has been an error fetching workspaces")
                setWorkspaces([]);
            } finally {
                setIsLoading(false);
            }
        };
    useEffect(() => {
        if (userId) {
            getWorkspaces();
        }
    }, [userId, board ,mainContext.userInfo]);
        // const interval = setInterval(getWorkspaces, 5 * 1000);
        // return () => clearInterval(interval); //Get workspaces every 5 seconds
 

    useEffect(()=>{
        const getActivities = async () =>{
            try{
                if(workspace){
                    const activityResponse = await getDataWithId("http://localhost:5157/GetWorkspaceActivityByWorkspaceId?WorkspaceId", WorkspaceId);
                    //console.log("Te dhenat e aktivitetit ",activityResponse.data)
                    const activityData = activityResponse.data;
                    if (activityData && Array.isArray(activityData) && activityData.length > 0) {
                        setActivities(activityData);
                    } else {
                        setActivities([]);
                        console.log("There is no workspace activity");
                    }
                }
                //Waiting for userIdn
            } catch (error) {
                console.error("There has been an error fetching workspace activities")
                setActivities([]);
            }
        };
        getActivities();
        //console.log("Activity fetched ",activities);
    },[workspace]);
    




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
                if (error.response) {
                    console.log(error.response.data);
                }
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
                //console.log("WORKSPACE OWNER IS: "+workspace.ownerId)
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
                    
                        //largoj closed boards
                    const openBoards = allBoards.filter(board => !board.isClosed);
    
                    const starredResponse = await getDataWithId('http://localhost:5157/backend/starredBoard/GetStarredBoardsByWorkspaceId?workspaceId', WorkspaceId);
                    const starredBoards = starredResponse.data;
    
                    const starredBoardsIds = new Set(starredBoards.map(board => board.boardId));
    
                    //filtrimi per starred bords dhe jo starred
                    const nonStarred = openBoards.filter(board => !starredBoardsIds.has(board.boardId));
                    const starred = openBoards.filter(board => starredBoardsIds.has(board.boardId));
    
                    // sortimi nese klikohet alafabetikisht
                    let sortedNonStarred = nonStarred;
                    if (selectedSort === 'Alphabetically') {
                        sortedNonStarred = sortAlphabetically(nonStarred);
                    }
    
                    setBoards(sortedNonStarred);
                    setStarredBoards(starred);
                }
            } catch (error) {
                if (error.response) {
                    console.error(error.response.data);
                }
                setBoards([]);
                setStarredBoards([]);
            }
        };
    
        getBoards();
    }, [WorkspaceId, userId, workspace, selectedSort, mainContext.userInfo.accessToken]);


    const [memberDetails, setMemberDetails] = useState([]);

        const getMembers = async () => {
            try {   
                if (workspace) {
                    const response = await getDataWithId('/backend/Members/getAllMembersByWorkspace?workspaceId', WorkspaceId);
                    const data = response.data;
                    setMembers(data);

                    const memberDetail = await Promise.all(data.map(async member =>{
                        const responseMemberDetail = await getDataWithId('http://localhost:5157/backend/user/adminUserID?userId', member.userId);                        
                        return responseMemberDetail.data;
                    }))
                    setMemberDetails(memberDetail);
                    //console.log('Members fetched: ',members);
                }                    
            } catch (error) {
                console.error("Error fetching members: ",error.message);

            }
        };

    useEffect(() => {
        getMembers();
    },[WorkspaceId, workspace, mainContext.userInfo.accessToken]);


    const handleRemoveMember = async(memberId, workspaceId) => {
       const removeMemberDto = {
        userId: memberId,
        workspaceId: workspaceId
       }
       
        try {
            const response = await deleteData('http://localhost:5157/backend/Members/RemoveMember',removeMemberDto);
            getMembers();
            
        } catch (error) {
            console.error("Error removing member: ",error.message);
        }
        
    }


    const handleCreateBoard = (newBoard) => {
        setBoards((prevBoards) => [...prevBoards, newBoard]);
    };

    
    const[updateWorkspaceModal, setUpdateWorkspaceModal] = useState(false);
    const [closedBoards, setClosedBoards] = useState([]);
    const handleWorkspaceUpdate = (updatedWorkspace) => {
        setWorkspace((prev) => ({
          ...prev,
          title: updatedWorkspace.Title,
          description: updatedWorkspace.Description,
        }));
      };

      const fetchClosedBoards=async ()=>{
        try{
            const response = await getDataWithId('http://localhost:5157/backend/board/GetClosedBoards?workspaceId', WorkspaceId);
            setClosedBoards(response.data);
        }catch(error){
            console.error("Error fetching closed boards: ",error);
        }
    };



    const handleCloseBoard = async (boardId) => {
        try{
            const closedBoard = {
                boardId: boardId,                
            };
            const response = await postData('http://localhost:5157/backend/board/Close', closedBoard);
            console.log("Board closed ",response.data);
           
            
            setBoards((prevBoards) => prevBoards.filter((b)=> b.boardId !== boardId));
            setStarredBoards((prevStarredBoards) => prevStarredBoards.filter((b) => b.boardId !== boardId));
            // const closedBoardData = response.data;
            // setClosedBoards((prevClosedBoards)=> [...prevClosedBoards, closedBoardData]);
        }
        catch(error){
            if (error.response) {
                console.error("Error closing board:",error.response?.data || error.message);
            }
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

    const handleStarButtonClick = (event, board) => {
        event.stopPropagation();  //se len mu hap bordi kur te behet star
        handleStarBoard(board);
    };
    const getBackgroundImageUrl = async (board) => {
        if (!board.backgroundId) {
            console.error("Board does not have a valid backgroundId");
            return myImage; // // Return a default image if backgroundId is not valid
        }
    
        try {
            const response = await getDataWithId('http://localhost:5157/backend/background/GetBackgroundByID?id', board.backgroundId);
    
            // Check if response contains image data
            if (response.data && response.data.imageData) {
                // Convert ImageData from byte array to Base64 format
            const background =response.data;
            const url = `data:image/jpeg;base64,${background.imageDataBase64}`;
            return url;
            }
        } catch (error) {
            console.error("Error fetching background image:", error);
            return myImage;  // Return a default image in case of error
        }
    };
    
      const [backgroundUrls, setBackgroundUrls] = useState({});

        useEffect(() => {
            const getBackgrounds = async () => {
                const urls = {};
                for (const board of [...boards, ...starredBoards]) {
                    const url = await getBackgroundImageUrl(board);
                    urls[board.boardId] = url;  // Store the URL for each board
                }
                setBackgroundUrls(urls);
            };
    
            getBackgrounds();
        }, [boards, starredBoards]);

        
    const [activeBackgrounds, setActiveBackgrounds] = useState([]);
    const [activeBackgroundUrls, setActiveBackgroundUrls] = useState({});
  
    const getActiveBackgrounds = async () => {
        try {
            const backgroundsResponse = await getData('http://localhost:5157/backend/background/GetActiveBackgrounds');
            const backgroundsData = backgroundsResponse.data;

            if (backgroundsData && Array.isArray(backgroundsData) && backgroundsData.length > 0) {
                setActiveBackgrounds(backgroundsData);

                // Create URLs for background images using backgroundId
                const urls = {};
                for (let background of backgroundsData) {
                    const url = `data:image/jpeg;base64,${background.imageDataBase64}`;
                    urls[background.backgroundId] = url; // Use backgroundId instead of id
                }
                setActiveBackgroundUrls(urls);
            } else {
                console.error("No active backgrounds found.");
            }
        } catch (error) {
            console.error("Error fetching backgrounds:", error.message);
        }
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


    // useEffect(()=>{
    //     if (WorkspaceId) {
    //         getTasks();
    //         console.log("Workspace id ",WorkspaceId);
    //         console.log("Tasks fetched: ",tasks);
    //     }
    // }, [WorkspaceId]);
    
        const getInitials = (firstName, lastName) => {
            if (!firstName || !lastName) {
                return '';
            }
            return `${firstName.charAt(0).toUpperCase()}${lastName.charAt(0).toUpperCase()}`;
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
                let data = response.data;
                //console.log("Sent Invites fetched: ", data);
        
                // filtrimi i ftesave pending
                let pendingInvites = data.filter(invite => invite.inviteStatus === "Pending");
                //console.log("Pending invites: ", pendingInvites);
        
                // sortimi i ftesave ne baze te dates (recent lart)
                pendingInvites = pendingInvites.sort((a, b) => new Date(b.dateSent) - new Date(a.dateSent));
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
            if (WorkspaceId) {
                getSentInvites();
            }
            
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

        useEffect(()=>{
        
            const getBoard = async () =>{
                try {
                    if(boardId){
                        const boardResponse = await getDataWithId('/backend/board/GetBoardByID?id',boardId);
                        const boardData = boardResponse.data;
                        setBoard(boardData);
                    }
                    
                } catch (error) {
                    if (error.response) {
                        console.log(error.response.data);
                    }
        
                    navigate('/main/workspaces');
                }
            };
            getBoard();
        },[boardId, userId, mainContext.userInfo.accessToken]);

        const getChecklistsByTask = async () => {
            try {
              if (taskId) {
                const response = await getDataWithId(
                  'http://localhost:5157/backend/checklist/GetChecklistByTaskId?taskId',
                  taskId
                );
                const data = response.data;
                console.log('Fetched checklists:', data); // Log checklists data for debugging
                setChecklists(data);
          
                // Fetch checklist items after setting checklists
                if (Array.isArray(data)) {
                  fetchChecklistItems(data);
                } else {
                  console.error("Invalid checklists data received.");
                }
              }
            } catch (error) {
              console.error("Error fetching checklists: ", error.message);
            }
          };
          
          useEffect(() => {
            getChecklistsByTask();
          }, [WorkspaceId, workspaces, mainContext.userInfo.accessToken, taskId]);
          
          const [checklistItems, setChecklistItems] = useState([]);
          
          const fetchChecklistItems = async (checklists) => {
            const items = {};
          
            if (!Array.isArray(checklists)) {
              console.error("Invalid checklists:", checklists);
              return;
            }
          
            for (const checklist of checklists) {
              if (!checklist.checklistId) {
                console.error("Invalid checklistId for checklist:", checklist);
                continue;
              }
          
              try {
                const response = await getDataWithId(
                  'http://localhost:5157/backend/checklistItems/GetChecklistItemByChecklistId?checklistId',
                  checklist.checklistId
                );
                items[checklist.checklistId] = response.data; // Store items by checklist ID
              } catch (error) {
                console.error(`Error fetching items for checklist ${checklist.checklistId}: `, error.message);
              }
            }
          
            setChecklistItems(items);
          };
          

            



        const handleCreateList = (newList) =>{
            setLists((prevLists) => [...prevLists, newList]);
        };

        useEffect(() =>{
            const getList = async () => {
                try {
                    if(listId){
                        const listResponse = await getDataWithId('http://localhost:5157/backend/list/GetListById',listId);
                        const listData = listResponse.data;
                        setList(listData);
                    }
                } catch (error) {
                    if (error.response) {
                        console.log(error.response.data);
                    }   
                    navigate('/main/workspaces');
                    
                }
            };
            getList();
        },[listId, userId ,mainContext.userInfo.accessToken]);


        const countClosedBoards = closedBoards.length;
        const ALLBoardsCount = boardCount+countClosedBoards;
  
        const [selectedBoardId, setSelectedBoardId] = useState(null);

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
            getInitialsFromFullName,
            memberDetails,
            setMemberDetails,
            handleRemoveMember,
            checklists,
            checklistItems,
            setChecklistItems,
            setChecklists,
            board,
            setBoard,
            lists,
            setLists,
            handleCreateList,
            listId,
            list,
            activities,
            isLoading,
            setIsLoading,
            getWorkspaces,
            setList,
            backgroundUrls,
            activeBackgrounds,
            activeBackgroundUrls,
            getActiveBackgrounds,
            fetchClosedBoards,
            closedBoards,
            setClosedBoards, 
            countClosedBoards,
            ALLBoardsCount,
            fetchChecklistItems,
            recentBoards,
            setRecentBoards,
            getChecklistsByTask,
            handleStarButtonClick,
            selectedBoardId,
            setSelectedBoardId
        }}>
            {children}
        </WorkspaceContext.Provider>
    );
};
