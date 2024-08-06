// Main.jsx
import React from 'react';
import Navbar from '../Components/Navbar/Navbar';
import Sidebar from '../Components/Side/Sidebar';
import { BoardsProvider } from '../Components/Side/WorkspaceContext';

const Main = () => {
  return (
    <BoardsProvider>
      <div>
      <Navbar />
        <Sidebar emri="Test" />
      </div>
    </BoardsProvider>
  );
};

export default Main;
