import React, { useState } from 'react';
import EditInfoModal from './EditInfoModal';
import EditPasswordModal from './EditPasswordModal';
import EditRoleModal from './EditRoleModal';

const UpdateUserModal = (props) => {

    //MainModal eshte UpdateUserModal
    const [showMainModal, setShowMainModal] = useState(props.showModal);

    //Te gjithe modalet tjere jane false (te mbyllur)
    const [showEditInfoModal, setShowEditInfoModal] = useState(false);
    const [showEditPasswordModal, setShowEditPasswordModal] = useState(false);
    const [showEditRoleModal, setShowEditRoleModal] = useState(false);

    //Modali per perditesim te te dhenave bazike
    const handleEditInfoClick = () => {
        //Mbyll modalin fillestar
        setShowMainModal(false);

        //Shfaq modalin per perditesim
        setShowEditInfoModal(true);
    };

    //Modali per perditesim te password
    const handleEditPasswordClick = () => {
        //Mbyll modalin fillestar
        setShowMainModal(false);

        //Shfaq modalin per perditesim te passwordit
        setShowEditPasswordModal(true);
    };

    //Modali per perditesim te rolit
    const handleEditRoleClick = () => {
        //Mbyll modalin fillestar
        setShowMainModal(false);

        //Shfaq modalin per perditesim te rolit
        setShowEditRoleModal(true);
    };

    return (
        <div>
            {/* Main modal */}
            {showMainModal && (
                <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
                <div className="bg-white dark:bg-gray-700 dark:text-gray-400 p-8 rounded-lg shadow-md w-72 h-auto">
                  <div className="flex flex-col gap-4 text-gray-500 items-center">
                        <button className="w-[90%] py-4 me-1 mb-1 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700" onClick={handleEditInfoClick}>Edit User Info</button>
                        <button className="w-[90%] py-4 me-1 mb-1 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700" onClick={handleEditPasswordClick}>Edit User Password</button>
                        <button className="w-[90%] py-4 me-1 mb-1 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700" onClick={handleEditRoleClick}>Edit User Role</button>
                        {/*Nese shtypet butoni cancel atehere mbyllet modali kryesor*/}
                        <button onClick={props.toggleModal} className="w-[50%] focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">Close</button>
                  </div>
                </div>
              </div>
            )}

            {/* Ketu shfaqen modalet te dyte*/}
            {showEditInfoModal && <EditInfoModal setShowEditInfoModal={setShowEditInfoModal}/>}

            {showEditPasswordModal && <EditPasswordModal setShowEditPasswordModal={setShowEditPasswordModal}/>}

            {showEditRoleModal && <EditRoleModal setShowEditRoleModal={setShowEditRoleModal}/>}
        </div>
    );
};

export default UpdateUserModal;
