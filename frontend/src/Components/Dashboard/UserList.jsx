import { useState, useEffect } from 'react';
import { getData } from '../../Services/FetchService.jsx';
import UserTable from './UserTable.jsx';

const UserList = () => {
    const [users, setUsers] = useState(null);
    useEffect(() => {
        async function getUsers() {
            try {
                const data = await getData('http://localhost:5157/backend/user/adminAllUsers');
                setUsers(data);
            } catch (error) {
                console.error('Error fetching data: ',error);
            }
        }
        getUsers();
    }, []);

    //Now render logic using users

    return (
    <>
    <UserTable users= {users}/>

    </>
    );
}

export default UserList
