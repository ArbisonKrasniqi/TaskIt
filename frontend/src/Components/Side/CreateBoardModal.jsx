import React, { useState } from 'react';
import { postData } from './../../Services/FetchService';

const CreateBoardModal = ({ open, onClose, onBoardCreated, children }) => {
    const [boardTitle, setBoardTitle] = useState('');
    const [backgroundId, setBackgroundId] = useState(null);
    const [workspaceId, setWorkspaceId] = useState(1); // Assuming workspaceId is known/fixed for now
    const [clicked, setClicked] = useState(false);

    const handleTitleChange = (e) => {
        setBoardTitle(e.target.value);
    };

    const handleBackgroundClick = (id) => {
        setBackgroundId(id);
        setClicked(!clicked);
    };

    const handleCreateBoard = async () => {
        const newBoard = {
            title: boardTitle,
            backgroundId: backgroundId,
            workspaceId: workspaceId,
        };
        console.log('Creating board with data:', newBoard);
        try {
            const response = await postData('http://localhost:5157/backend/board/CreateBoard', newBoard);
            console.log('Board created successfully:', response.data);
            onBoardCreated(response.data);
            onClose(); // Close the modal after creating the board
        } catch (error) {
            console.error('Failed to create board', error);
            console.log('Error response data: ', error.response.data);
        }
    };

    return (
        <div
            className={`
        fixed inset-0 flex justify-center items-center transition-colors 
        ${open ? "visible bg-black/20" : "invisible"}
        `}>
            <div
                className={`bg-white rounded-xl shadow p-6 transition-all w-80 text-center
            ${open ? "scale-100 opacity-100" : "scale-125 opacity-0"}`}>
                <button
                    onClick={onClose}
                    className="absolute top-1 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-50 hover:text-gray-600">
                    X
                </button>
                <p className="w-full origin-left font-sans text-gray-400 font-bold text-l">Create Board</p>
                <hr className="w-full border-gray-400 mt-3"></hr>
                <br></br>
                <p className="w-full origin-left font-sans text-gray-400 font-semibold text-l">Background</p>
                <br></br>
                <div className="flex flex-wrap justify-between gap-2">
                    {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((id) => (
                        <button
                            key={id}
                            onClick={() => handleBackgroundClick(id)}
                            className={`w-10 h-10 rounded-lg px-3 ${
                                backgroundId === id  && clicked===true? 'border-8 border-grey-500' : 'border-2 border-transparent'
                            } ${
                                id === 1
                                    ? 'bg-gradient-to-r from-blue-400 to-indigo-500'
                                    : id === 2
                                    ? 'bg-gradient-to-r from-red-400 to-red-500'
                                    : id === 3
                                    ? 'bg-gradient-to-r from-pink-400 to-pink-500'
                                    : id === 4
                                    ? 'bg-gradient-to-r from-purple-400 to-purple-500'
                                    : id === 5
                                    ? 'bg-gradient-to-r from-green-400 to-green-500'
                                    : id === 6
                                    ? 'bg-gradient-to-r from-yellow-400 to-yellow-500'
                                    : id === 7
                                    ? 'bg-gradient-to-r from-teal-400 to-teal-500'
                                    : id === 8
                                    ? 'bg-gradient-to-r from-gray-400 to-gray-500'
                                    : id === 9
                                    ? 'bg-gradient-to-r from-cyan-400 to-cyan-500'
                                    : 'bg-gradient-to-r from-lime-400 to-lime-500'
                            }`}></button>
                    ))}
                </div>
                <br></br>
                <p className="w-full origin-left font-sans text-gray-400 font-semibold text-l">Board Title</p>
                <br></br>
                <input
                    type="text"
                    name="boardTitle"
                    id="boardTitle"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                    value={boardTitle}
                    onChange={handleTitleChange}
                />
                <br /><br />
                <button
                    className="bg-gray-800 font-bold text-white px-4 py-2 rounded-md w-[60%] hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out"
                    onClick={handleCreateBoard}
                >
                    Create
                </button>

                {children}
            </div>
        </div>
    );
}

export default CreateBoardModal;
