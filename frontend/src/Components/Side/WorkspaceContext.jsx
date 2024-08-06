// BoardsContext.js
import React, { createContext, useContext, useState } from 'react';

const BoardsContext = createContext();

export const useBoards = () => useContext(BoardsContext);


export const BoardsProvider = ({ children }) => {
    
    const [boards, setBoards] = useState([]);
    const [workspaces, setWorkspaces] = useState([]);
  
  const handleCreateBoard = (newBoard) => {
    setBoards((prevBoards) => [...prevBoards, newBoard]);
  };

  const handleCreateWorkspace = (newWorkspace) => {
    setWorkspaces((prevWorkspaces) => [...prevWorkspaces, newWorkspace]);
}

  return (
    <BoardsContext.Provider value={{ boards, workspaces, setBoards, setWorkspaces, handleCreateBoard, handleCreateWorkspace }}>
      {children}
    </BoardsContext.Provider>
  );
};
