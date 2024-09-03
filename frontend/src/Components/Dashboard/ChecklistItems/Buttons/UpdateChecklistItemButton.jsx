import React, { useState } from 'react';
import UpdateChecklistItemModal from '../Modals/UpdateChecklistItemModal.jsx';
import CustomButton from '../../Buttons/CustomButton';


const UpdateChecklistItemButton = () => {
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);
    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return (
        <>
            <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange"/>
            {showUpdateInfoModal && <UpdateChecklistItemModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    )
}

export default UpdateChecklistItemButton;