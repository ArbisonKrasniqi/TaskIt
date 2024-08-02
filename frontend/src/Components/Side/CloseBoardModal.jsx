import React, {useEffect, useState} from 'react';


const CloseBoardModal = ({open, boardTitle ,onClose, role}) =>{
  

    const [action, setAction] = useState("Leave"); 
    const [modalContent, setModalContent] =useState(" ");
    const [clicked, setClicked] = useState(false);

        useEffect(()=>{
            if(role=="Owner"){
                setAction("Close");
            }
            else{
                setAction("Leave");
            }
        }, [role]);

    
        const handleClickH2 = (prop1) =>{
            setClicked(true);
                if(prop1==="Close"){
                   setModalContent( 
                    <div>
                   <span className='text-gray-900'>
                    You can find and reopen closed boards at the bottom of 
                    <a href="/boards" className="text-blue-500 underline ml-1">Your Boards Page</a>.
                </span>
                <button className='text-gray-900 bg-red-700 w-48 m-2 rounded' onClick={handleClose}>Close Board</button>
                    </div>
            );
            
                } else {
                    setModalContent(
                    <span className='text-gray-900'>
                        Are you sure you want to leave this board?
                        <button className='text-gray-900 bg-red-700 w-48 m-2 rounded' onClick={handleClose}>Leave Board</button>
                    </span>
                    );
                }
        };
        const handleClose = () => {
            setClicked(false);
            onClose();
        };

        if(!open) return null; //kontrollojm a me e shfaq modalin
    return(
        <div className="fixed inset-0 flex items-center justify-start p-4 ml-60 zIndex: 1000">
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
                        onClick={() => handleClickH2(action)}
                    >
                        {`${action} board`}
                    </h2>
                )}
            </div>
        </div>



    );
}
export default CloseBoardModal