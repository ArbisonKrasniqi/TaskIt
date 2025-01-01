import React, {useState} from 'react';
import UpdateStarredBoardModal from '../Modals/UpdateStarredBoardModal';
import CustomButton from '../../Buttons/CustomButton';

const UpdateStarredBoardButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange" />
        {showUpdateInfoModal && <UpdateStarredBoardModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateStarredBoardButton