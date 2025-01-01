import React, { useContext } from 'react';
import { MdOutlineStarOutline, MdOutlineStarPurple500 } from 'react-icons/md';
import { WorkspaceContext } from '../Side/WorkspaceContext';

const StarButton = ({ board }) => {
    const { starredBoards, handleStarBoard } = useContext(WorkspaceContext);

    const isStarred = starredBoards.some(b => b.boardId === board.boardId);

    return (
        <button
            className="text-slate-200 text-2xl focus:outline-none"
            onClick={() => handleStarBoard(board)}
        >
            {isStarred ? <MdOutlineStarPurple500 /> : <MdOutlineStarOutline />}
        </button>
    );
}

export default StarButton;
