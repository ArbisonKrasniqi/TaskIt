import CloseButton from "../../Buttons/CloseButton";
import EditUserButton from "../../Buttons/EditUserButton";
const UpdateUserModal = (props) => {
    return (
        <>
            {props.showModal && (
                <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
                <div className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-72 h-auto">
                  <div className="flex flex-col gap-4 text-gray-500 items-center">
                        <EditUserButton onClick={props.handleEditInfoClick} type="button" name="Edit User Info" />
                        <EditUserButton onClick={props.handleEditPasswordClick} type="button" name="Edit User Password" />
                        <EditUserButton onClick={props.handleEditRoleClick} type="button" name="Edit User Role" />
                       
                        {/*Nese shtypet butoni cancel atehere mbyllet modali kryesor*/}
                        <CloseButton onClick={props.toggleModal} type="button" name="Close" />
                  </div>
                </div>
              </div>
            )}
        </>
    );
};

export default UpdateUserModal;
