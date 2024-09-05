import { useContext } from "react";
import List from "../List/List";
import { useContext, useEffect, useState } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const Board = () => {
  const { board, getBackgroundImageUrl } = useContext(WorkspaceContext);
  const [backgroundUrl, setBackgroundUrl] = useState('');

  useEffect(() => {
    const fetchBackground = async () => {
      if (board && board.backgroundId) {
        try {
          const imageUrl = await getBackgroundImageUrl(board);
          setBackgroundUrl(imageUrl);  // Set the background URL after fetching
        } catch (error) {
          console.error("Error fetching background image:", error);
        }
      } else {
        console.warn("Board or backgroundId is not defined.");
        setBackgroundUrl('');  // Reset the background URL if board or backgroundId is undefined
      }
    };

    fetchBackground();
  }, [board, getBackgroundImageUrl]);  // Re-fetch the background when the board or backgroundId changes

  return (
    <div
      className="board"
      style={{
        backgroundImage: `url(${backgroundUrl})`,  // Apply the background URL
        backgroundSize: 'cover',
        backgroundPosition: 'center',
      }}
    >
      <List />
    </div>
  );
};

export default Board;
