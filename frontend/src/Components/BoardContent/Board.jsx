import React, { useState, useContext } from "react";
import List from "../List/List";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { MdOutlineStarOutline, MdOutlineStarPurple500 } from "react-icons/md";
import MemberProfilePic from "../ProfilePic/MemberProfilepic";
import { DropdownContext } from "../Navbar/Navbar";

const Board = () => {
    const { board, handleStarBoard } = useContext(WorkspaceContext);
    const [ProfilePicIsOpen, setProfilePicDropdownIsOpen] = useState(false);

    if (!board) {
        return <div>Loading...</div>;
    }

    const isStarred = board.isStarred;

    const toggleDropdownProfilePic = () => {
        setProfilePicDropdownIsOpen(prev => !prev);
    };

    const values = {
        ProfilePicIsOpen,
        toggleDropdownProfilePic
    };

    return (
        <div>
            <header className="flex items-center justify-between w-full p-4 bg-white bg-opacity-30 text-white shadow-lg">
                <div className="flex items-center">
                    <h2 className="text-xl font-semibold text-slate-900 mr-4">{board.title}</h2>
                    <button
                        className="text-slate-900 text-2xl focus:outline-none"
                        onClick={() => handleStarBoard(board)}
                        aria-label={isStarred ? "Unstar board" : "Star board"}
                    >
                        {isStarred ? <MdOutlineStarPurple500 /> : <MdOutlineStarOutline />}
                    </button>
                </div>

                <div className="flex items-center space-x-4">
                    <DropdownContext.Provider value={values}>
                        <MemberProfilePic />
                    </DropdownContext.Provider>
                </div>
            </header>

            <div>
                <List />
            </div>
        </div>
    );
};

export default Board;


