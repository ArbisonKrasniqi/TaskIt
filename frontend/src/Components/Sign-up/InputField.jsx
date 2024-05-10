

const InputField = ({type, name, placeholder}) =>{

return(

    <input 
    type={type} 
    name={name} 
    placeholder={placeholder}
    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"

    />


);

};
export default InputField
