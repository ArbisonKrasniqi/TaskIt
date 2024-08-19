import {useState, useEffect, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import InvitesTable from "./InvitesTable";
import InvitesErrorModal from "./Modals/InvitesErrorModal"

export const InvitesContext = createContext();

const InvitesList = () =>{
    const [invites, setInvites] = useState([]);
    const [showInvitesErrorModal, setShowInvitesErrorModal] =useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");

    const getInvites = async () =>{
        try{
            const response = await getData("http://localhost:5157/backend/invite/GetAllInvites");
            setInvites(response.data);
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