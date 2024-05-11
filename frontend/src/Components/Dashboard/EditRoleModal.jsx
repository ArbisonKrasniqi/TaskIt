import { UpdateContext } from './UserTable';
import { useContext } from 'react';

const EditRoleModal = (props) => {
    const updateContext = useContext(UpdateContext);
    return(
        <div>
            <button onClick={() => props.setShowEditRoleModal(false)}>Close</button>
        </div>
    )

}
export default EditRoleModal
