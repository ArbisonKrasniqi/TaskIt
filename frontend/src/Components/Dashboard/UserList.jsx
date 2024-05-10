import { useState, useEffect } from 'react';
import { getData } from '../../Services/FetchService.jsx';

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
    <table>
        <thead>
            <tr>
                <th>#</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Date Created</th>
                <th>ID</th>
            </tr>
        </thead>
        <tbody>
            {users ? (users.map((item, index) => (
                <tr key={index}>
                    <td>{index}</td>
                    <td>{item.firstName}</td>
                    <td>{item.lastName}</td>
                    <td>{item.email}</td>
                    <td>{item.dateCreated}</td>
                    <td>{item.id}</td>
                </tr>
            ))): (
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            )}
        </tbody>
    </table>
    </>
    );
}

export default UserList
