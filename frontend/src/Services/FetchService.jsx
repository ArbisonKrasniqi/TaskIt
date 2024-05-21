import axios from 'axios';
import { GetToken } from './TokenService';

const api = axios.create({
  baseURL: 'YOUR_API_BASE_URL', // Replace this with your API base URL
});

// Request interceptor to add token to headers
api.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${GetToken()}`;
  return config;
});

// Response interceptor to handle errors
api.interceptors.response.use(
    (response) => response.data,
    (error) => {
      if (error.response) {
        // The request was made and the server responded with a status code
        if (error.response.status === 401) {
          // Handle 401 Unauthorized error here (e.g., redirect to login page)
          console.error('Unauthorized access. Redirecting to login page...');
          // Redirect to login page or dispatch logout action
          throw new Error(`401: Unauthorized`);
        }
        throw new Error(`${error.response.status}: ${error.response.data}`);
      } else if (error.request) {
        // The request was made but no response was received
        throw new Error('No response from server');
      } else {
        // Something happened in setting up the request that triggered an Error
        throw new Error('Request failed to set up');
      }
    }
  );

export async function getData(apiEndpoint) {
  try {
    const response = await api.get(apiEndpoint);
    return response;
  } catch (error) {
    throw error;
  }
}

export async function getDataWithId(apiEndpoint, id) {
  try {
    const response = await api.get(`${apiEndpoint}=${id}`);
    return response;
  } catch (error) {
    throw error;
  }
}

export async function postData(apiEndpoint, data) {
  try {
    const response = await api.post(apiEndpoint, data);
    return response;
  } catch (error) {
    throw error;
  }
}

export async function deleteData(apiEndpoint, data) {
  try {
    const response = await api.delete(apiEndpoint, { data });
    return response;
  } catch (error) {
    throw error;
  }
}

export async function putData(apiEndpoint, data) {
  try {
    const response = await api.put(apiEndpoint, data);
    return response;
  } catch (error) {
    throw error;
  }
}
