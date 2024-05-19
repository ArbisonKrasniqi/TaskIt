import UpdateUserButton from "./Buttons/UpdateUserButton.jsx"
import { deleteData } from '../../../Services/FetchService.jsx';
import React, {useContext, createContext} from 'react';
import { UserContext } from "./UsersList.jsx";
import CustomButton from "../Buttons/CustomButton.jsx";


export const UpdateContext = createContext();

const UsersTable = () => {

    const userContext = useContext(UserContext);

    const HandleUserDelete = (id) => {
        async function deleteUser() {
            try {
                const data = {
                    id: id
                };
                const response = await deleteData('http://localhost:5157/backend/user/adminDeleteUserById', data);
                console.log(response);

                //Ne rast se nuk eshte bere ndonje error
                //Perdoret userContext i cili mban te gjitha users
                //Dhe e largon nga lista e users, user-in qe sa eshte bere delete
                const updatedUsers = userContext.users.filter(user => user.id !== id);
                userContext.setUsers(updatedUsers);

            } catch (error) {
                //Nese ka pasur error

                //Vendos ErrorMessage
                
                userContext.setErrorMessage(error.message);

                //Beje UserErrorModal te shfaqet
                userContext.setShowUserErrorModal(true);

                //Beje fetch users per te perditesuar listen e usereve
                userContext.getUsers();
            }
        }
        deleteUser();
    }

    

    //Therritet konteksti nga userContext per te pasur qasje ne funksionet dhe variablat
    return(
        <div className="overflow-x-auto">
            <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
            <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                    {/* <th className="px-6 py-3">#</th> */}
                    <th className="px-6 py-3" >First Name</th>
                    <th className="px-6 py-3" >Last Name</th>
                    <th className="px-6 py-3" >Email</th>
                    <th className="px-6 py-3" >Date Created</th>
                    <th className="px-6 py-3" >ID</th>
                    <th className="px-6 py-3" >Role</th>
                    <th className="px-6 py-3" >Actions</th>
                </tr>
            </thead>
        <tbody>
            {/* Per secilin user ne listen e usereve nga konteksti, krijo nje row ku te gjitha atributet e userit shfaqen
                Po ashtu, krijo 2 butona special per secilin user, njera merr id per te bere delete
                Butoni tjeter krijon kontekst unik per secilin user per te edituar.
                Arsyea pse krijohet kontekst i ri, eshte sepse duam te editojme secilin user ndamas nga useret tjere.
                Pra per secilin user, krijohet nje kontekst i vecante i cili mban te dhenat per at user.
            */}
            {userContext.users ? (userContext.users.map((user, index) => (
                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    {/* <td className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">{index}</td> */}
                    <td className="px-6 py-4">{user.firstName}</td>
                    <td className="px-6 py-4">{user.lastName}</td>
                    <td className="px-6 py-4">{user.email}</td>
                    <td className="px-6 py-4">{user.dateCreated}</td>
                    <td className="px-6 py-4">{user.id}</td>
                    <td className="px-6 py-4">{user.role}</td>
                    <td className="px-6 py-4"><UpdateContext.Provider value={user}><UpdateUserButton/></UpdateContext.Provider><CustomButton onClick={ ()=> {HandleUserDelete(user.id)}} type="button" text="Delete" color="red"/></td>
                </tr>
            ))): (
                <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    {/* <td className="px-6 py-4"></td> */}
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                </tr>
            )}
        </tbody>
    </table>
    </div>
    );
}

export default UsersTable