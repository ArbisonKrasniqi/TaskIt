import React, { useState } from 'react';
import UpdateChecklistModal from '../Modals/UpdateChecklistModal.jsx';
import CustomButton from '../../Buttons/CustomButton';

const UpdateChecklistButton = () => {
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return (
        <>
            <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange"/>
            {showUpdateInfoModal && <UpdateChecklistModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    )
}

export default UpdateChecklistButton;