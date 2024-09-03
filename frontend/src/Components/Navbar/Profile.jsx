import { useContext, useEffect, useState } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { getDataWithId, putData } from "../../Services/FetchService";
import MessageModal from "../ContentFromSide/MessageModal";
import PasswordModal from "./PasswordModal";
const Profile = () =>{

const {userId}= useContext(WorkspaceContext);
const [userData, setUserData] = useState({
    id: '',
    firstName: '',
    lastName: '',
    email: '',
});
const [userPassword, setUserPassword] = useState('');
const [modalMessage, setModalMessage] = useState('');
const [isModalOpen, setIsModalOpen] = useState(false);
const [isPasswordModalOpen, setIsPasswordModalOpen] = useState(false);
const getUser = async ()=>{
    try{
        const responseUser = await getDataWithId('http://localhost:5157/backend/user/adminUserID?userId', userId);
       setUserData(responseUser.data);
       setUserPassword(responseUser.data.password);
       console.log("PASSWORDI: ",responseUser.data);
    }catch(error){
        console.error("Error fetching user data ", error.response.data);
    }
};

useEffect(()=>{
    getUser();
}, [userId]);

const handleInputChange = (e) =>{
    const {name, value} = e.target;
    setUserData({
        ...userData,
        [name]: value
    });
};

const handleSubmit = async (e)=>{
    e.preventDefault();
    try{
        const updatedUserData = {
            id: userId,
            ...userData
            
        };
        //console.log(updatedUserData);
        const response = await putData("http://localhost:5157/backend/user/adminUpdateUser", updatedUserData);
        setModalMessage('User successfully updated');
        setIsModalOpen(true);
    }catch (error) {
    console.error("Error updating user data:", error);
    setModalMessage('Failed to update user');
    setIsModalOpen(true);
}
};
return (
    <div className="min-h-screen h-full flex flex-col items-center justify-center" style={{ backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)' }}>
      <div className="bg-gray-800 p-8 rounded-lg shadow-lg w-full max-w-md">
      <h1 className="font-semibold font-sans text-gray-400 text-center text-2xl mb-5">Edit Your Profile Info</h1>
      <form onSubmit={handleSubmit} className="text-gray-400">
        <div className="mb-4">
          <label className="block text-lg">First Name:</label>
          <input
            type="text"
            name="firstName"
            value={userData.firstName}
            onChange={handleInputChange}
            className="mt-1 p-2 w-full bg-gray-700 text-white rounded"
          />
        </div>
        <div className="mb-4">
          <label className="block text-lg">Last Name:</label>
          <input
            type="text"
            name="lastName"
            value={userData.lastName}
            onChange={handleInputChange}
            className="mt-1 p-2 w-full bg-gray-700 text-white rounded"
          />
        </div>
        <div className="mb-4">
          <label className="block text-lg">Email:</label>
          <input
            type="email"
            name="email"
            value={userData.email}
            onChange={handleInputChange}
            className="mt-1 p-2 w-full bg-gray-700 text-white rounded"
          />
        </div>
    
        <button type="submit" 
        className="mt-4 p-2 bg-blue-500 text-white rounded">Update Profile</button>
      </form>

      </div>
        

        <button className="mt-10 p-2 bg-blue-500 text-white rounded shadow-lg w-full max-w-md"
          onClick={() => setIsPasswordModalOpen(true)}>Update Password</button>

<MessageModal
                message={modalMessage}
                isOpen={isModalOpen}
                duration={2000}
                onClose={() => setIsModalOpen(false)}
            />


<PasswordModal
                isOpen={isPasswordModalOpen}
                onClose={() => setIsPasswordModalOpen(false)}
                user = {userData}
            />
    </div>
  );
};
export default Profile;