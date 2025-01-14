import React, { useContext, useState, useEffect, createContext } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { putData, getData, getDataWithId } from "../../Services/FetchService";
import MessageModal from "./MessageModal";
import { RxActivityLog } from "react-icons/rx";
import BoardLabelsModal from "./BoardLabelsModal";
import EditBoardLabelModal from "./EditBoardLabelModal";
// board settings contexti
export  const BoardSettingsContext = createContext();


 const BoardSettings = ({ children }) => {
  const { board, setBoard, getInitials } = useContext(WorkspaceContext);
  const [isEditing, setIsEditing] = useState(false);
  const [title, setTitle] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [isMessageModalOpen, setIsMessageModalOpen] = useState(false);
  const [message, setMessage] = useState('');
  const [labels, setLabels] = useState([]);
  const [isLabelModalOpen, setIsLabelModalOpen] = useState(false);
  const [isEditLabelModalOpen, setIsEditLabelModalOpen] = useState(false);
  const [selectedLabel, setSelectedLabel] = useState(null);


  const toggleLabelsModal = () => {
    if (!isLabelModalOpen) {
        setIsLabelModalOpen(true);
        setIsEditLabelModalOpen(false);
    } else {
        setIsLabelModalOpen(false);
    }
};
const toggleEditLabelModal = (label) => {
  if (!isEditLabelModalOpen) {
      setSelectedLabel(label);
      setIsLabelModalOpen(false);
      setIsEditLabelModalOpen(true);
  } else {
      setIsEditLabelModalOpen(false);
  }
}


  // Fetch labels for the board
  const fetchLabels = async () => {
    try {
      if (board && board.boardId) {
        const labelsResponse = await getData(`http://localhost:5127/backend/label/GetLabelsByBoardId?boardId=${board.boardId}`);
        const labelsData = labelsResponse.data;
        setLabels(labelsData);
      }
    } catch (error) {
      console.error("Error fetching labels");
    }
  };
  useEffect(() => {
    fetchLabels();
  }, [board]);

  const handleEditClick = () => {
    setTitle(board.title || ''); 
    setIsEditing(true);
  };

  const handleInputChange = (e) => {
    setTitle(e.target.value);
  };

  const handleSaveTitle = async () => {
    if (title.length < 2 || title.length > 280) {
      setErrorMessage('Board title must be between 2 and 280 characters.');
      return;
    }

    const updatedBoard = {
      boardId: board.boardId,
      title: title,
      isClosed: board.isClosed,
    };

    try {
      const response = await putData('http://localhost:5127/backend/board/UpdateBoard', updatedBoard);
      console.log('Board title updated successfully!', response.data);

      setBoard(prevBoard => ({
        ...prevBoard,
        title: title
      }));
      setErrorMessage('');
      setIsEditing(false);
      setMessage("Title updated successfully!");
      setIsMessageModalOpen(true);

    } catch (error) {
      console.error('Error updating title');
    }
  };


  const [boardActivities, setBoardActivities] = useState([]);
  const [visibleActivities, setVisibleActivities] = useState(10);
  const [searchTerm, setSearchTerm] = useState('');
  const getBoardActivities = async () =>{
    try{        
            const activityResponse = await getDataWithId("http://localhost:5127/GetBoardActivityByBoardId?BoardId", board.boardId);
            console.log("Te dhenat e Board aktivitetit ",activityResponse.data)
            const activityData = activityResponse.data;
            if (activityData && Array.isArray(activityData) && activityData.length > 0) {
                setBoardActivities(activityData);
            } else {
                setBoardActivities([]);
                console.log("There is no board activity");
            }
      
        //Waiting for userIdn
    } catch (error) {
        console.error("There has been an error fetching board activities")
        setBoardActivities([]);
    }
  };
   useEffect(()=>{
     getBoardActivities();
 },[board]);
 
 
 const filteredBoardActivities = boardActivities.filter((activity) => {
     const fullName = `${activity.userFirstName} ${activity.userLastName}`.toLowerCase(); //search ne baz te emrit dhe mbiemrit
     return fullName.includes(searchTerm.toLowerCase());
 });
 
 
     const formatDateTime = (dateString) => {
     const date = new Date(dateString);
     const formattedDate = date.toLocaleDateString('en-US');
     const formattedTime = date.toLocaleTimeString('en-US', {
         hour: '2-digit',
         minute: '2-digit',
         hour12: true 
     });
     return `${formattedDate} - ${formattedTime}`;
     };
         const loadMoreActivities = () => {
             setVisibleActivities(prev => prev + 10);
         };

  if (!board) {
    return "Loading...";
  }

  const values = {
    toggleEditLabelModal,
    toggleLabelsModal,
    labels,
    selectedLabel,
  setIsLabelModalOpen,
    fetchLabels,
    getBoardActivities
  };

  return (
    <BoardSettingsContext.Provider value={values}>
    <div className="min-h-screen h-full overflow-y-auto" style={{ backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)' }}>
      <div className="font-semibold font-sans text-gray-400 ml-20 pt-10">
        <h1 className="text-3xl">Board Settings</h1>
        <h1 className="text-2xl mt-5">Board Title:</h1>
        {!isEditing ? (
          <div>
            <p className="text-2xl mt-3 mb-5">{board.title}</p>
            <button className="text-blue-500 hover:text-blue-700" onClick={handleEditClick}>
              Edit board title
            </button>
          </div>
        ) : (
          <div>
            <input 
              type="text" 
              value={title} 
              onChange={handleInputChange}
              className="text-xl mt-3 mb-10 p-2 border border-gray-500 rounded"
            />
            <button 
              className="text-blue-500 hover:text-blue-700 ml-4"
              onClick={handleSaveTitle}
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
      <hr className="w-full border-gray-400 mt-2" />
      <div className="font-semibold font-sans text-gray-400 ml-20 pt-10 mr-20">
      <h1 className="text-3xl">Board Labels</h1>
        <div className="mt-5 flex flex-wrap gap-4">
            {labels.map((label) => (
             <div 
                 key={label.labelId}
                 className="p-2 rounded-lg border border-gray-300 flex items-center justify-center w-[150px] h-[50px]"
                 style={{ backgroundColor: label.color }}
             >
              <p className="text-white truncate">{label.name}</p>
            </div>
             ))}
        <div className="mt-3 mb-5">
          <button 
            className="bg-gray-800 text-white px-4 py-2 rounded-md"
            onClick={toggleLabelsModal}
          >
            Edit Labels
          </button>
        </div>
        

     </div> 
    </div>
             <br/>

      <h1 className="text-3xl mt-10 ml-20 mb-10 font-semibold font-sans text-gray-400 flex flex-row items center "> <RxActivityLog className="mt-1 mr-3"/>Board Activity</h1>

      {/* Search Bar */}
      <div className="ml-20 mb-6">
      <input
        type="text"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        placeholder="Search by name..."
        className="p-2 border border-gray-400 rounded w-1/3 bg-transparent"
      />
    </div>

{/* Activity List */}

  
    <div className="mt-3 ml-10 max-h-[500px] overflow-y-auto"
     style={{
      scrollbarWidth: 'thin',
      scrollbarColor: 'rgba(0, 0, 0, 0.2) transparent',
    }}>
                {filteredBoardActivities
                    .sort((a, b) => new Date(b.actionDate) - new Date(a.actionDate))
                    .slice(0, visibleActivities)
                    .map((activity, index) => (
                        <div key={index} className="flex items-center text-gray-300 mb-4">
                            <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-m text-white bg-gradient-to-r from-orange-400 to-orange-600">
                                {getInitials(activity.userFirstName, activity.userLastName)}
                            </div>
                            <div className="ml-2">
                                <p><b>{activity.userFirstName} {activity.userLastName} </b>{activity.actionType} {activity.entityName}</p>
                                <p className="text-sm text-gray-500">{formatDateTime(activity.actionDate)}</p>
                            </div>
                        </div>
                    ))
                }
                {visibleActivities < filteredBoardActivities.length && (
                    <button
                        className="p-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                        onClick={loadMoreActivities}
                    >
                        Load More
                    </button>
                )}
            </div>
            <hr className="w-full border-gray-400 mt-5"></hr>
                <br/><br/>
   
    <br/>
    <br/>
    {isLabelModalOpen && <BoardLabelsModal/>}
    {isEditLabelModalOpen && <EditBoardLabelModal/>}
    </div>
    </BoardSettingsContext.Provider>
  );
};

export default BoardSettings;