import React, { useState } from 'react';
import StarredBoardsList from '../StarredBoards/StarredBoardsList';
import WorkspacesList from '../Workspaces/WorkspacesList';

const UserTable = () => {
    const [activeList, setActiveList] = useState('workspaces');

    return (
        <div className="flex flex-col h-full">
            <div className="flex mb-4">
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'workspaces' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('workspaces')}>Workspaces</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'starredBoards' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('starredBoards')}>Starred Boards</button>
            </div>

            {/* Render the selected list */}
            <div className="flex-grow overflow-auto">
                {activeList === 'workspaces' && <WorkspacesList />}
                {activeList === 'starredBoards' && <StarredBoardsList />}
            </div>
        </div>
    );
};

export default UserTable;
