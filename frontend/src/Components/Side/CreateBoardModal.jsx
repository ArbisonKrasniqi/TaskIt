import React, { useState, useEffect, useContext } from 'react'; 
import { postData, getData } from './../../Services/FetchService';
import { MainContext } from '../../Pages/MainContext';

const CreateBoardModal = ({ open, onClose, onBoardCreated, children }) => {
    const mainContext = useContext(MainContext);
    const [boardTitle, setBoardTitle] = useState('');
    const [backgroundId, setBackgroundId] = useState(2);
    const [workspaceId, setWorkspaceId] = useState(mainContext.workspaceId);
    const [clicked, setClicked] = useState(false);
    const [backgrounds, setBackgrounds] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');


     useEffect(()=>{
     const getBackgrounds = async () =>{
         try{
             const backgroundsResponse = await getData('http://localhost:5157/backend/background/GetAllBackgrounds');
             const backgroundsData = backgroundsResponse.data;
            console.log("Background fetched:",backgroundsData);

             if (backgroundsData && Array.isArray(backgroundsData) && backgroundsData.length > 0) {
                 setBackgrounds(backgroundsData);
            }else {
                console.error('Data is null, not an array, or empty:', backgroundsData);
                 setBackgrounds([]); //trajtohen si te dhena te zbrazeta
             }
         }catch (error) {
             console.error(error.message);
         }
     };
     getBackgrounds();
 }, []);




    const handleTitleChange = (e) => {
        setBoardTitle(e.target.value);
    };

    const handleBackgroundClick = (id) => {
        setBackgroundId(id);
        setClicked(!clicked);
    };

    const handleCreateBoard = async () => {
        if (boardTitle.length < 2 || boardTitle.length > 20) {
            setErrorMessage('Board title must be between 2 and 20 characters.');
            return;
        }

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
            console.log('Error response data: ', error.response.data);
        }
    };

    if(!open) return null;

    return (
        <div
            className={`
        fixed z-30 inset-0 flex justify-center items-center transition-colors 
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
                <p className="w-full origin-left font-sans text-gray-500 font-bold text-l">Create Board</p>
                <hr className="w-full border-gray-400 mt-3"></hr>
                <br></br>
                <p className="w-full origin-left font-sans text-gray-500 font-semibold text-l">Background</p>
                <br></br>
                {/* <div className="flex flex-wrap justify-between gap-2">
                    {backgrounds.map((background) => (
                        <button
                            key={background.id}
                            onClick={() => handleBackgroundClick(background.id)}
                            className={`w-10 h-10 rounded-lg px-3 ${
                                backgroundId === background.id  && clicked? 'border-8 border-grey-500' : 'border-2 border-transparent'
                            } `}></button>
                    ))}
                </div> */}
                <br></br>
                <p className="w-full origin-left font-sans text-gray-500 font-semibold text-l">Board Title</p>
                <br></br>
                <input
                    type="text"
                    name="boardTitle"
                    id="boardTitle"
                    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
                    value={boardTitle}
                    onChange={handleTitleChange}
                />
                   {errorMessage && <p className="text-red-500 text-sm">{errorMessage}</p>}
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
