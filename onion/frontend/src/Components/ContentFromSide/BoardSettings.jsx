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
  const { board, setBoard, getBackgroundImageUrl, getInitials } = useContext(WorkspaceContext);
  const [isEditing, setIsEditing] = useState(false);
  const [title, setTitle] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [isMessageModalOpen, setIsMessageModalOpen] = useState(false);
  const [message, setMessage] = useState('');
  const [currentbgUrl, setCurrentbgUrl] = useState('');
  const [isBackgroundModalOpen, setIsBackgroundModalOpen] = useState(false);
  const [activeBackgrounds, setActiveBackgrounds] = useState([]);
  const [activeBackgroundUrls, setActiveBackgroundUrls] = useState({});
  const [selectedBackgroundId, setSelectedBackgroundId] = useState(board?.backgroundId || null);
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
  useEffect(() => {
    const fetchBackground = async () => {
      if (board && board.backgroundId) {
        try {
          const imageUrl = await getBackgroundImageUrl(board);
          setCurrentbgUrl(imageUrl);  // Set the background URL after fetching
        } catch (error) {
          console.error("Error fetching background image");
          setCurrentbgUrl(''); // Handle the error by resetting the URL
        }
      } else {
        console.warn("Board or backgroundId is not defined.");
        setCurrentbgUrl('');  // Reset the background URL if board or backgroundId is undefined
      }
    };

    fetchBackground();
  }, [board]);

  // Fetch active backgrounds once
  useEffect(() => {
    const fetchActiveBackgrounds = async () => {
      try {
        const backgroundsResponse = await getData('http://localhost:5157/backend/background/GetActiveBackgrounds');
        const backgroundsData = backgroundsResponse.data;

        if (backgroundsData && Array.isArray(backgroundsData) && backgroundsData.length > 0) {
          setActiveBackgrounds(backgroundsData);

          const urls = {};
          for (let background of backgroundsData) {
            const url = `data:image/jpeg;base64,${background.imageDataBase64}`;
            urls[background.backgroundId] = url;
          }
          setActiveBackgroundUrls(urls);
        } else {
          console.error("No active backgrounds found.");
        }
      } catch (error) {
        console.error("Error fetching backgrounds:");
      }
    };

    fetchActiveBackgrounds();
  }, []);

  // Fetch labels for the board
  const fetchLabels = async () => {
    try {
      if (board && board.boardId) {
        const labelsResponse = await getData(`http://localhost:5157/backend/label/GetLabelsByBoardId?boardId=${board.boardId}`);
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

  // Update the board background
  const handleSaveBackground = async () => {
    try {
      const updatedBoard = {
        boardId: board.boardId,
        title: board.title,
        backgroundId: selectedBackgroundId,
        isClosed: board.isClosed,
      };

      const response = await putData('http://localhost:5157/backend/board/UpdateBoard', updatedBoard);
      console.log('Board background updated successfully!', response.data);

      setBoard(prevBoard => ({
        ...prevBoard,
        backgroundId: selectedBackgroundId
      }));

      setMessage("Background updated successfully!");
      setIsMessageModalOpen(true);
      setIsBackgroundModalOpen(false);

    } catch (error) {
      console.error('Error updating background');
    }
  };

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
      backgroundId: board.backgroundId, // Keep the current backgroundId
      isClosed: board.isClosed,
    };

    try {
      const response = await putData('http://localhost:5157/backend/board/UpdateBoard', updatedBoard);
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

  const handleBackgroundModalOpen = () => {
    setIsBackgroundModalOpen(true);
  };

  const handleBackgroundModalClose = () => {
    setIsBackgroundModalOpen(false);
  };

  const handleBackgroundSelect = async (id) => {
    setSelectedBackgroundId(id);
    const imageUrl = activeBackgroundUrls[id];
    setCurrentbgUrl(imageUrl);
  };


  const [boardActivities, setBoardActivities] = useState([]);
  const [visibleActivities, setVisibleActivities] = useState(10);
  const [searchTerm, setSearchTerm] = useState('');
  const getBoardActivities = async () =>{
    try{        
            const activityResponse = await getDataWithId("http://localhost:5157/GetBoardActivityByBoardId?BoardId", board.boardId);
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
     const fullName = `${activity.userName} ${activity.userLastName}`.toLowerCase(); //search ne baz te emrit dhe mbiemrit
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
      <div className="font-semibold font-sans text-gray-400 ml-20 pt-10">
        <h1 className="text-3xl">Board Background</h1>
        <div
          className="w-96 h-64 mt-5 border border-gray-400"
          style={{
            backgroundImage: `url(${currentbgUrl})`,
            backgroundSize: 'cover',
            backgroundPosition: 'center',
          }}
        ></div>
        <div className="mt-5">
          <button
            className="bg-gray-800 text-white px-4 py-2 rounded-md"
            onClick={handleBackgroundModalOpen}
          >
            Change Background
          </button>
        </div>
        {isBackgroundModalOpen && (
          <div className="fixed z-30 inset-0 flex justify-center items-center bg-black/20">
            <div className="relative bg-white rounded-xl shadow p-6 w-80 text-center">
              <button 
                onClick={handleBackgroundModalClose} 
                className="absolute top-2 right-2 p-1 rounded-lg text-gray-400 bg-white hover:bg-gray-50 hover:text-gray-600"
              >
                X
              </button>
              <p className="w-full font-sans text-gray-500 font-semibold text-l">Select Background</p>
              <div className="flex flex-wrap justify-center gap-2 mt-3">
                {activeBackgrounds.map((background) => (
                  <button
                    key={background.backgroundId}
                    onClick={() => handleBackgroundSelect(background.backgroundId)}
                    className={`relative w-16 h-16 rounded-lg bg-cover bg-center ${selectedBackgroundId === background.backgroundId ? "border-4 border-blue-500" : "border-2 border-transparent"}`}
                    style={{ backgroundImage: `url(${activeBackgroundUrls[background.backgroundId]})` }}
                  >
                  </button>
                ))}
              </div>
              <button 
                className="w-full py-2 mt-5 bg-gray-800 text-white rounded-md font-semibold hover:bg-gray-600"
                onClick={handleSaveBackground}
   
              >
                  Save Background
              </button>
            </div>
          </div>
        )}
      </div>
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
                                {getInitials(activity.userName, activity.userLastName)}
                            </div>
                            <div className="ml-2">
                                <p><b>{activity.userName} {activity.userLastName} </b>{activity.actionType} {activity.entityName}</p>
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