import { deleteData } from '../../../../Services/FetchService.jsx';
import React, {useContext} from 'react';
import { UserContext } from '../UsersList.jsx';
import DeleteButton from '../../Buttons/DeleteButton.jsx';


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
            <DeleteButton onClick={() => {HandleUserDelete(props.id)}} type="button" name="Delete"/>
            
        </>
    );
}

export default DeleteUserButton