import React,{ useState } from "react";
import UpdateBackgroundModal from "../Modals/UpdateBackgroundModal";
import CustomButton from "../../Buttons/CustomButton";

const UpdateBackgroundButton = () => {
    const [showUpdateBackgroundInfoModal, setShowUpdateBackgroundInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateBackgroundInfoModal(true);
    }

    return(
        <>
            <CustomButton onCLick={handleEditInfoClick} type='button' text='Edit' color='orange'/>
            {showUpdateBackgroundInfoModal && <UpdateBackgroundModal setShowUpdateBackgroundInfoModal={setShowUpdateBackgroundInfoModal} />}
        </>
    );
}

export default UpdateBackgroundButton;