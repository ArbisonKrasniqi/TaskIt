import {useState, useEffect, createContext} from "react";
import { getData } from "../../../Services/FetchService";
import MembersTable from "./MembersTable";
import MembersErrorModal from "./Modals/MembersErrorModal"
export const MembersContext = createContext();

const MembersList = () =>{
    const [members, setMembers] = useState([]);
    const [showMembersErrorModal, setShowMembersErrorModal] =useState(false);
    const [errorMessage, setErrorMessage] = useState("There has been a server error!");

    const getMembers = async ()=>{
        try{
            const response = await getData('http://localhost:5157/backend/Members/GetAllMembers');
            setMembers(response.data);
        }catch(error){
            setErrorMessage(error.message);
            setShowMembersErrorModal(true);
        }
    };
    useEffect(()=>{
        getMembers();
    }, []);

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