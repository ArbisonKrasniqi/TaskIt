
import React, { useState } from 'react';
import UpdateUserModal from './UpdateUserModal.jsx';
import EditInfoModal from './EditInfoModal';
import EditPasswordModal from './EditPasswordModal';
import EditRoleModal from './EditRoleModal';

const UpdateUserButton = () => {

    //By default, modal kryesor per update eshte i mbyllur
    const [showModal, setShowModal] = useState(false);
    const [showEditInfoModal, setShowEditInfoModal] = useState(false);
    const [showEditPasswordModal, setShowEditPasswordModal] = useState(false);
    const [showEditRoleModal, setShowEditRoleModal] = useState(false);

    //Modali per perditesim te te dhenave bazike
    const handleEditInfoClick = () => {
        //Mbyll modalin fillestar
        setShowModal(false);

        //Shfaq modalin per perditesim
        setShowEditInfoModal(true);
    };

    //Modali per perditesim te password
    const handleEditPasswordClick = () => {
        //Mbyll modalin fillestar
        setShowModal(false);
        //Shfaq modalin per perditesim te passwordit
        setShowEditPasswordModal(true);
    };

    //Modali per perditesim te rolit
    const handleEditRoleClick = () => {
        //Mbyll modalin fillestar
        setShowModal(false);

        //Shfaq modalin per perditesim te rolit
        setShowEditRoleModal(true);
    };

    const toggleModal = () => {
        setShowModal(!showModal);
    }

    return(
        <>
            <button onClick={toggleModal} type="button" className="focus:outline-none text-white bg-orange-400 hover:bg-orange-500 focus:ring-4 focus:ring-orange-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:focus:ring-orange-900">Edit</button>
            {showModal && <UpdateUserModal handleEditInfoClick={handleEditInfoClick}
                                            handleEditPasswordClick={handleEditPasswordClick}
                                            handleEditRoleClick={handleEditRoleClick}
                                            toggleModal={toggleModal} showModal={showModal} setShowModal={setShowModal}/>}

            {showEditInfoModal && <EditInfoModal setShowEditInfoModal={setShowEditInfoModal}/>}

            {showEditPasswordModal && <EditPasswordModal setShowEditPasswordModal={setShowEditPasswordModal}/>}

            {showEditRoleModal && <EditRoleModal setShowEditRoleModal={setShowEditRoleModal}/>}
        </>
    );
}

export default UpdateUserButton