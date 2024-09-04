import React, { useState } from 'react';
import UpdateListModal from '../Modals/UpdateListModal.jsx';
import CustomButton from '../../Buttons/CustomButton';

const UpdateListButton = () => {
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = (e) => {
        e.stopPropagation();
        setShowUpdateInfoModal(true);
    }

    return(
        <>
            <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange"/>
            {showUpdateInfoModal && <UpdateListModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    )
}

export default UpdateListButton