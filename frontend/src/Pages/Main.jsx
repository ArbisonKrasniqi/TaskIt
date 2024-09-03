import React, { createContext, useEffect } from 'react';
import Navbar from '../Components/Navbar/Navbar';
import Sidebar from '../Components/Side/Sidebar';
import { WorkspaceProvider } from '../Components/Side/WorkspaceContext';
import Boards from '../Components/ContentFromSide/Boards';
import Workspaces from '../Components/ContentFromSide/Workspaces';
import WorkspaceSettings from '../Components/ContentFromSide/WorkspaceSettings';
import { MainContext } from './MainContext.jsx';
import { getAccessToken, checkAndRefreshToken } from '../Services/TokenService.jsx';
import { useParams } from 'react-router-dom';
import { useState } from "react"; 
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import Table from '../Components/ContentFromSide/Table.jsx';
import Calendar from '../Components/ContentFromSide/Calendar.jsx';
import LoadingModal from '../Components/Modal/LoadingModal.jsx';
import Members from '../Components/ContentFromSide/Members.jsx';
import WithAuth from "../Services/WithAuth.jsx";
import TaskModal from '../Components/TaskModal/TaskModal.jsx';
import Board from '../Components/BoardContent/Board.jsx';

const Main = () => {
    const {opened, workspaceId, boardId, taskId} = useParams();
    const navigate = useNavigate();

    const [userInfo, setUserInfo] = useState({});
    useEffect(() => {
        const updateUserInfoToken = async () => {
            try {
                if (await !checkAndRefreshToken()){ //If invalid refresh
                    navigate('/login');
                    return;
                }
                const accessToken = getAccessToken();
                
                if (accessToken) {
                    const decodedToken = jwtDecode(accessToken);
                    setUserInfo({
                        userId: decodedToken.Id,
                        email: decodedToken.Email,
                        role: decodedToken.Role,
                        accessToken: decodedToken
                    });
                } else {
                    navigate('/login'); //If no access token exists
                    return;
                }
            } catch (error) {
                console.log("There has been an error, please log in again.");
                document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
                document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
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
                        {opened !== 'workspaces' && <Sidebar />}

                        {/* Conditional rendering based on the value of `opened` */}
                        <div className='w-full flex-grow h-full p-0'>
                            {opened === 'boards' && <Boards />}
                            {opened === 'board' && <Board />}
                            {opened === 'workspaceSettings' && <WorkspaceSettings/>}
                            {opened === 'workspaces' && <Workspaces/>}
                            {opened === 'table' && <Table/>}
                            {opened === 'calendar' && <Calendar/>}
                            {opened === 'loadingModal' && <LoadingModal/>}
                            {opened === 'members' && <Members />}
                            {opened === 'task' && <TaskModal/>}
                        </div>
                    </div>
                </div>
            </WorkspaceProvider>
        </MainContext.Provider>
    );
}
export default WithAuth(Main);