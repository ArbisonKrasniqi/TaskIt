import { useEffect } from "react";
import UserList from "../Components/Dashboard/Users/UsersList.jsx";
import { useNavigate } from "react-router-dom";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";
import BoardsList from "../Components/Dashboard/Boards/BoardsList.jsx"
import { checkAndRefreshToken, getAccessToken, getRefreshToken, isTokenExpiring, refreshTokens, validateAdmin} from '../Services/TokenService.jsx';
import { jwtDecode } from 'jwt-decode';
import MembersList from "../Components/Dashboard/Members/MembersList.jsx"
import InvitesList from "../Components/Dashboard/Invites/InvitesList.jsx";

const Dashboard = () => {

    const navigate = useNavigate();

    useEffect(() => {
        const validateUser = async () => {
          if (await !checkAndRefreshToken()) {
            console.info("You are not logged in");
            navigate('/login');
          }
          const isAdmin = validateAdmin();
          if (!isAdmin) {
              console.info("You are not an administrator.");
              navigate('/login');
              return;
          }
        }
        
        validateUser();
        const interval = setInterval(validateUser, 5 * 1000);
        return () => clearInterval(interval);
    }, []);

    console.log("Is admin: "+validateAdmin());
    return (<div className="w-[100%] h-[100%] p-0 m-0 bg-gray-800">
        <UserList/>
        
        <WorkspacesList/>

        <BoardsList/>

        <MembersList/>
        <InvitesList/>
    </div>
);
}

export default Dashboard