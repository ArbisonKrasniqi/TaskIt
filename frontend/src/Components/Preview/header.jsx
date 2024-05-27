import React from 'react';
const Preview = () =>{
  
        
 return(
    <nav class=" dark:bg-gray-900 min-h-screen">
        <div class="flex items-center  mx-auto px-4 py-2.5 ">
            <h1 style={{textAlign: 'left', padding: 30, color: '#134946', fontSize: '40px', fontWeight: 'bold'}}>Taskit</h1>

            <div class="flex justify-start items-start mx-auto px-4 py-2.5">
                <ul class="flex flex-col font-medium p-4 mt-4 border rounded-lg  md:flex-row md:mt-0 md:text-sm  md:border-0  md:space-x-8 ">
                    <li>
                        <a href="#" class="block py-2 px-3 text-white rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0  md:p-0 dark:text-gray-400 md:dark:hover:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent">Resources</a>
                    </li>
                    <li>
                        <a href="#" class="block py-2 px-3 text-white rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0  md:p-0 dark:text-gray-400 md:dark:hover:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent">About Us</a>
                    </li>
                    <li>
                        <a href="#" class="block py-2 px-3 text-white rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0  md:p-0 dark:text-gray-400 md:dark:hover:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent">Contact Us</a>
                    </li>
                </ul>
            </div>

            <div className="justify-end">
                <button className="flex items-center justify-center w-full h-12 px-6 py-2 text-base font-medium text-center border border-transparent rounded-lg shadow-md" style={{backgroundColor: '#134946'}}>
                    <span className=" text-white">Login</span>
                </button>
            </div>
        </div>
        <div className="relative">
            <div className="absolute top-1/2 left-0 transform -translate-y-1/2 w-1/2 text-center pt-80 text-4xl px-20" >
                <h1 className="text-white">Taskit brings all your tasks, team and tools in one place</h1>
                <a className="flex items-center justify-center w-full h-12 px-6 py-2 text-base font-medium text-center text-white cursor-pointer text-2xl" style={{color: '#134946'}}> Sign up here!</a>
            </div>
            
        </div>
        
    </nav>

        
        

        

        
    );

}

export default Preview;