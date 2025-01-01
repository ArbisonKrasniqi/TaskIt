import {useState, useEffect, createContext, useContext} from "react";
import { getData,getDataWithId } from "../../../Services/FetchService";
import InvitesTable from "./InvitesTable";
import DashboardErrorModal from "../DashboardErrorModal";
import { useParams } from "react-router-dom";
import {DashboardContext} from "../../../Pages/dashboard";

export const InvitesContext = createContext();

const InvitesList = () =>{
    const [invites, setInvites] = useState([]);
    const dashboardContext = useContext(DashboardContext);
    const { workspaceId } = useParams();

    const getInvites = async () =>{
        try{
            if (workspaceId) {
                console.log(workspaceId);
                const response = await getDataWithId("/backend/invite/GetInvitesByWorkspace?workspaceId", workspaceId);
                setInvites(response.data);
            } else {
                const response = await getData("/backend/invite/GetAllInvites");
                setInvites(response.data);
            }
            
        }
        catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
        }
    };
    useEffect(()=>{
        getInvites();
    }, []);

    const contextValue = {invites, setInvites, getInvites};

        return(
            <InvitesContext.Provider value={contextValue}>
                <InvitesTable/>
                {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
            </InvitesContext.Provider>
        );
}
export default InvitesList