import { useEffect } from "react";
import UserList from "../Components/Dashboard/Users/UsersList.jsx";
import { useNavigate } from "react-router-dom";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";
import BoardsList from "../Components/Dashboard/Boards/BoardsList.jsx"
import { checkAndRefreshToken, validateAdmin} from '../Services/TokenService.jsx';
import MembersList from "../Components/Dashboard/Members/MembersList.jsx"
import InvitesList from "../Components/Dashboard/Invites/InvitesList.jsx";
import WithAuth from "../Services/WithAuth.jsx";
import BackgroundsList from "../Components/Dashboard/Backgrounds/BackgroundsList.jsx";
import ListsList from "../Components/Dashboard/Lists/ListsList.jsx";
import TasksList from "../Components/Dashboard/Tasks/TasksList.jsx";
import { Route, Routes } from 'react-router-dom';
import WorkspaceTable from "../Components/Dashboard/WorkspaceTable.jsx";
import DashboardSideBar from "../Components/Dashboard/DashboardSidebar.jsx";

const Dashboard = () => {

    const navigate = useNavigate();

    useEffect(() => {
      const validateUser = async () => {
          try {
            if (!checkAndRefreshToken()) {
              navigate('/login');
              return;
            }
  
            if (!validateAdmin()) {
              console.info("You are not an administrator.");
              navigate('/main/workspaces');
              return;
            } 
        } catch (error) {
          console.log("Token error. Log in again.");
          document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
          document.cookie = "refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
          navigate('/login');
        }
      }
        
      validateUser();
      const interval = setInterval(validateUser, 5 * 1000);
      return () => clearInterval(interval);
    }, []);


    return (
    <div className="flex h-screen bg-gray-800">
      {/*Sidebar*/}
      <DashboardSideBar/>
      <div className="flex-1 ml-[15%] p-4 bg-gray-800">
        <Routes>
          <Route path="users/" element={<UserList/>}/>
          <Route path="workspaces/:userId?" element={<WorkspacesList/>}/>
          <Route path="workspace/:workspaceId" element={<WorkspaceTable/>}/>
          <Route path="boards/:workspaceId?" element={<BoardsList/>}/>
          <Route path="members/:workspaceId?" element={<MembersList/>}/>
          <Route path="invites/:workspaceId?" element={<InvitesList/>}/>
          <Route path="backgrounds" element={<BackgroundsList/>}/>
          <Route path="lists/:boardId?" element={<ListsList/>}/>
          <Route path="tasks/:listId?" element={<TasksList/>}/>
        </Routes>
      </div>
    </div>
);
}

export default WithAuth(Dashboard);