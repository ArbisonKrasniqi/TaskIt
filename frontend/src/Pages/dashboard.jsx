import { useEffect } from "react";
import UserList from "../Components/Dashboard/Users/UsersList.jsx";
import { useNavigate } from "react-router-dom";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";
import BoardsList from "../Components/Dashboard/Boards/BoardsList.jsx"
import { getAccessToken, getRefreshToken, isTokenExpiring, refreshTokens, validateAdmin} from '../Services/TokenService.jsx';
import { jwtDecode } from 'jwt-decode';

const Dashboard = () => {

    const navigate = useNavigate();

    useEffect(() => {
        const checkAndRefreshToken = async () => {
    
          const accessToken = getAccessToken();
          const refreshToken = getRefreshToken();
          if (!refreshToken) {
            console.info("U IS NOT LOGGED IN FAM");
            navigate('/login');
            return;
          }

          if (accessToken && refreshToken) {
            
            const decodedToken = jwtDecode(accessToken);
            const expiryTime = decodedToken.exp;

            if (isTokenExpiring(expiryTime)) {
              var refreshResult = await refreshTokens();
              if (refreshResult) {
                console.info("Tokens successfully refreshed because it almost expired");
                return;
              } else {
                console.info("Refresh token invalid! Please login again!");
                navigate('/login');
                return;
              }
            }
          } else if (!accessToken && refreshToken) {
            var refreshResult = await refreshTokens();
            if (refreshResult) {
              console.info("Token successfully refreshed because it expired");
            } else {
                console.info("Refresh token invalid! Please login again!");
                navigate('/login');
                return;
            }
          }

          const isAdmin = validateAdmin();
          if (!isAdmin) {
              console.info("You are not an administrator.");
              navigate('/login');
              return;
          }
        }
        checkAndRefreshToken();
        const interval = setInterval(checkAndRefreshToken, 5 * 1000);
        return () => clearInterval(interval);
    }, []);

    console.log("Is admin: "+validateAdmin());
    return (<div className="w-[100%] h-[100%] p-0 m-0 bg-gray-800">
        <UserList/>
        
        <WorkspacesList/>

        <BoardsList/>
    </div>
);
}

export default Dashboard