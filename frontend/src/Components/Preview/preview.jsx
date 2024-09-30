// import React from 'react';
import React, { useState } from 'react';
import mainimg from '../Preview/main.png';
import { useNavigate } from 'react-router-dom';
// import AboutUs from '../Preview/aboutus.jsx';

const Preview = () =>{

  const [menuOpen, setMenuOpen] = useState(false);

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  const navigate = useNavigate();

  return(

        
    <nav className="min-h-screen">
    <div className="flex items-center justify-between mx-auto px-4 py-2.5">
      <h1 style={{ textAlign: 'left', padding: 30, color: 'dark-blue', fontSize: '40px', fontWeight: 'bold' }}>TaskIt</h1>


      <div className="hidden md:flex md:space-x-8">
        <button onClick={() => navigate(`/AboutUs`)} className="py-2 px-3 rounded text-gray-400 hover:bg-gray-700 hover:text-white">About Us</button>
        <button onClick={() => navigate('/ContactUs')} className="py-2 px-3 rounded text-gray-400 hover:bg-gray-700 hover:text-white">Contact Us</button>
        <button onClick={() => navigate('/PrivacyPolicy')} className="py-2 px-3 rounded text-gray-400 hover:bg-gray-700 hover:text-white">Privacy Policy</button>
      </div>

      <div className="hidden md:flex">
        <button onClick={() => navigate('/Login')} className="flex items-center justify-center px-5 py-3 rounded-md text-base font-medium text-center border border-transparent shadow-md" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
          <span className="text-white">Login</span>
        </button>
      </div>

      {/* Mobile Menu Button */}
      <div className="md:hidden">
        <button onClick={toggleMenu} className="text-black">
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/1000/svg">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"></path>
          </svg>
        </button>
      </div>
    </div>

    {/* Mobile Dropdown Menu */}
    {menuOpen && (
      <div className="md:hidden  bg-gray-800 text-white">
        <ul className="flex flex-col items-start p-4 space-y-4 rounded-lg">
          <li><button  onClick={() => navigate(`/AboutUs`)} className="text-gray-400 ">About Us</button></li>
          <li><button onClick={() => navigate('/ContactUs')} className="text-gray-400">Contact Us</button></li>
          <li><button onClick={() => navigate('/PrivacyPolicy')} className="text-gray-400">Privacy Policy</button></li>
          <li>
            <button onClick={() => navigate('/Login')} className="w-full py-3 px-5 text-center text-white rounded-md" style={{ backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
              Login
            </button>
          </li>
        </ul>
      </div>
    )}

 
      <div className="relative mt-5s top-1/2 left-0 transform -translate-y-1/2 w-full md:w-1/2 text-center pt-40 md:pt-80 text-2xl md:text-4xl lg:text-5xl px-6 md:px-20">
        <h1 className="text-black text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl">
          Taskit brings all your tasks, team, and tools in one place
        </h1>
        <h4 className="text-sm sm:text-base md:text-lg lg:text-xl xl:text-2xl mt-4">
          Keep everything in the same place - even if your team isn’t.
        </h4>
        <div className="mt-8 md:mt-12">
          <button onClick={() => navigate('/signup')} className="flex items-center justify-center text-base h-12 px-4 py-2 rounded-md w-[60%] font-medium text-center border border-transparent rounded-lg" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)', margin: '0 auto'}}>
            <span className=" text-white">Sign Up here!</span>
          </button>
        </div>
               
      </div>

    <div className="absolute bottom-0 right-0 p-10">
        <img src={mainimg} alt="Description of the image" style={{ width: '750px', height: '300px' }}/>
    </div>
    
  </nav>

     
  );
   
}

export default Preview;




