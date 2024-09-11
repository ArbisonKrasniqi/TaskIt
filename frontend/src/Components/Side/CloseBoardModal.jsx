import React, { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { WorkspaceContext } from './WorkspaceContext';

const CloseBoardModal = ({ open, boardTitle, onClose, role, boardId, onBoardClosed }) => {
    const { WorkspaceId } = useContext(WorkspaceContext);
    const [modalContent, setModalContent] = useState(" ");
    const [clicked, setClicked] = useState(false);
    const navigate = useNavigate();

    const handleClickH2 = () => {
        setClicked(true);
        setModalContent(
            <div>
                <span className='text-gray-900'>
                    You can find and reopen closed boards at the bottom of
                    <a href="/boards" className="text-blue-500 underline ml-1">Your Boards Page</a>.
                </span>
                <button className='text-gray-900 bg-red-700 w-full mt-2 rounded p-2' onClick={handleClose}>Close Board</button>
            </div>
        );
    };

    const handleClose = () => {
        onBoardClosed(boardId);
        setClicked(false);
        onClose();
    };

    const closeModal = () => {
        setClicked(false);
        onClose();
    };

    const handleNav = () => {
        navigate(`/main/boardSettings/${WorkspaceId}/${boardId}`);
        onClose();
    };

    if (!open) return null;

    return (
        <div
        className="absolute top-20 z-50 flex flex-col bg-white border border-gray-300 rounded-lg shadow-lg w-80 max-w-lg mx-4 sm:mx-auto"
            onClick={(e) => {
                if (e.target.className.includes('bg-black')) {
                    e.stopPropagation();
                    closeModal();
                }
            }}
        >
            <div
                className="bg-white border border-gray-300 rounded-lg shadow-lg w-80 max-w-lg mx-4 sm:mx-auto" // Center and limit width
                onClick={(e) => e.stopPropagation()}
            >
                <div className="p-4 flex justify-between items-center border-b border-gray-300">
                    <h3 className="text-gray-700 font-semibold">{boardTitle}</h3>
                    <button onClick={closeModal} className="text-gray-500 hover:text-gray-700">
                        X
                    </button>
                </div>
                <div className="p-4">
                    {clicked ? (
                        <div>
                            {modalContent}
                        </div>
                    ) : (
                        <div>
                            {role === 'Owner' && (
                                <h2
                                    className="p-2 cursor-pointer text-gray-700 hover:bg-gray-300 rounded"
                                    onClick={handleClickH2}
                                >
                                    {'Close Board'}
                                </h2>
                            )}
                            <h2
                                className="p-2 cursor-pointer text-gray-700 hover:bg-gray-300 rounded"
                                onClick={handleNav}
                            >
                                {'Board Settings'}
                            </h2>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default CloseBoardModal;
