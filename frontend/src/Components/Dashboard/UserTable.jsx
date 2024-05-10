import DeleteUserButton from "./DeleteUserButton";

const UserTable = (props) => {
    return(
        <div className="overflow-x-auto">
        <table className="w-full text-sm text-left rtl:text-right text-gray-300 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
                {/* <th className="px-6 py-3">#</th> */}
                <th className="px-6 py-3" >First Name</th>
                <th className="px-6 py-3" >Last Name</th>
                <th className="px-6 py-3" >Email</th>
                <th className="px-6 py-3" >Date Created</th>
                <th className="px-6 py-3" >ID</th>
                <th className="px-6 py-3" >Actions</th>
            </tr>
        </thead>
        <tbody>
            {props.users ? (props.users.map((item, index) => (
                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    {/* <td className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white">{index}</td> */}
                    <td className="px-6 py-4">{item.firstName}</td>
                    <td className="px-6 py-4">{item.lastName}</td>
                    <td className="px-6 py-4">{item.email}</td>
                    <td className="px-6 py-4">{item.dateCreated}</td>
                    <td className="px-6 py-4">{item.id}</td>
                    <td className="px-6 py-4"><DeleteUserButton id={item.id}/> Update</td>
                </tr>
            ))): (
                <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    {/* <td className="px-6 py-4"></td> */}
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                </tr>
            )}
        </tbody>
    </table>
    </div>
    );
}

export default UserTable