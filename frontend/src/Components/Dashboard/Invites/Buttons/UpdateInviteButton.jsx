import React, {useState} from 'react';
import UpdateInviteModal from '../Modals/UpdateInviteModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateInviteButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange" />
        {showUpdateInfoModal && <UpdateInviteModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateInviteButton