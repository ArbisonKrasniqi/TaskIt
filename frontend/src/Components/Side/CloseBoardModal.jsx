import React, {useContext, useState} from 'react';
import { WorkspaceContext } from './WorkspaceContext';


const CloseBoardModal = ({open, boardTitle ,onClose, role, boardId, onBoardClosed}) =>{
  


    const [modalContent, setModalContent] =useState(" ");
    const [clicked, setClicked] = useState(false);
    const {workspace}= useContext(WorkspaceContext);

    
        const handleClickH2 = () =>{
            setClicked(true);
                   setModalContent( 
                    <div>
                   <span className='text-gray-900'>
                    You can find and reopen closed boards at the bottom of 
                    <a href={`/main/boards/${workspace.workspaceId}`} className="text-blue-500 underline ml-1">Your Boards Page</a>.
                </span>
                <button className='text-gray-900 bg-red-700 w-48 m-2 rounded' onClick={handleClose}>Close Board</button>
                    </div>
            );
        };
        
        const handleClose = () => {
            onBoardClosed(boardId);
            setClicked(false);
            onClose();
        };


        if(!open) return null; //kontrollojm a me e shfaq modalin

    return(
        <div className="fixed inset-0 z-50 flex items-center justify-start p-4 ml-60 "  onClick={(e) => {
            if (e.target.className.includes('modal-overlay')) {
                e.stopPropagation();//e parandalon mbylljen e modalit kur klikohet mbrapa ne sfond
            }
        }}>
            <div className="bg-white border border-gray-300 rounded-lg shadow-lg w-60">
            <div className="p-2 flex justify-between items-center">
            <h3 className="text-gray-700 font-semibold">{boardTitle}</h3>
            <button onClick={handleClose} className="text-gray-500 hover:text-gray-700">
            X
          </button>
                </div>
                {clicked ? (
                    <div className="p-2 m-1">
                        {modalContent}
                    </div>
                ) : ( // i ndajna qe me mujt mu perdor funksioni handle close tek butoni close(leave)
                    <h2
                        className="p-2 m-1 cursor-pointer text-gray-700 hover:bg-gray-300"
                        onClick={handleClickH2}
                    >
                        {'close board'}
                    </h2>
                )}
            </div>
        </div>



    );
}
export default CloseBoardModal