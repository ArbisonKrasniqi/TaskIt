import Cookies from 'js-cookie';
// import jwtDecode from 'jwt-decode';
import { jwtDecode } from 'jwt-decode'
import { postData } from './FetchService';


export function validateAdmin() {
    const token = getAccessToken();
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

    const refreshTokenExpiresIn = 30 // Days of validity
    Cookies.set('refreshToken', refreshToken, {expires: refreshTokenExpiresIn, secure: true});
}

export function getAccessToken() { 
    return Cookies.get('accessToken');   
}

export function getRefreshToken() {
    return Cookies.get('refreshToken');
}


//Ky funksion duhet te thirret sa here qe tokeni i userit eshte gati per t'u skaduar
//OSE nese nje api endpoint qe eshte i thirrur tregon se userit i ka skaduar tokeni.
export const refreshTokens = async () => {
    try {
        const refreshToken = getRefreshToken();
        const data = {
            refreshToken: refreshToken
        };

        const response = await postData("/backend/token/refreshToken", data);
        const newAccessToken = response.data.accessToken;
        const newRefreshToken = response.data.refreshToken;

        StoreTokens(newAccessToken, newRefreshToken);
        return true;
    } catch (error) {
        return false;
    }
}

export const isTokenExpiring = (expiryTime) => {
    const currentTime = Math.floor((Date.now()) / 1000);
    const timeUntilExpiry = expiryTime - currentTime;
    return timeUntilExpiry < 10;
}

// export const checkAndRefreshToken = async () => {
//     var accessToken = getAccessToken();
//     var refreshToken = getRefreshToken();

//     if (!refreshToken) {
//         console.info("You are not logged in.");
//         return false;
//     } else if (accessToken && refreshToken) {
//         const decodedToken = jwtDecode(accessToken);
//         const expiryTime = decodedToken.exp;    
//         if(isTokenExpiring(expiryTime)) {
//             var refreshResult = await refreshTokens();
//             if (refreshResult) {
//                 console.info("Tokens refreshed before expiry");
//                 accessToken = getAccessToken();
//                 refreshToken = getRefreshToken();  
//                 return true;
//             } else {
//                 console.info("You have been logged out.");
//                 return false;
//             }
//         }
//     } else if (!accessToken && refreshToken) {
//         var refreshResult = await refreshTokens();
//         if (refreshResult) {
//             console.info("Tokens refreshed after expiry");
//             accessToken = getAccessToken();
//             refreshToken = getRefreshToken();
//             return true;
//         } else {
//             console.info("Couldnt refresh tokens");
//             return false;
//         }
//     } else {
//         console.info("There has been an error please log in again!");
//         return false;
//     }
// }

export const checkAndRefreshToken = async () => {
    try {
        var accessToken = getAccessToken();
        var refreshToken = getRefreshToken();
        if (!refreshToken) {
            console.log("You are not logged in.");
            return false;
        }

        if (accessToken) {
            // Decode the token to verify its structure
            const decodedToken = jwtDecode(accessToken);
            const expiryTime = decodedToken.exp;

            if (isTokenExpiring(expiryTime)) {
                var refreshResult = await refreshTokens();
                if (refreshResult) {
                    //console.log("Tokens refreshed before expiry");
                    accessToken = getAccessToken();
                    refreshToken = getRefreshToken();
                    return true;
                } else {
                    console.log("You have been logged out.");
                    return false;
                }
            }
        } else if (!accessToken && refreshToken) {
            var refreshResult = await refreshTokens();
            if (refreshResult) {
                //console.log("Tokens refreshed after expiry");
                accessToken = getAccessToken();
                refreshToken = getRefreshToken();
                return true;
            } else {
                console.log("Couldn't refresh tokens");
                return false;
            }
        } else {
            console.log("There has been an error. Please log in again!");
            return false;
        }
    } catch (error) {
        return false;
    }
};