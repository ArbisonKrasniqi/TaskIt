
const Button = ({type, name}) => {
    return (
        <button
        type={type}
        className="bg-blue-500 text-white px-4 py-2 rounded-md w-full"
        
        >
        {name}
        </button>
    );

};
export default Button;