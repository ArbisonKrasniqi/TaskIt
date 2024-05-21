import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import SignUpPage from './Pages/SignUpPage.jsx';
import Sidebar from '../src/Components/Side/Sidebar.jsx';
function App() {
  return (
   <>
 <Sidebar emri="Test" ></Sidebar> 
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signUp" element={<SignUpPage/>}/>
          <Route path="/dashboard" element={<Dashboard/>}/>
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
