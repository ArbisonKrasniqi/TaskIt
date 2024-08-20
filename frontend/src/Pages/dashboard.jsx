import { useEffect } from "react";
import UserList from "../Components/Dashboard/Users/UsersList.jsx";
import { useNavigate } from "react-router-dom";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";
import BoardsList from "../Components/Dashboard/Boards/BoardsList.jsx"
import { checkAndRefreshToken, getAccessToken, getRefreshToken, isTokenExpiring, refreshTokens, validateAdmin} from '../Services/TokenService.jsx';
import { jwtDecode } from 'jwt-decode';
import MembersList from "../Components/Dashboard/Members/MembersList.jsx"
import InvitesList from "../Components/Dashboard/Invites/InvitesList.jsx";
import WithAuth from "../Services/WithAuth.jsx";
import BackgroundsList from "../Components/Dashboard/Backgrounds/BackgroundsList.jsx";
import StarredBoardsList from "../Components/Dashboard/StarredBoards/StarredBoardsList.jsx";

const Dashboard = () => {

    const navigate = useNavigate();

    useEffect(() => {
        const validateUser = async () => {
          if (!checkAndRefreshToken()) {
            navigate('/login');
            return;
          }

          if (!validateAdmin()) {
            console.info("You are not an administrator.");
            navigate('/login');
            return;
        }
      }
        
        validateUser();
        const interval = setInterval(validateUser, 5 * 1000);
        return () => clearInterval(interval);
    }, []);

    return (<div className="w-[100%] h-[100%] p-0 m-0 bg-gray-800">
        <UserList/>
        
        <WorkspacesList/>

        <BoardsList/>

        <MembersList/>
        <InvitesList/>
        <BackgroundsList/>
        <StarredBoardsList/>
    </div>
);
}

export default WithAuth(Dashboard);