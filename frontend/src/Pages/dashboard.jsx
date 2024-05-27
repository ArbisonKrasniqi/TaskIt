import { useEffect } from "react";
import UserList from "../Components/Dashboard/Users/UsersList.jsx";
import { ValidateAdmin, ValidateToken} from "../Services/TokenService";
import { useNavigate } from "react-router-dom";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";
import BoardsList from "../Components/Dashboard/Boards/BoardsList.jsx"

const Dashboard = () => {

    const navigate = useNavigate();


    useEffect(() => {
        const isValid = ValidateToken();
        if (!isValid) {
            console.error("You are not logged in");
            // Handle refresh token?
            
            navigate('/logIn');
            return;
        }

        const isAdmin = ValidateAdmin();
        if (!isAdmin) {
            console.error("You are not an administrator.");
            navigate('/');
            return;
        }
    }, []);
    console.log(ValidateAdmin());
    return (<div className="w-[100%] h-[100%] p-0 m-0 bg-gray-800">
        <UserList/>
        
        <WorkspacesList/>

        <BoardsList/>
    </div>
);
}

export default Dashboard