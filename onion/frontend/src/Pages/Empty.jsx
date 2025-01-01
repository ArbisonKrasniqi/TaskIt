import { useNavigate } from "react-router-dom";
import { getAccessToken, getRefreshToken, refreshTokens, validateAdmin } from "../Services/TokenService";
import { useEffect, useState } from "react";
import LoadingModal from "../Components/Modal/LoadingModal";
import { jwtDecode } from "jwt-decode";

const Empty = () => {
    const navigate = useNavigate()
    
    useEffect(() => {
    const checkAuth = async () => {
        let refreshToken = getRefreshToken();
        if (refreshToken && await refreshTokens()) {
            const accessToken = getAccessToken();
            if (accessToken) {
                if (validateAdmin()) {
                    navigate('/dashboard');
                } else {
                    navigate('/main/workspaces')
                }
            } else {
                navigate('/preview')
            }
        } else {
            navigate('/preview');
        }
    }
    checkAuth();
    }, []);
    
    return <LoadingModal/>
}
export default Empty;