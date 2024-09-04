import React, { useState } from 'react';
import UpdateTaskModal from '../Modals/UpdateTaskModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateTaskButton = () => {
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = (e) => {
        setShowUpdateInfoModal(true);
        e.stopPropagation();
    }

    return(
        <>
            <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange"/>
            {showUpdateInfoModal && <UpdateTaskModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    )
}

export default UpdateTaskButton