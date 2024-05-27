import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Preview from './Components/Preview/header.jsx';

import SignUpPage from './Pages/signUpPage.jsx';

function App() {
  return (
   <>
 {/*<Sidebar emri="Test" /> */} 
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signUp" element={<SignUpPage/>}/>
          <Route path="/dashboard" element={<Dashboard/>}/>
          <Route path="/preview" element={<Preview/>}/>
        </Routes>
      </BrowserRouter>

    </>
    
  );
}

export default App;
