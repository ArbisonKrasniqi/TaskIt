import React, { useState } from 'react';
import MembersList from '../Members/MembersList';
import InvitesList from '../Invites/InvitesList';
import BoardsList from '../Boards/BoardsList';
import { useParams } from 'react-router-dom';

const WorkspaceTable = () => {
    const [activeList, setActiveList] = useState('boards'); // Default to 'boards'

    const { workspaceId } = useParams();

    return (
        <div className="flex flex-col h-full">
            <div className="flex mb-4">
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'boards' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('boards')}>Boards</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'members' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('members')}>Members</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'invites' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('invites')}>Invites</button>
            </div>

            {/* Render the selected list */}
            <div className="flex-grow overflow-auto">
                {activeList === 'boards' && <BoardsList />}
                {activeList === 'members' && <MembersList />}
                {activeList === 'invites' && <InvitesList />}
            </div>
        </div>
    );
};

export default WorkspaceTable;
