import { UpdateContext } from './UserTable';
import { UserContext } from './UserList';
import { useContext, useState } from 'react';
import { putData } from '../../Services/FetchService';

const EditRoleModal = (props) => {
    const updateContext = useContext(UpdateContext);
    const userContext = useContext(UserContext);

    const [isAdmin, setIsAdmin] = useState(updateContext.role === "Admin");

    const toggleIsAdmin = () => {
        setIsAdmin(!isAdmin);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const data = {
                id: updateContext.id,
                isAdmin: isAdmin
            }

            const response = await putData('http://localhost:5157/backend/user/adminUpdateRole', data);
            console.log(response);

            const updatedUsers = userContext.users.map(user => {
                if (user.id === updateContext.id) {
                    return {
                        ...user,
                        role: isAdmin?"Admin":"User"
                    };
                } else {
                    return user;
                }
            });

            userContext.setUsers(updatedUsers);
            props.setShowEditRoleModal(false);
        } catch (error) {
            userContext.setErrorMessage(error.message);
            userContext.setShowUserErrorModal(true);
            userContext.getUsers();

            props.setShowEditRoleModal(false);
        } 
    }


    return (
        <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/5 h-auto">
    
                <div className="mb-5">
                    <div className="flex justify-center mb-2">
                        <div className="w-full">
                            <p className="text-gray-600 dark:text-gray-400 text-center">{updateContext.id}</p>
                        </div>
                    </div>
                    <div className="flex justify-between mb-2">
                        <div>
                            <p className="text-sm font-medium text-gray-900 dark:text-gray-300">First Name:</p>
                        </div>
                        <div>
                            <p className="text-gray-600 dark:text-gray-400">{updateContext.firstName}</p>
                        </div>
                    </div>
                    <div className="flex justify-between mb-2">
                        <div>
                            <p className="text-sm font-medium text-gray-900 dark:text-gray-300">Last Name:</p>
                        </div>
                        <div>
                            <p className="text-gray-600 dark:text-gray-400">{updateContext.lastName}</p>
                        </div>
                    </div>
                    <div className="flex justify-between mb-2">
                        <div>
                            <p className="text-sm font-medium text-gray-900 dark:text-gray-300">Email:</p>
                        </div>
                        <div>
                            <p className="text-gray-600 dark:text-gray-400">{updateContext.email}</p>
                        </div>
                    </div>
                    
                    <div className="flex justify-between mb-2">
                        <div>
                            <p className="text-sm font-medium text-gray-900 dark:text-gray-300">Is an admin:</p>
                        </div>
                        <div>
                            <label className="flex items-center cursor-pointer">
                                <div className={`relative w-11 h-6 bg-gray-200 rounded-full ${isAdmin ? 'bg-blue-600' : 'bg-gray-700'}`}>
                                    <input type="checkbox" className="sr-only" checked={isAdmin} onChange={toggleIsAdmin} />
                                    <div className={`absolute left-0 top-0 w-6 h-6 bg-white border-2 rounded-full transition-transform ${isAdmin ? 'translate-x-full' : ''} transform`}></div>
                                </div>
                            </label>
                        </div>
                    </div>
                </div>
    
                <div className="flex justify-around">
                    <button onClick= {() => props.setShowEditRoleModal(false)}className="w-[45%] focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">Close</button>
                    <button type="submit" className="w-[45%] focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800">Update</button>
                </div>
            </form>
        </div>
    );

}
export default EditRoleModal
