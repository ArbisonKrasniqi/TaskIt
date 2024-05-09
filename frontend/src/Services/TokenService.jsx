import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode'

export function ValidateToken() {
    const token = GetToken();
    if (!token) {
        return false;
    }

    const decodedToken = jwtDecode(token);
    const expiryDate = decodedToken.expiryDate;
    
    //Divide by 1000 to get time in seconds
    const currentTime =  Math.floor(Date.now() / 1000); 
    if (expiryDate < currentTime) {
        //If invalid token return false
        return false;
    }

    //Return true if valid token
    return true;
}

export function StoreToken(token) {
    const decodedToken = jwtDecode(token);

    //Get expiry date
    const expiresIn = (decodedToken.exp * 1000 - Date.now()) / (1000 * 60 * 60 * 24);
    Cookies.set('taskItToken', token, { expires: expiresIn, secure: true });
}

export function GetToken() { 
    return Cookies.get('taskItToken');   
}