import axios from 'axios';
import { getAccessToken } from './TokenService';

const api = axios.create({
  baseURL: 'http://localhost:5157', // Replace this with your API base URL
});

api.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${getAccessToken()}`;
  return config;
});

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


export async function deleteData(endpoint, params) {
  let url = endpoint;
  if (params && Object.keys(params).length > 0) {
      const queryString = new URLSearchParams(params).toString();
      url = `${endpoint}?${queryString}`;
  }

  const options = {
      method: 'DELETE',
      headers: {
          'Content-Type': 'application/json',
      },
  };

  try {
      const response = await fetch(url, options);
      const responseText = await response.text(); // Get the raw response text

      console.log('Response Text:', responseText); // Log it for debugging

      if (!response.ok) {
          throw new Error(`Error: ${response.status} - ${responseText}`);
      }

      return JSON.parse(responseText); // Parse the JSON manually
  } catch (error) {
      console.error('Error:', error);
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
