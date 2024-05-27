
const Button = ({type, name}) => {
    return (
        <button
        type={type}
        className="bg-gray-800 font-bold text-white px-4 py-2 rounded-md w-[60%] hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out"
        
        >
        {name}
        </button>
    );

};
export default Button;