import { useContext, useState } from "react"
import { WorkspaceContext } from "../Side/WorkspaceContext"
import { putData } from "../../Services/FetchService";
import MessageModal from "./MessageModal";
const BoardSettings = ()=>{

const {board, setBoard} = useContext(WorkspaceContext);
const [isEditing, setIsEditing] = useState(false);
const[title, setTitle] =useState('');
const[errorMessage, setErrorMessage] = useState('');
const [isMessageModalOpen, setIsMessageModalOpen] = useState(false);
const [message, setMessage] = useState('');

if (board == null) {
    return <div>Loading...</div>;
}

const handleEditClick = () =>{
    setTitle(board.title || ''); 
    setIsEditing(true);
}
const handleInputChange = (e) =>{
    setTitle(e.target.value);
}


const  handleSaveClick = async () =>{
    if(title.length<2 || title.length>280){
        setErrorMessage('Board title must be between 2 and 280 characters.');
        return;
    }

    const updatedBoard = {
        boardId: board.boardId,
        title: title,
        backgroundId: board.backgroundId,
        isClosed: board.isClosed,
    }
    console.log('Updating board with data: ',updatedBoard);
    try{
        const response = await putData('http://localhost:5157/backend/board/UpdateBoard', updatedBoard);
    console.log('Board updated successfully! ',response.data);

    setBoard(prevBoard => ({
        ...prevBoard, title: title
    }));
    
    setIsEditing(false);
    setMessage("Board title updated successfully!");
    setIsMessageModalOpen(true);  

    }catch (error) {
        console.log('Error response data: ', error.response?.data || error.message);
    }
};
return(
    <div className="min-h-screen h-full" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
    <div className="font-semibold font-sans text-gray-400 ml-20 pt-10">
    <h1 className="text-3xl">
       Board Settings</h1>
       <h1 className="text-2xl mt-5">Board Title:</h1>
       {!isEditing ? ( 
        <div>
             <p className="text-2xl mt-3 mb-5">{board.title}</p>
             <button className="text-blue-500 hover:text-blue-700" onClick={handleEditClick}>
                    Edit board title
                </button>
        </div>): (
            <div>
            <input 
                type="text" 
                value={title} 
                onChange={handleInputChange}
                className="text-xl mt-3 mb-10 p-2 border border-gray-500 rounded"
            />
            <button 
                className="text-blue-500 hover:text-blue-700 ml-4"
                onClick={handleSaveClick}
            >
                Save
            </button>
            {errorMessage && <p className="text-red-500">{errorMessage}</p>}
        </div>
        
       
       )}
         <MessageModal 
                    isOpen={isMessageModalOpen} 
                    message={message} 
                    duration={2000}
                    onClose={() => setIsMessageModalOpen(false)} 
                />
 </div>
</div>
);
}
export default BoardSettings;
