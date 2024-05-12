import { useState, useEffect, createContext } from 'react';
import { getData } from '../../Services/FetchService.jsx';
import UserTable from './UserTable.jsx';
import UserErrorModal from './UserErrorModal.jsx';


export const UserContext = createContext();

const UserList = () => {
    //Ketu do te ruhet lista e usereve qe vjen nga API
    const [users, setUsers] = useState(null);

    //showUserErrorModal e ka vleren false ne fillim sepse nuk ka asnje error
    const [showUserErrorModal, setShowUserErrorModal] = useState(false);
    //Error message do te perdoret per te vendosur vlera te ndryshme ne showUserErrorModal
    const [errorMessage, setErrorMessage] = useState("There has been a server error!")

    //Funksioni getUsers thirret kur deshirohet te perditesohet lista e users duke bere fetch API
    const getUsers = async () => {
        try {
            const data = await getData('http://localhost:5157/backend/user/adminAllUsers');
            setUsers(data);
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

            <UserTable/>
            {/*Fillimisht UserErrorModal nuk shfaqet sepse showUserErrorModal eshte false (false && _____ == false*/}
            {showUserErrorModal && <UserErrorModal/>}
        </UserContext.Provider>
    );
}

export default UserList
