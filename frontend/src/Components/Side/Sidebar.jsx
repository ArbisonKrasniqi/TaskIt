import { useState } from "react";
import { SlArrowLeft } from "react-icons/sl";
import { CiViewBoard } from "react-icons/ci";
import { IoPersonOutline } from "react-icons/io5";
import { IoIosSettings } from "react-icons/io";
import { PiTable } from "react-icons/pi";
import { LuCalendarDays } from "react-icons/lu";
import { FaPlus } from "react-icons/fa6";
import FetchService from "../../Services/FetchService.jsx";

const Sidebar = (props) =>{
const[open, setOpen] = useState(true);
const Menus = [
    {title: "Boards", tag: "CiViewBoard"},
    {title: "Members", tag: "IoPersonOutline"},
    {title: "Workspace settings", tag: "IoIosSettings"},
]
const tagComponents = {
    CiViewBoard: <CiViewBoard/>,
    IoPersonOutline: <IoPersonOutline/>,
    IoIosSettings: <IoIosSettings/>,
    PiTable: <PiTable/>,
    LuCalendarDays: <LuCalendarDays/>,
    };
const WorkspaceViews = [
    {title: "Table", tag: "PiTable"},
    {title: "Calendar", tag: "LuCalendarDays"},
]


    return(
        <div className="flex">
            <div className={`${open ? 'w-72' : 'w-8'} duration-100 h-screen p-5 pt-8 relative`} style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
            
        

        <SlArrowLeft className={`absolute cursor pointer rounded-full h-6 px-2 right-1 w-8 mt-2 border-2 text-center border-gray-700 bg-gray-800 text-gray-400 font-extrabold ${!open && "rotate-180 -right-3"}`}
           onClick={()=>setOpen(!open)}
            ></SlArrowLeft>

           <div className="flex gap-x-3 items-center">
           <button className={`w-10 h-10 text-black bg-gradient-to-r from-blue-400 to-indigo-500 font-bold text-xl rounded-lg text-center px-3 items-center dark:bg-blue-600 dark:focus:ring-blue-800 duration-200 ${!open && "scale-0"}`}>{props.emri.charAt(0)}</button>
           <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-xl duration-200 ${!open && "scale-0"}`}>{props.emri}</h1> 
           </div> 
           
           <hr className="w-full border-gray-400 mt-3"></hr>

            <ul className="pt-4">
            {Menus.map((menu,index)=>(
                <li key={index} className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500 ${!open && "scale-0"}`}>
                {tagComponents[menu.tag]}
                <span>{menu.title}</span>
                </li>
            ))}

            </ul>
           <br></br>
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Workspace views</h1>
           
            <ul>
            {WorkspaceViews.map((views,index)=>(
                <li key={index} className={`text-gray-400 text-l font-semibold flex items-center gap-x-4 cursor-pointer p-2 hover:bg-gray-500 ${!open && "scale-0"}`}>
                {tagComponents[views.tag]}
                <span>{views.title}</span>
                </li>
            ))}

            </ul>
            <br/>

            <div className="flex justify-between">
            <h1 className={`w-full origin-left font-sans text-gray-400 font-bold text-l ${!open && "scale-0"}`}>Your boards</h1>
            <FaPlus className="text-gray-400 cursor-pointer"/>
            </div>


            </div>
            
          
        </div>


    );

}

export default Sidebar
