import React, { useState } from 'react';
import mainimg from '../Preview/main.png';
import { useNavigate } from 'react-router-dom';

const Preview = () => {
  const [menuOpen, setMenuOpen] = useState(false);

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  const navigate = useNavigate();

  return (
    <nav className="min-h-screen flex flex-col">
      {/* Header */}
      <div className="flex items-center mx-auto px-4 py-2.5 w-full justify-between">
                <div>
                <button  onClick={() => navigate(`/Preview`)} style={{ textAlign: 'left', padding: 30, color: 'dark-blue', fontSize: '40px', fontWeight: 'bold' }}>TaskIt</button>
                </div>
                {/* Desktop Menu */}
                <div className="hidden md:flex md:space-x-8">
                    <button  onClick={() => navigate(`/AboutUs`)} className="py-2 px-3 hover:bg-black-100 rounded dark:text-gray-400 dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white">About Us</button>
                    <button  onClick={() => navigate(`/ContactUs`)} className="py-2 px-3 hover:bg-black-100 rounded dark:text-gray-400 dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white">Contact Us</button>
                    <button onClick={() => navigate('/PrivacyPolicy')} className="py-2 px-3 hover:bg-black-100 rounded dark:text-gray-400 dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white">Privacy Policy</button>
                </div>

   

                {/* Desktop Login Button */}
                <div className="hidden md:flex">
                    <button onClick={() => navigate(`/Login`)} className="flex items-center justify-center px-5 py-3 rounded-md text-base font-medium text-center border border-transparent shadow-md" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
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
                <div className="md:hidden text-white" style={{ backgroundImage: 'linear-gradient(150deg, #2E3440, #414B5C)'}} >
                    <ul className="flex flex-col items-start p-4 space-y-4 rounded-lg">
                        <li><button  onClick={() => navigate(`/AboutUs`)} className="text-gray-400">About Us</button></li>
                        <li><button onClick={() => navigate('/ContactUs')} className="text-gray-400">Contact Us</button></li>
                        <li><button onClick={() => navigate('/PrivacyPolicy')} className="text-gray-400">Privacy Policy</button></li>
                        <li>
                            <button onClick={() => navigate(`/Login`)} className="w-full py-3 px-5 text-center text-white rounded-md" style={{ backgroundImage: 'linear-gradient(115deg,  #1a202c, #2d3748)'}}>
                                Login
                            </button>
                        </li>
                    </ul>
                </div>
            )}

      {/* Main Content */}
      <div className="flex-grow flex flex-col md:flex-row items-center justify-between px-4 py-10 md:py-0">
        {/* Text Section */}
        <div className="w-full md:w-1/2 flex flex-col justify-start space-y-4">
          <h1 className="text-black text-3xl md:text-4xl lg:text-5xl">
            Taskit brings all your tasks, team, and tools in one place
          </h1>
          <h4 className="text-gray-700 text-lg md:text-xl lg:text-2xl">
            Keep everything in the same place - even if your team isn’t.
          </h4>
          <div className="mt-8">
            <button
              onClick={() => navigate('/signup')}
              className="px-6 py-3 bg-gradient-to-r from-gray-900 to-gray-700 text-white rounded-lg"
            >
              Sign Up here!
            </button>
          </div>
        </div>

        {/* Image Section */}
        <div className="w-full md:w-1/2 flex justify-end md:justify-center mt-10 md:mt-0">
          <img src={mainimg} alt="Description of the image" className="w-full max-w-lg md:max-w-none" />
        </div>
      </div>
    </nav>
  );
};

export default Preview;
