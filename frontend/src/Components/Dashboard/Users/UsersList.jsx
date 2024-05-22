import { useState, useEffect, createContext } from 'react';
import { getData } from '../../../Services/FetchService.jsx';
import UsersTable from './UsersTable.jsx';
import UserErrorModal from './Modals/UserErrorModal.jsx';


export const UserContext = createContext();

const UsersList = () => {
    //Ketu do te ruhet lista e usereve qe vjen nga API
    const [users, setUsers] = useState(null);

    //showUserErrorModal e ka vleren false ne fillim sepse nuk ka asnje error
    const [showUserErrorModal, setShowUserErrorModal] = useState(false);
    //Error message do te perdoret per te vendosur vlera te ndryshme ne showUserErrorModal
    const [errorMessage, setErrorMessage] = useState("There has been a server error!")

    //Funksioni getUsers thirret kur deshirohet te perditesohet lista e users duke bere fetch API
    const getUsers = async () => {
        try {
            const response = await getData('/backend/user/adminAllUsers');
            setUsers(response.data);
        } catch (error) {
                    setErrorMessage(error.message);
                    //Beje UserErrorModal te shfaqet
                    setShowUserErrorModal(true);
        }
    };
    useEffect(() => {
        getUsers();
    }, []);

    

    const contextValue = { users, setUsers, getUsers, setShowUserErrorModal, showUserErrorModal, errorMessage, setErrorMessage};
    //Te gjithe femijet e komponentes UserTable do te kene qasje ne keto funksione dhe atribute
    

    return (
        <UserContext.Provider value={contextValue}>

            <UsersTable/>
            {/*Fillimisht UserErrorModal nuk shfaqet sepse showUserErrorModal eshte false (false && _____ == false*/}
            {showUserErrorModal && <UserErrorModal/>}
        </UserContext.Provider>
    );
}

export default UsersList
