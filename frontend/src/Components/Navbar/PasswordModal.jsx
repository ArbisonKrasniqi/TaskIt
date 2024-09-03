import { useContext,useState } from "react";
import { putData } from "../../Services/FetchService";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import MessageModal from "../ContentFromSide/MessageModal";
const PasswordModal = ({isOpen, onClose, user})=>{
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [modalMessage, setModalMessage] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [errorMessage, setErrorMessage]=useState('');

    const handleSubmit = async(e)=>{
        e.preventDefault();
        setErrorMessage('');

        if(newPassword!==confirmPassword){
            setErrorMessage('New passwords do not match!');
            return;
        }
        // if(newPassword === oldPassword){
        //     setError("You should use a different password ")
        // }
        try{
          
            const updatePasswordData = {
                id: user.id,
                password: newPassword
            };
            console.log(updatePasswordData);
            const response = await putData("http://localhost:5157/backend/user/adminUpdatePassword", updatePasswordData);
            setOldPassword('');
            setNewPassword('');
            setConfirmPassword('');
            setModalMessage('Password updated successfully');
            setIsModalOpen(true);
        }catch (error) {
            console.error("Error changing password ",error.message)
            setModalMessage('Failed to update password');
            setIsModalOpen(true);
        }
    };

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-5 rounded-lg shadow-lg max-w-md w-full">
                <h2 className="text-lg font-semibold mb-4">Update Password</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm">Old Password:</label>
                        <input
                            type="password"
                            value={oldPassword}
                            onChange={(e) => setOldPassword(e.target.value)}
                            className="mt-1 p-2 w-full bg-gray-200 rounded"
                        />
                    </div>
                    <div>
                        <label className="block text-sm">New Password:</label>
                        <input
                            type="password"
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                            className="mt-1 p-2 w-full bg-gray-200 rounded"
                        />
                    </div>
                    <div>
                        <label className="block text-sm">Confirm New Password:</label>
                        <input
                            type="password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            className="mt-1 p-2 w-full bg-gray-200 rounded"
                        />
                    </div>
                    {errorMessage && <p className="text-red-500">{errorMessage}</p>}
                   <div className="flex flex-row gap-10 justify-center">
                   <button
                        type="submit"
                        className="p-2 bg-blue-500 text-white rounded"
                    >
                        Update Password
                    </button>
                    <button
                        type="button"
                        onClick={onClose}
                        className="p-2 bg-gray-500 text-white rounded"
                    >
                        Close
                    </button>
                   </div>
                </form>
            </div>

            <MessageModal
                message={modalMessage}
                isOpen={isModalOpen}
                duration={3000} // Modal shfaqet për 3 sekonda
                onClose={() => setIsModalOpen(false)}
            />
        </div>
    );
};
export default PasswordModal;