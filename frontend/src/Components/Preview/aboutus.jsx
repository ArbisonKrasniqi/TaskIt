import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';



const AboutUs = () => {
    const [menuOpen, setMenuOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    };

    const navigate = useNavigate();

    return(
        <div>
            <div className="flex items-center justify-between mx-auto px-4 py-2.5">
                <button  onClick={() => navigate(`/Preview`)} style={{ textAlign: 'left', padding: 30, color: 'dark-blue', fontSize: '40px', fontWeight: 'bold' }}>TaskIt</button>
                {/* Desktop Menu */}
                <div className="hidden md:flex md:space-x-8">
                    <button  onClick={() => navigate(`/AboutUs`)} className="py-2 px-3  hover:bg-black-100 rounded md:hover:bg-transparent md:border-0 dark:text-gray-400 md:dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent ">About Us</button>
                    <button  onClick={() => navigate(`/ContactUs`)} className="py-2 px-3  hover:bg-black-100 rounded md:hover:bg-transparent md:border-0 dark:text-gray-400 md:dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent ">Contact Us</button>
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
                        <li>
                            <button onClick={() => navigate(`/Login`)} className="w-full py-3 px-5 text-center text-white rounded-md" style={{ backgroundImage: 'linear-gradient(115deg,  #1a202c, #2d3748)'}}>
                                Login
                            </button>
                        </li>
                    </ul>
                </div>
            )}


            <div style={{ height: '200px', backgroundColor: '#2E3440', textAlign: 'center', paddingTop: '40px'}}>
                <h1 style={{fontSize: '2rem', fontWeight: 'bold', color: 'white'}}>About TaskIt</h1>
                <h1 style={{fontSize: '1rem', color: 'white'}}>What’s behind the boards.</h1>
            </div>

           
            <div style={{ paddingTop: '70px', textAlign: 'center', display: 'flex', flexDirection: 'column', alignItems: 'center'}}>
                <h1 style={{ fontSize: '2rem', fontWeight: 'bold', color: '#2E3440' }}>The way your team works is unique — so is TaskIt.</h1>
                <p style={{ maxWidth: '1000px', width: '100%', fontSize: '1.4rem', lineHeight: '1.6', margin: '0 auto'}}>
                    At TaskIt, we're passionate about helping individuals and teams stay organized, productive, and focused on what matters most. Our platform allows you to seamlessly manage your tasks, projects, 
                    and collaborations in one easy-to-use interface. Whether you're planning a personal project, coordinating with a team, or 
                    tracking progress on a large initiative, TaskIt provides the flexibility and efficiency you need to get things done.
                    We believe in empowering people to achieve their goals without the clutter of unnecessary complexity. That's why TaskIt is designed to be intuitive, customizable, and responsive to the way you work. 
                    With features like task boards, labels, lists, and real-time updates, our platform keeps you and your team on track no matter where you are.
                </p>
            </div>





        </div>
    );

}

export default AboutUs;