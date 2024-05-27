import React, {useState} from 'react';
import UpdateWorkspaceModal from '../Modals/UpdateWorkspaceModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateWorkspaceButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange" />
        {showUpdateInfoModal && <UpdateWorkspaceModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateWorkspaceButton