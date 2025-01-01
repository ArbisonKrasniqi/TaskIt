import { useState } from "react";
import CustomButton from "../../Buttons/CustomButton";
import UpdateLabelModal from "../Modals/UpdateLabelModal";

const UpdateLabelButton = () => {
    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = (e) => {
        setShowUpdateInfoModal(true);
        e.stopPropagation();
    }

    return (
        <>
            <CustomButton onClick={handleEditInfoClick} type="button" text="Edit" color="orange"/>
            {showUpdateInfoModal && <UpdateLabelModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    )
}

export default UpdateLabelButton;