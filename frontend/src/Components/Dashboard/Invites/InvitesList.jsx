import {useState, useEffect, createContext} from "react";
import { getData,getDataWithId } from "../../../Services/FetchService";
import InvitesTable from "./InvitesTable";
import InvitesErrorModal from "./Modals/InvitesErrorModal"
import { useParams } from "react-router-dom";

export const InvitesContext = createContext();

const InvitesList = () =>{
    const [invites, setInvites] = useState([]);
    const [showInvitesErrorModal, setShowInvitesErrorModal] =useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");
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
            setErrorMessage(error.message);
            setShowInvitesErrorModal(true);
        }
    };
    useEffect(()=>{
        getInvites();
    }, []);

    const contextValue = {invites, setInvites, getInvites, 
        showInvitesErrorModal, setShowInvitesErrorModal, errorMessage, setErrorMessage};

        return(
            <InvitesContext.Provider value={contextValue}>
                <InvitesTable/>
                {showInvitesErrorModal && <InvitesErrorModal/>}
            </InvitesContext.Provider>
        );
}
export default InvitesList