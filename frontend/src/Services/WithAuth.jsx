import React, { useEffect, useState } from 'react';
import { getAccessToken, getRefreshToken, refreshTokens } from './TokenService';
import { useNavigate } from 'react-router-dom';
import LoadingModal from '../Components/Modal/LoadingModal';

const WithAuth = (WrappedComponent) => {
  // The outer function is the HOC factory, which returns the actual component
  const AuthComponent = (props) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
      const checkAuth = async () => {
        let refreshToken = getRefreshToken();
        if (refreshToken && await refreshTokens()) {
          const accessToken = getAccessToken();
            if (accessToken) {
              setIsAuthenticated(true);
              console.info("VALIDATED");
            } else {
              console.info("NOT VALIDATED");
              navigate('/login');
            }
        } else {
          console.log("NOT LOGGED IN");
          navigate('/login');
        }
      }
      checkAuth();
    }, [navigate]);

    if (!isAuthenticated) {
      return <LoadingModal/>
    }

    return <WrappedComponent {...props} />;
  };

  return AuthComponent;
};

export default WithAuth;
