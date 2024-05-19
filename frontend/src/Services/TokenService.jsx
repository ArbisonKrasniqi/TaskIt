import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode'
import { postData } from './FetchService';
import{ useNavigate } from 'react-router-dom';

export function ValidateToken() {
    const refreshToken = getRefreshToken();
    if (refreshToken == null) {
        return false;
    }
    const accessToken = GetAccessToken();
    if (!accessToken) {
        return false;
    }

    const decodedToken = jwtDecode(accessToken);
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

export function ValidateAdmin() {
    const token = GetAccessToken();
    if (!token) {
        return false;
    }

    const decodedToken = jwtDecode(token);
    const isAdmin = decodedToken.Role === "Admin";
    if (isAdmin) {
        return true;
    }
    return false;
}

export function StoreTokens(accessToken, refreshToken) {
    const decodedToken = jwtDecode(accessToken);

    //Get expiry date
    const accessTokenExpiresIn = (decodedToken.exp * 1000 - Date.now()) / (1000 * 60 * 60 * 24);
    Cookies.set('accessToken', accessToken, { expires: accessTokenExpiresIn, secure: true });

    const refreshTokenExpiresIn = 15 / (60 * 24); // 15 minutes in days
    Cookies.set('refreshToken', refreshToken, {expires: refreshTokenExpiresIn, secure: true});

}

export function GetAccessToken() { 
    return Cookies.get('accessToken');   
}

export function getRefreshToken() {
    return Cookies.get('refreshToken');
}


//Ky funksion duhet te thirret sa here qe tokeni i userit eshte gati per t'u skaduar
//OSE nese nje api endpoint qe eshte i thirrur tregon se userit i ka skaduar tokeni.
export const refreshAccessToken = async () => {
    try {
        const refreshToken = getRefreshToken();
        const data = {
            refreshToken: refreshToken
        };

        const response = await postData("http://localhost:5157/backend/token/refreshToken", data);
        const newAccessToken = response.data.accessToken;
        const newRefreshToken = response.data.refreshToken;

        StoreTokens(newAccessToken, newRefreshToken);
        return true;
    } catch (error) {
        console.error(error.message);
        console.error("Your session has expired please log back in!");
        return false;
    }
}