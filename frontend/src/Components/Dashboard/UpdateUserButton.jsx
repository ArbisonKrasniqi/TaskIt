
import React, { useState } from 'react';
import UpdateUserModal from './UpdateUserModal.jsx';
import UserErrorModal from './UserErrorModal.jsx';


const UpdateUserButton = () => {

    //By default, modal kryesor per update eshte i mbyllur
    const [showModal, setShowModal] = useState(false);


    const toggleModal = () => {
        setShowModal(!showModal);
    }

    return(
        <>
            {/*Nese shtypet butoni i update, atehere kthe nga true ne false ose nga false ne true
                Ne kete menyre mund te shfaqet modal kryesor i cili mban modal-et e tjere */}
            <button onClick={toggleModal} type="button" className="focus:outline-none text-white bg-orange-400 hover:bg-orange-500 focus:ring-4 focus:ring-orange-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:focus:ring-orange-900">Edit</button>
            {showModal && <UpdateUserModal toggleModal={toggleModal} showModal={showModal}/>}
        </>
    );
}

export default UpdateUserButton