import Navbar from '../Components/Navbar/Navbar'
import Sidebar from '../Components/Side/Sidebar';

const Main = () => {
    return (
        <div className="w-full">
            <Navbar/>
            <Sidebar emri="Test"/>
    </div>
    );
}

export default Main