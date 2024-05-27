import React, { useState } from 'react';
import UpdateUserModal from '../Modals/UpdateUserModal.jsx';
import EditInfoModal from '../Modals/EditInfoModal.jsx';
import EditPasswordModal from '../Modals/EditPasswordModal.jsx';
import EditRoleModal from '../Modals/EditRoleModal.jsx';
import EditButton from '../../Buttons/EditButton.jsx';

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
            <EditButton onClick={toggleModal} type="button" name="Edit" />
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