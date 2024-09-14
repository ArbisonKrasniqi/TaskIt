import React, { useState, useEffect, useContext } from 'react'; 
import { postData, getData } from './../../Services/FetchService';
import { MainContext } from '../../Pages/MainContext';
import { WorkspaceContext } from './WorkspaceContext';
import { useNavigate } from 'react-router-dom';
import CloseBoardModal from './CloseBoardModal';
const CreateBoardModal = ({ open, onClose, onBoardCreated, children }) => {
    const mainContext = useContext(MainContext);
    const [boardTitle, setBoardTitle] = useState('');
    const [backgroundId, setBackgroundId] = useState();
    const [workspaceId, setWorkspaceId] = useState(mainContext.workspaceId);
    const [clicked, setClicked] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [chooseBackgroundError, setChooseBackgroundError] = useState('');
    const navigate = useNavigate();
    const {activeBackgrounds,activeBackgroundUrls, getActiveBackgrounds} = useContext(WorkspaceContext)
   
    useEffect(() => {
       
        getActiveBackgrounds();
    }, [clicked]);
    
// console.log("Bakkkgraunsddd ",backgrounds);
// console.log("Bakkkgraunsddd urlddsss",backgroundUrls);

    const handleTitleChange = (e) => {
        setBoardTitle(e.target.value);
        console.log("lengthh: ",boardTitle.length);
        
        if (boardTitle.length > 0 && boardTitle.length<21) {
            setErrorMessage('');
        }
    };

    const handleBackgroundClick = (id) => {
        setBackgroundId(id);
        setClicked(!clicked);
        setChooseBackgroundError('');
    };

    const handleCreateBoard = async () => {
        var isError = false;


        if (!backgroundId) {
            setChooseBackgroundError('You must choose a background.');
        } else {
            setChooseBackgroundError('');
        }

        if (boardTitle.length < 2 || boardTitle.length > 20) {
            setErrorMessage('Board title must be between 2 and 20 characters.');
            isError = true;
        } else {
            setErrorMessage('');
        }

    
        if (isError) return;

    
        const newBoard = {
            title: boardTitle,
            backgroundId: backgroundId,
            workspaceId: workspaceId
        };
        try {
            const response = await postData('http://localhost:5157/backend/board/CreateBoard', newBoard);
            console.log('Full response:', response); 
        
                console.log('Board created successfully:', response.data);
                onClose();
                onBoardCreated(response.data);
                setBoardTitle('');  
                setBackgroundId(null);  
                setErrorMessage('');
                navigate(`/main/board/${workspaceId}/${response.data.boardId}`);
         
        } catch (error) {
            console.log('Error creating board');
            if(error.response){
                console.error("response status ",error.response.status);
                console.error("response data ",error.response.data);
                console.error("response header ",error.response.headers);
            } 
           
        }
    
    }


    if(!open) return null;

    return (
        <div
        className="fixed inset-0 z-50 felx items-center justify-center bg-black bg-opacity-0"
        onClick={(e) =>{
            if (e.target.className.includes('bg-black')) {
                e.stopPropagation();
                onClose();
                
            }
        }}
        >

        <div className={`fixed z-30 inset-0 flex justify-center items-center transition-colors ${open ? "visible bg-black/20" : "invisible"}`}>
            <div className={`bg-white rounded-xl shadow p-6 transition-all w-80 text-center ${open ? "scale-100 opacity-100" : "scale-125 opacity-0"}`}>
                <button onClick={() => {onClose(); setBoardTitle(''); setBackgroundId(null); setChooseBackgroundError(''); setErrorMessage('');} } className="absolute top-1 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-50 hover:text-gray-600">X</button>
                <p className="w-full font-sans text-gray-500 font-bold text-l">Create Board</p>
                <hr className="w-full border-gray-400 mt-3"></hr>

                <p className="w-full font-sans text-gray-500 font-semibold text-l">Background</p>
                <div className="flex flex-wrap justify-center gap-2 mt-3">
                {activeBackgrounds.map((background, index) => (
    <button
        key={`${background.backgroundId}-${index}`} // Ensure a unique key by combining backgroundId with the index
        onClick={() => handleBackgroundClick(background.backgroundId)}
        className={`relative w-16 h-16 rounded-lg bg-cover bg-center ${backgroundId === background.backgroundId ? 'border-4 border-blue-500' : 'border-2 border-transparent'}`}
        style={{ backgroundImage: `url(${activeBackgroundUrls[background.backgroundId] || 'background.jpg'})` }} // Use backgroundId here as well
    />
                ))}

                </div>
                {chooseBackgroundError && <p className='text-red-500 text-sm'>{chooseBackgroundError}</p>}

                <p className="w-full font-sans text-gray-500 font-semibold text-l mt-5">Board Title</p>
                <input
                    type="text"
                    name="boardTitle"
                    id="boardTitle"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full mt-2"
                    value={boardTitle}
                    onChange={handleTitleChange}
                />
                {errorMessage &&  <p className="text-red-500 text-sm">{errorMessage}</p>}

                <button className="bg-gray-800 font-bold text-white px-4 py-2 rounded-md w-[60%] mt-5 hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out" onClick={handleCreateBoard}>
                    Create
                </button>

                {children}
            </div>
        </div>
        </div>
    );
}
export default CreateBoardModal;