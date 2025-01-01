import React, { useState } from 'react';
import UpdateBoardModal from '../Modals/UpdateBoardModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateBoardButton = () => {

    const [showUpdateBoardInfoModal, setShowUpdateBoardInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateBoardInfoModal(true);
    }

    return(
        <>
        <CustomButton onCLick={handleEditInfoClick} type='button' text='Edit' color='orange'/>
        {showUpdateBoardInfoModal && <UpdateBoardModal setShowUpdateBoardInfoModal={setShowUpdateBoardInfoModal}/>}
        </>
    );

}

export default UpdateBoardButton;