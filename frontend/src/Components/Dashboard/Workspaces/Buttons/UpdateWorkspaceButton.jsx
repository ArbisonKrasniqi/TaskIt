import React, {useState} from 'react';
import UpdateWorkspaceModal from '../Modals/UpdateWorkspaceModal';

const UpdateWorkspaceButton = () => {

    const [showUpdateInfoModal, setShowUpdateInfoModal] = useState(false);

    const handleEditInfoClick = () => {
        setShowUpdateInfoModal(true);
    }

    return(
        <>
        <button onClick={handleEditInfoClick} type="button" className="focus:outline-none text-white bg-orange-400 hover:bg-orange-500 focus:ring-4 focus:ring-orange-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:focus:ring-orange-900">Edit</button>
        {showUpdateInfoModal && <UpdateWorkspaceModal setShowUpdateInfoModal={setShowUpdateInfoModal}/>}
        </>
    );
}
export default UpdateWorkspaceButton