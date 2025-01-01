import { useState, useEffect, createContext } from 'react';
import { getData } from '../../../Services/FetchService.jsx';
import UsersTable from './UsersTable.jsx';
import DashboardErrorModal from '../DashboardErrorModal.jsx';
import { useContext } from 'react';
import { DashboardContext } from '../../../Pages/dashboard.jsx';

export const UserContext = createContext();

const UsersList = () => {
    const dashboardContext = useContext(DashboardContext);
    //Ketu do te ruhet lista e usereve qe vjen nga API
    const [users, setUsers] = useState(null);

    //Funksioni getUsers thirret kur deshirohet te perditesohet lista e users duke bere fetch API
    const getUsers = async () => {
        try {
            const response = await getData('/backend/user/adminAllUsers');
            setUsers(response.data);
        } catch (error) {
                    dashboardContext.setDashboardErrorMessage(error.message);
                    //Beje UserErrorModal te shfaqet
                    dashboardContext.setShowDashboardErrorModal(true);
        }
    };
    useEffect(() => {
        getUsers();
    }, []);

    

    const contextValue = { users, setUsers, getUsers};
    //Te gjithe femijet e komponentes UserTable do te kene qasje ne keto funksione dhe atribute
    

    return (
        <UserContext.Provider value={contextValue}>

            <UsersTable/>
            {/*Fillimisht DashboardErrorModal nuk shfaqet sepse showDashboardErrorModal eshte false (false && _____ == false*/}
            {dashboardContext.showDashboardErrorModal && <DashboardErrorModal/>}
        </UserContext.Provider>
    );
}

export default UsersList
