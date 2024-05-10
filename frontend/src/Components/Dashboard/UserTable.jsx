const UserTable = (props) => {
    return(
        <>
        <table>
        <thead>
            <tr>
                <th>#</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Date Created</th>
                <th>ID</th>
            </tr>
        </thead>
        <tbody>
            {props.users ? (props.users.map((item, index) => (
                <tr key={index}>
                    <td>{index}</td>
                    <td>{item.firstName}</td>
                    <td>{item.lastName}</td>
                    <td>{item.email}</td>
                    <td>{item.dateCreated}</td>
                    <td>{item.id}</td>
                </tr>
            ))): (
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            )}
        </tbody>
    </table>
        </>
    );
}

export default UserTable