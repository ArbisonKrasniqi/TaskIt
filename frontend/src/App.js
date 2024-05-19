import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Main from './Pages/Main.jsx'
import SignUpPage from './Pages/signUpPage.jsx';

function App() {
  return (
   <>
 {/*<Sidebar emri="Test" /> */} 
      <BrowserRouter>
        <Routes>
          <Route path="/main" element={<Main/>}/>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signUp" element={<SignUpPage/>}/>
          <Route path="/dashboard" element={<Dashboard/>}/>
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
