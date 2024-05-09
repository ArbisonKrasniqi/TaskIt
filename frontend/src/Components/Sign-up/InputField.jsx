

const InputField = ({type, name, placeholder}) =>{

return(

    <input 
    type={type} 
    name={name} 
    placeholder={placeholder}
    className="border rounded-md px-3 py-2 mb-2"
    />


);

};
export default InputField
