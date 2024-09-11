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
// import jwtDecode from 'jwt-decode';
import { jwtDecode } from 'jwt-decode';
import Table from '../Components/ContentFromSide/Table.jsx';
import Calendar from '../Components/ContentFromSide/Calendar.jsx';
import LoadingModal from '../Components/Modal/LoadingModal.jsx';
import Members from '../Components/ContentFromSide/Members.jsx';
import WithAuth from "../Services/WithAuth.jsx";
import PrivacyPolicy from '../Components/Navbar/PrivacyPolicy.jsx';
import Profile from '../Components/Navbar/Profile.jsx';
import TaskModal from '../Components/TaskModal/TaskModal.jsx';
import Board from '../Components/BoardContent/Board.jsx';
import BoardSettings from '../Components/ContentFromSide/BoardSettings.jsx';

// import Preview from '../Components/Preview/preview.jsx';
import AboutUs from '../Components/Preview/aboutus.jsx';
import ContactUs from '../Components/Preview/contactus.jsx';

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
                        name: decodedToken.Name,
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
                <div className="w-screen h-screen flex flex-col overflow-hidden">
                    {/* Navbar at the top */}
                    <Navbar />
                    
                    {/* Container for Sidebar and Boards */}
                    <div className="flex flex-grow h-full p-0">
                        {/* Sidebar on the left */}
                        {opened !== 'workspaces' && opened !== 'privacyPolicy'  && opened !== 'profile' && <Sidebar />}

                        {/* Conditional rendering based on the value of `opened` */}
                        <div className='w-full flex-grow h-full p-0 m-0'>
                            {opened === 'boards' && <Boards />}
                            {opened === 'board' && <Board />}
                            {opened === 'workspaceSettings' && <WorkspaceSettings/>}
                            {opened === 'workspaces' && <Workspaces/>}
                            {opened === 'table' && <Table/>}
                            {opened === 'calendar' && <Calendar/>}
                            {opened === 'loadingModal' && <LoadingModal/>}
                            {opened === 'members' && <Members />}
                            {opened === 'aboutus' && <AboutUs/>}
                            {opened === 'contactus' && <ContactUs/>}
                            {opened === 'privacyPolicy' && <PrivacyPolicy/>}
                            {opened === 'profile' && <Profile/>}
                            {opened === 'task' && <TaskModal/>}
                            {opened === 'boardSettings' &&  <BoardSettings/>}

                        </div>
                    </div>
                </div>
            </WorkspaceProvider>
        </MainContext.Provider>
    );
}
export default WithAuth(Main);