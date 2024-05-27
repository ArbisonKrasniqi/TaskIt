

const InputField = ({type, name, placeholder,value,onChange,error}) =>{

return(

    <div>
    <input 
    type={type} 
    name={name} 
    placeholder={placeholder}
    className="border border-gray-400 rounded-md px-3 py-2 mb-2 w-full"
    value={value}
    onChange={onChange}
    />
{error && <p className="text-red-500">{error}</p>}
</div>
);

};
export default InputField
