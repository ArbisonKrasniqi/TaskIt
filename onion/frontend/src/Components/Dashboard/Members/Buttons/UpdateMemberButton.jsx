import React, {useState} from 'react';
import UpdateMemberModal from '../Modals/UpdateMemberModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateMemberButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange" />
        {showUpdateInfoModal && <UpdateMemberModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateMemberButton