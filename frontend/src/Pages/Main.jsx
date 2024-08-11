// Main.jsx
import React, { createContext, useEffect } from 'react';
import Navbar from '../Components/Navbar/Navbar';
import Sidebar from '../Components/Side/Sidebar';
import Boards from '../Components/ContentFromSide/Boards';
import { MainContext } from './MainContext.jsx';
import { getAccessToken, checkAndRefreshToken } from '../Services/TokenService.jsx';
import { WorkspaceProvider } from '../Components/Side/WorkspaceContext';
import { useParams } from 'react-router-dom';
import { useState } from "react"; 
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const Main = () => {
    const {opened, workspaceId, boardId, taskId} = useParams();
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({});
    useEffect(() => {
        const updateUserInfoToken = async () => {
            const accessToken = getAccessToken();
            
            if (accessToken) {
                const decodedToken = jwtDecode(accessToken);
                setUserInfo({
                    userId: decodedToken.Id,
                    email: decodedToken.Email,
                    role: decodedToken.Role
                });
            } else {
                navigate('/login'); //If no access token exists
            }

            if (!checkAndRefreshToken()){ //If invalid refresh
                navigate('/login');
            }
        }
        updateUserInfoToken();
        
        const interval = setInterval(updateUserInfoToken, 5 * 1000);
        return () => clearInterval(interval);
    }, []);

    const mainContextValue = {
        opened, workspaceId, boardId, taskId, userInfo
    }

    return (
        <MainContext.Provider value={mainContextValue}>
            <WorkspaceProvider>
                <div className="w-full h-full flex flex-col">
                    {/* Navbar at the top */}
                    <Navbar />
                    
                    {/* Container for Sidebar and Boards */}
                    <div className="flex flex-grow h-full p-0">
                        {/* Sidebar on the left */}
                        <Sidebar />

                        {/* Conditional rendering based on the value of `opened` */}
                        <div className='w-full flex-grow h-full p-0'>
                            {opened === 'boards' && <Boards />}
                            {/* {opened === 'members' && <Members />} */}
                        </div>
                    </div>
                </div>
            </WorkspaceProvider>
        </MainContext.Provider>
    );
}
export default Main
