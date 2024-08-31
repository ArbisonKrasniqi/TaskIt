import React, { useState, useContext, createContext } from 'react';
import UpdateUserButton from "./Buttons/UpdateUserButton.jsx";
import { deleteData } from '../../../Services/FetchService.jsx';
import { UserContext } from "./UsersList.jsx";
import CustomButton from "../Buttons/CustomButton.jsx";
import { useNavigate } from "react-router-dom";

export const UpdateContext = createContext();

const UsersTable = () => {
    const userContext = useContext(UserContext);
    const navigate = useNavigate();
    const [searchQuery, setSearchQuery] = useState('');

    const handleUserDelete = (id) => {
        async function deleteUser() {
            try {
                const data = { id };
                const response = await deleteData('/backend/user/adminDeleteUserById', data);
                console.log(response);
                const updatedUsers = (userContext.users || []).filter(user => user.id !== id);
                userContext.setUsers(updatedUsers);
            } catch (error) {
                userContext.setErrorMessage(error.message);
                userContext.setShowUserErrorModal(true);
                userContext.getUsers();
            }
        }
        deleteUser();
    }

    const handleRowClick = userId => {
        console.log(userId);
        navigate(`/dashboard/workspaces/${userId}`);
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    // Ensure userContext.users is always an array
    const users = userContext.users || [];
    const filteredUsers = users.filter(user => {
        return (
            user.firstName.toLowerCase().includes(searchQuery) ||
            user.lastName.toLowerCase().includes(searchQuery) ||
            user.email.toLowerCase().includes(searchQuery) ||
            user.id.toString().toLowerCase().includes(searchQuery) ||
            user.role.toLowerCase().includes(searchQuery) ||
            (user.dateCreated ? user.dateCreated.toLowerCase().includes(searchQuery) : false)
        );
    });

    return (
        <div className="flex flex-col h-full overflow-x-auto">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for users by name, email, ID, role, or date created"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            {/* Scrollable Table Container */}
            <div className="flex-grow max-h-[600px] overflow-y-auto">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="sticky top-0 text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">First Name</th>
                            <th className="px-6 py-3">Last Name</th>
                            <th className="px-6 py-3">Email</th>
                            <th className="px-6 py-3">Date Created</th>
                            <th className="px-6 py-3">User ID</th>
                            <th className="px-6 py-3">Role</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredUsers.length > 0 ? (
                            filteredUsers.map((user, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 cursor-pointer"
                                    onClick={() => handleRowClick(user.id)}
                                >
                                    <td className="px-6 py-4">{user.firstName}</td>
                                    <td className="px-6 py-4">{user.lastName}</td>
                                    <td className="px-6 py-4">{user.email}</td>
                                    <td className="px-6 py-4">{user.dateCreated}</td>
                                    <td className="px-6 py-4">{user.id}</td>
                                    <td className="px-6 py-4">{user.role}</td>
                                    <td className="px-6 py-4">
                                        <UpdateContext.Provider value={user}>
                                            <UpdateUserButton
                                                onClick={(e) => e.stopPropagation()}
                                            />
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                handleUserDelete(user.id);
                                            }}
                                            type="button"
                                            text="Delete"
                                            color="red"
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={7}>No users found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default UsersTable;
