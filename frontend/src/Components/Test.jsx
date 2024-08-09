import { useParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getDataWithId } from '../Services/FetchService';

const Test = () => {
    const navigate = useNavigate();
    const { userId, workspaceId, boardId } = useParams();

    const [workspace, setWorkspace] = useState(null);
    const [workspaces, setWorkspaces] = useState([]);
    const [board, setBoard] = useState(null);
    const [boards, setBoards] = useState([]);

    const fetchWorkspaces = async (id) => {
        try {
            const workspacesData = await getDataWithId('/backend/workspace/GetWorkspacesByOwnerId?OwnerId', id);
            const data = workspacesData.data;
            setWorkspaces(data);
        } catch (error) {
            console.error('Error fetching workspaces:', error);
        }
    };

    const fetchBoards = async (id) => {
        try {
            const boardsData = await getDataWithId('/backend/board/GetBoardsByWorkspaceId?workspaceId', id);
            const data = boardsData.data;
            setBoards(data);
        } catch (error) {
            console.error('Error fetching boards:', error);
        }
    };

    const handleWorkspaceClick = (newWorkspaceId) => {
        navigate(`/${userId}/${newWorkspaceId}`);
      };

    const handleBoardClick = (newBoardId) => {
        navigate(`/${userId}/${workspaceId}/${newBoardId}`);
    }

    const formatDate = (dateString) => {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(dateString).toLocaleDateString(undefined, options);
      };

    useEffect(() => {
        if (userId) {
            fetchWorkspaces(userId);
        }
    }, [userId]);

    useEffect(() => {
        if (workspaceId) {
            fetchBoards(workspaceId);
        }
    }, [workspaceId]);

    useEffect(() => {
        if (workspaceId && workspaces.length > 0) {
            const filteredWorkspace = workspaces.find(ws => ws.workspaceId == workspaceId);
            console.log("testing filter: "+filteredWorkspace);
            if (filteredWorkspace) {
                setWorkspace(filteredWorkspace);
            }
        }
    }, [workspaceId, workspaces]);

    useEffect(() => {
        if (boardId && boards.length > 0) {
            const filteredBoard = boards.find(b => b.boardId == boardId);
            if (filteredBoard) {
                setBoard(filteredBoard);
            }
        }
    }, [boardId, boards]);

    if (!userId) {
        return <div>No user ID provided</div>;
    }

    if (!workspaceId && !boardId) {
        return (
            // <div>
            //     {console.log('Workspaces:', workspaces)}
            //     Workspaces printed?
            // </div>



                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4">
            {workspaces.map(workspace => (
                <div
                key={workspace.workspaceId}
                className="p-4 border rounded-lg shadow-lg cursor-pointer hover:bg-gray-100"
                onClick={() => handleWorkspaceClick(workspace.workspaceId)}
                >
                <h3 className="text-lg font-bold">{workspace.title}</h3>
                <p className="text-sm text-gray-600">{workspace.description}</p>
                </div>
            ))}
            </div>
        );
    }

    if (workspaceId && !boardId) {
        if (!workspace) {
            return <div>Loading workspace...</div>;
        }

        return (
            // <div>
            //     {console.log('Boards:', boards)}
            //     Workspace is set to {workspace.title}
            // </div>

            <div className="p-4">
      {workspaces.map(workspace => (
        <div key={workspace.workspaceId} className="mb-6">
          <div
            className="p-4 border rounded-lg shadow-lg cursor-pointer hover:bg-gray-100"
            onClick={() => handleWorkspaceClick(workspace.workspaceId)}
          >
            <h3 className="text-lg font-bold">{workspace.title}</h3>
            <p className="text-sm text-gray-600">{workspace.description}</p>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-4">
            {workspace.boards.map(board => (
              <div
                key={board.boardId}
                className="p-4 border rounded-lg shadow-lg cursor-pointer hover:bg-gray-100"
                onClick={() => handleBoardClick(board.boardId)}
              >
                <h4 className="text-md font-semibold">{board.title}</h4>
                <p className="text-sm text-gray-600">Board ID: {board.boardId}</p>
                <p className="text-sm text-gray-600">Created: {formatDate(board.dateCreated)}</p>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
        );
    }

    if (workspaceId && boardId) {
        if (!workspace || !board) {
            return <div>Loading workspace or board...</div>;
        }

        return (
            <div>
                {console.log('Board:', board)}
                Workspace is set to {workspace.title};
                Board is set to {board.title}
            </div>
        );
    }

    return <div>Unexpected state</div>;
};

export default Test;
