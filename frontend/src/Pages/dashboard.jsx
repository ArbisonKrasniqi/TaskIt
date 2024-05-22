import UserList from "../Components/Dashboard/Users/UsersList";
import WorkspacesList from "../Components/Dashboard/Workspaces/WorkspacesList";

const Dashboard = () => {

    return (<div className="w-[100%] h-[100%] p-0 m-0 bg-gray-800">
        <UserList/>
        
        <WorkspacesList/>
    </div>
);
}

export default Dashboard