import React,{useState} from 'react';
import InputField from '../Sign-up/InputField.jsx';
import ErrorMessage from '../Sign-up/ErrorMessage.jsx';
import Button from '../Sign-up/Button.jsx';
import { StoreTokens } from '../../Services/TokenService.jsx';
import { postData } from '../../Services/FetchService.jsx';
import { useNavigate } from 'react-router-dom';
import jwtDecode from 'jwt-decode';
// import { jwtDecode } from 'jwt-decode'; 
// po e boj koment se nuk pom bon me kllapa e qishtu pomm punon 

const LogInForm =  () =>{

    const navigate = useNavigate();
    const [formData,setFormData] = useState({
        email:'',
        password:'',
    });

    const[error, setError] =useState('');

    const handleInputChange = (e)=>{
        const {name, value} = e.target;

        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await postData('/backend/user/login', formData);
            StoreTokens(response.data.accessToken, response.data.refreshToken);
            const decodedToken = jwtDecode(response.data.accessToken);
            const role = decodedToken.Role;
            if (role === "Admin") {
                navigate(`/dashboard`);
            } else {
                navigate(`/main/workspaces`);
            }
        } catch (error) {
            console.error("Login failed please try again");
        }
        validateForm();

    };

    const validateForm = () =>{
        if(!formData.email){
            setError("Please enter your email");
            return false;
        }
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,4}$/;
        if(!emailRegex.test(formData.email)){
            setError('Please enter a valid email address.');
            return false;
        }
        if(!formData.password){
            setError("Please enter your password to continue")
            return false;
        }
        return true;
    };

    const handleSignUpClick = () => {
        navigate('/signup');
    }

    return(
        <div className='container mx-auto mt-20 h-[1000px]'>
            <div className='flex flex-col lg:flex-row w-full lg:w-8/12 h-screen lg:h-96 bg-white roundes=x; mx-auto shadow-lg overflow-hidden inset-x-20'>
                <div className='flex lg:flex-row justify-center items-center w-full lg:w-7/12 py-16 px-12 '>
                    <form onSubmit={handleSubmit} >
                    <div>
                    <h2 className='text-3xl mb-10 text-center font-sans font-bold'>Log in to continue</h2>
                    </div>
                        <div className='mt-5'>
                            <InputField
                            type="text"
                            name="email"
                            placeholder="Email"
                            value={formData.email}
                            onChange={handleInputChange}
                            />
                        </div>
                        <div className='mt-5'>
                            <InputField
                            type="password"
                            name="password"
                            placeholder="Password"
                            value={formData.password}
                            onChange={handleInputChange}
                            />
                        </div>
                        {error && <ErrorMessage content={error}/>}

                        <div className='mt-6 w-3/4 mx-auto'>
                            <Button type="submit" name="LogIn"/>
                        </div>

                    </form>


                </div>
             

                <div className='w-full lg:w-5/12 flex flex-col items-center justify-center p-12 bg-no-repeat bg-cover bg-center' style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
                    <h1 className='text-white text-3xl mb-3 font-sans font-bold'>Dont have an account?</h1>
                    <p className='text-white mt-5 font-sans' >Sign up below to start organizing your projects and collaborating with your team</p>
                    <button onClick={handleSignUpClick} className='bg-white text-gray-700 px-4 py-2 mt-20 border border-solid border-gray-700 rounded-md w-[50%]  hover:border hover:border-solid hover:border-black hover:text-black font-bold'>Sign Up</button>

                </div>

            </div>

        </div>

    );
}
export default LogInForm