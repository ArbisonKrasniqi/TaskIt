import { deleteData } from '../../../../Services/FetchService.jsx';
import React, {useContext} from 'react';
import { UserContext } from '../UserList.jsx';


const DeleteUserButton = (props) => {
    const userContext = useContext(UserContext);

    //Kur klikohet butoni, thirret ky funksion per te bere delete user
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
    
    return (
        <> 
            <button type="button" onClick={() => {HandleUserDelete(props.id)}} className="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900" >Delete</button>
        </>
    );
}

export default DeleteUserButton