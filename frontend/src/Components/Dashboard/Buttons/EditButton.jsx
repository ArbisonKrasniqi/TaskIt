
const EditButton = ({onClick, type, name}) => {
    return(
        <>

 <button 
 onClick={onClick} 
 type={type} 
 className="focus:outline-none text-white bg-orange-400 hover:bg-orange-500 focus:ring-4 focus:ring-orange-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:focus:ring-orange-900">
   {name}
    </button>
        </>
    );
}
export default EditButton;