import {useState, useEffect, createContext, useContext} from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import MembersTable from "./MembersTable";
import { useParams } from "react-router-dom";
import { DashboardContext } from "../../../Pages/dashboard";
import DashboardErrorModal from "../DashboardErrorModal";

export const MembersContext = createContext();

const MembersList = () =>{
    const [members, setMembers] = useState([]);
    const { workspaceId } = useParams();

    const dashboardContext = useContext(DashboardContext);

    const getMembers = async ()=>{
        try{
            if (workspaceId) {
                const membersWithWorkspaceId = await getDataWithId('/backend/Members/getAllMembersByWorkspace?workspaceId', workspaceId);
                setMembers(membersWithWorkspaceId.data);
            } else {
                const allMembers = await getData('http://localhost:5157/backend/Members/GetAllMembers');
                setMembers(allMembers.data);
            }
            
        }catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };
    useEffect(()=>{
        getMembers();
    }, [workspaceId]);

    const contextValue = {members, setMembers, getMembers};

        return(
            <MembersContext.Provider value={contextValue}>
                <MembersTable/>
                {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
            </MembersContext.Provider>
        );
}
export default MembersList