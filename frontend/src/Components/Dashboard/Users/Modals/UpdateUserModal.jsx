
import CustomButton from "../../Buttons/CustomButton";
const UpdateUserModal = (props) => {
    return (
        <>
            {props.showModal && (
                <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
                <div className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-72 h-auto">
                  <div className="flex flex-col gap-4 text-gray-500 items-center">
                        <CustomButton color='EditUserList' onClick={props.handleEditInfoClick} type="button" text="Edit User Info"/>
                        <CustomButton color='EditUserList' onClick={props.handleEditPasswordClick} type="button" text="Edit User Password"/>
                        <CustomButton color='EditUserList' onClick={props.handleEditRoleClick} type="button" text="Edit User Role"/> 
                      
                        {/*Nese shtypet butoni cancel atehere mbyllet modali kryesor*/}
                        <CustomButton onClick={props.toggleModal} type="button" text="Close" color="longRed"/>
                  </div>
                </div>
              </div>
            )}
        </>
    );
};

export default UpdateUserModal;
