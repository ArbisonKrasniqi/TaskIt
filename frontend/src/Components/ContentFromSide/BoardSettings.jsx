import React, { useContext, useState, useEffect } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { putData, getData } from "../../Services/FetchService";
import MessageModal from "./MessageModal";

const BoardSettings = () => {
  const { board, setBoard, getBackgroundImageUrl } = useContext(WorkspaceContext);
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

  useEffect(() => {
    const fetchBackground = async () => {
      if (board && board.backgroundId) {
        try {
          const imageUrl = await getBackgroundImageUrl(board);
          setCurrentbgUrl(imageUrl);  // Set the background URL after fetching
        } catch (error) {
          console.error("Error fetching background image:", error);
          setCurrentbgUrl(''); // Handle the error by resetting the URL
        }
      } else {
        console.warn("Board or backgroundId is not defined.");
        setCurrentbgUrl('');  // Reset the background URL if board or backgroundId is undefined
      }
    };

    fetchBackground();
  }, [board, getBackgroundImageUrl]);

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
        console.error("Error fetching backgrounds:", error.message);
      }
    };

    fetchActiveBackgrounds();
  }, []);

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
      console.error('Error updating background:', error.response?.data || error.message);
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
      console.error('Error updating title:', error.response?.data || error.message);
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

  if (!board) {
    return "Loading...";
  }

  return (
    <div className="min-h-screen h-full" style={{ backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)' }}>
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
                Save
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default BoardSettings;
