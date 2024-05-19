import React, {useState} from 'react';
import UpdateWorkspaceModal from '../Modals/UpdateWorkspaceModal';
import EditButton from '../../Buttons/EditButton';

const UpdateWorkspaceButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <EditButton onClick={handleEditInfoClick} type="button" name="Edit" />
        {showUpdateInfoModal && <UpdateWorkspaceModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateWorkspaceButton