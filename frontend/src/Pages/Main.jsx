// Main.jsx
import React from 'react';
import Navbar from '../Components/Navbar/Navbar';
import Sidebar from '../Components/Side/Sidebar';
import Boards from '../Components/ContentFromSide/Boards';
import { WorkspaceProvider } from '../Components/Side/WorkspaceContext';
import { useState } from "react"; 
const Main = () => {
    const [open, setOpen] = useState(true);
  
    
    return (   
    
    <WorkspaceProvider>
        <div className="w-full h-full flex flex-col">
            {/* Navbar at the top */}
            <Navbar />
            
            {/* Container for Sidebar and Boards */}
            <div className="flex flex-grow">
                {/* Sidebar on the left */}
                <Sidebar />
                
                {/* Boards content on the right */}
                <div className='w-full flex-grow'>
                    <Boards />
                </div>
            </div>
        </div>
    </WorkspaceProvider>
    );
}
export default Main
