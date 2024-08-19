import React, { useEffect, useState } from 'react';
import { getAccessToken, getRefreshToken, refreshTokens } from './TokenService';
import { useNavigate } from 'react-router-dom';

const WithAuth = (WrappedComponent) => {
  // The outer function is the HOC factory, which returns the actual component
  const AuthComponent = (props) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
      const checkAuth = async () => {
        let accessToken = getAccessToken();
        if (accessToken) {
          setIsAuthenticated(true);
        } else {
          if (getRefreshToken() && await refreshTokens()) {
            accessToken = getAccessToken();
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
      };

      checkAuth();
    }, [navigate]);

    if (!isAuthenticated) {
      return <div>Loading...</div>; // Or a custom loader
    }

    return <WrappedComponent {...props} />;
  };

  return AuthComponent;
};

export default WithAuth;
