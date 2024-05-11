import React, {useContext} from 'react';
import { UpdateContext } from './UserTable';

const EditPasswordModal = (props) => {
    const updateContext = useContext(UpdateContext);
    return(
        <div>
            <button onClick={() => props.setShowEditPasswordModal(false)}>Close</button>
        </div>
    )

}
export default EditPasswordModal
