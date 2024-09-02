import {useState, useEffect, createContext} from "react";
import { getData, getDataWithId } from "../../../Services/FetchService";
import MembersTable from "./MembersTable";
import MembersErrorModal from "./Modals/MembersErrorModal"
import { useParams } from "react-router-dom";

export const MembersContext = createContext();

const MembersList = () =>{
    const [members, setMembers] = useState([]);
    const [showMembersErrorModal, setShowMembersErrorModal] =useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");
    const { workspaceId } = useParams();

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
            setErrorMessage(error.message);
            setShowMembersErrorModal(true);
        }
    };
    useEffect(()=>{
        getMembers();
    }, [workspaceId]);

    const contextValue = {members, setMembers, getMembers, 
        showMembersErrorModal, setShowMembersErrorModal, errorMessage, setErrorMessage};

        return(
            <MembersContext.Provider value={contextValue}>
                <MembersTable/>
                {showMembersErrorModal && <MembersErrorModal/>}
            </MembersContext.Provider>
        );
}
export default MembersList