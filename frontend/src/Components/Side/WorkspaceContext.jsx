import React, {createContext, useState, useEffect} from 'react';
import { getDataWithId, deleteData, postData } from '../../Services/FetchService';
import myImage from './background.jpg';
export const WorkspaceContext = createContext();

export const WorkspaceProvider = ({ children }) => {
    const USERID = "asdajsdanlkdjad";
    const WorkspaceId = 1;
    const[open, setOpen] = useState(true);
    const [workspace, setWorkspace] = useState(null);
    const [workspaces, setWorkspaces] = useState(null);
    const [boards, setBoards] = useState([]);
    const [selectedSort, setSelectedSort] = useState('Alphabetically');
    const [openModal, setOpenModal] = useState(false);
    const [hoveredIndex, setHoveredIndex] = useState(null);
    const [hoveredBoardIndex, setHoveredBoardIndex] =useState(null);
    const [hover, setHover] = useState(false);
    const [openSortModal, setOpenSortModal] = useState(false);
    const [selectedBoardTitle, setSelectedBoardTitle] = useState("");
    const [openCloseModal, setOpenCloseModal] = useState(false);
    const[roli, setRoli] = useState("Owner");
    const [members, setMembers] = useState([]);

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

    
const workspaceTitle = workspace ? workspace.title : 'Workspace';
console.log(workspaceTitle);
    useEffect(() => {
        const getBoards = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId?workspaceId', WorkspaceId);
                const data = response.data;
                console.log('Fetched data: ', data);
                if (data && Array.isArray(data) && data.length > 0) {
                    let sortType = localStorage.getItem('selectedSort') || 'Alphabetically';
                    let sortedBoards = [];
                    if (sortType === 'Alphabetically') {
                        sortedBoards = sortAlphabetically(data);
                    } else {
                        sortedBoards = data; // Sepse i merr te sortume by recent nga databaza
                    }
                    const starredBoards = JSON.parse(localStorage.getItem('starredBoards')) || [];
                    sortedBoards.forEach(board => {
                        board.starred = starredBoards.includes(board.boardId);
                    });
                    sortedBoards = moveStarredBoardsToTop(sortedBoards);
                    setBoards(sortedBoards);
                    setSelectedSort(sortType);
                    
                } else {
                    console.error('Data is null, not an array, or empty:', data);
                    setBoards([]); // Trajtohen si të dhëna të zbrazëta
                }
            } catch (error) {
                console.error(error.message);
                setBoards([]); // Në rast që ndodh ndonjë gabim
            }
        };
        getBoards();
        console.log("Boards fetched:", boards);
    },[WorkspaceId]);


    useEffect(() => {
        const getMembers = async () => {
            try {
                const response = await getDataWithId('http://localhost:5157/backend/Members/getAllMembers?workspaceId', WorkspaceId);
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
    },[]);

    const handleCreateBoard = (newBoard) => {
        setBoards((prevBoards) => [...prevBoards, newBoard]);
    };

    const handleCreateWorkspace = (newWorkspace) => {
      setWorkspaces((prevWorkspaces) => [...prevWorkspaces, newWorkspace]);
  }

    const moveStarredBoardsToTop = (boards) => {
        return boards.sort((a, b) => b.starred - a.starred);
    };

    const sortAlphabetically = (boards) => {
        return boards.slice().sort((a, b) => a.title.localeCompare(b.title));
    };

    const sortByRecent = async () => {
        const dataResponse = await getDataWithId('http://localhost:5157/backend/board/GetBoardsByWorkspaceId?workspaceId', WorkspaceId);
        return dataResponse.data;
    };

    const handleSortChange = async (sortType) => {
        setSelectedSort(sortType);
        localStorage.setItem('selectedSort', sortType);
        let sortedBoards = [];
        if (sortType === 'Alphabetically') {
            sortedBoards = sortAlphabetically(boards);
        } else {
            sortedBoards = await sortByRecent();
        }
        sortedBoards = moveStarredBoardsToTop(sortedBoards);
        setBoards(sortedBoards);
    };

    const handleStarBoard = async (board) => {
        const isStarred = board.starred;
        const data = {
            BoardId: board.boardId,
            UserId: USERID,
        };
        try {
            if (isStarred) { // Pra starred=true, atëherë bëje unstar
                const dataUnstar = await deleteData('http://localhost:5157/backend/starredBoard/UnstarBoard', data);
                console.log(dataUnstar.data);
                // REMOVE FROM LOCALSTORAGE
                let starredBoards = JSON.parse(localStorage.getItem('starredBoards')) || [];
                starredBoards = starredBoards.filter(id => id !== board.boardId);
                localStorage.setItem('starredBoards', JSON.stringify(starredBoards));
            } else { // DMTH starred=false, atëherë bëje star
                const dataResponse = await postData('http://localhost:5157/backend/starredBoard/StarBoard', data);
                console.log(dataResponse.data);
                // ADD TO LOCAL STORAGE
                let starredBoards = JSON.parse(localStorage.getItem('starredBoards')) || [];
                starredBoards.push(board.boardId);
                localStorage.setItem('starredBoards', JSON.stringify(starredBoards));
            }
    
            setBoards(prevBoards => {
                const updatedBoards = prevBoards.map(b =>
                    b.boardId === board.boardId ? { ...b, starred: !isStarred } : b
                );
                console.log('Updated Boards:', updatedBoards);
                return moveStarredBoardsToTop(updatedBoards);
            });
        } catch (error) {
            console.error("Error starring/unstarring the board:", error.response ? error.response.data : error.message);
        }
    };
    
    const getBackgroundImageUrl = (board) => {
        // const background = backgrounds.find(b=>b.backgroundId === board.backgroundId);
        // return background? background.imageUrl : '';
        return myImage;
    };

    return (
        <WorkspaceContext.Provider value={{
            USERID,
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
            handleCreateBoard,
            setHoveredIndex,
            hoveredIndex,
            setSelectedBoardTitle,
            selectedBoardTitle,
            roli,
            hoveredBoardIndex,
            setHoveredBoardIndex,
            moveStarredBoardsToTop,
            sortAlphabetically,
            sortByRecent,
            handleSortChange,
            handleStarBoard,
            getBackgroundImageUrl,
            handleCreateWorkspace,
            members,
            starredBoards: boards.filter(board => board.starred),
        }}>
            {children}
        </WorkspaceContext.Provider>
    );
};
