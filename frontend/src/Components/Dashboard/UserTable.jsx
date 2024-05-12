import DeleteUserButton from "./DeleteUserButton.jsx";
import UpdateUserButton from "./UpdateUserButton.jsx"
import React, {useContext, createContext} from 'react';
import { UserContext } from "./UserList";


export const UpdateContext = createContext();

const UserTable = () => {

    const userContext = useContext(UserContext);
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
                    <td className="px-6 py-4"><UpdateContext.Provider value={user}><UpdateUserButton/></UpdateContext.Provider><DeleteUserButton id={user.id}/></td>
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

export default UserTable