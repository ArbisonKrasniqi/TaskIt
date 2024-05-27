import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Login from './Pages/loginPage.jsx';
import SignUp from './Pages/signUpPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Preview from './Components/Preview/header.jsx';


function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signUp" element={<SignUp/>}/>
          <Route path="/dashboard" element={<Dashboard/>}/>
          <Route path="/preview" element={<Preview/>}/>
        </Routes>
      </BrowserRouter>

    </>
    
  );
}

export default App;
