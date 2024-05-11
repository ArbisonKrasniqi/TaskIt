import { useState, useEffect, createContext } from 'react';
import { getData } from '../../Services/FetchService.jsx';
import UserTable from './UserTable.jsx';


export const UserContext = createContext();

const UserList = () => {
    const [users, setUsers] = useState(null);
    const getUsers = async () => {
        try {
            const data = await getData('http://localhost:5157/backend/user/adminAllUsers');
            setUsers(data);
        } catch (error) {
            console.error('Error fetching data: ',error);
        }
    };
    useEffect(() => {
        getUsers();
    }, []);

    const contextValue = { users, setUsers, getUsers };
    return (
        <UserContext.Provider value={contextValue}>
            <UserTable/>
        </UserContext.Provider>
    );
}

export default UserList
