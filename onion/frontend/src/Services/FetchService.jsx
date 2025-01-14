import axios from 'axios';
import { getAccessToken } from './TokenService';

const api = axios.create({
  baseURL: 'http://localhost:5127', // Replace this with your API base URL
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

export async function deleteData(endpoint, data) {
  try {
    const response = await api.delete(endpoint, {
      headers: {
        'Content-Type': 'application/json',
      },
      data: data, // Pass the DTO in the body
    });

    return response.data; // Axios automatically parses JSON responses
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

export async function getDataWithIds(apiEndpoint, id1, id2) {
  try {
    // Construct the query string with two IDs
    const queryString = new URLSearchParams({ id1, id2 }).toString();
    const response = await api.get(`${apiEndpoint}?${queryString}`);
    return response.data; // Return response.data to get the data directly
  } catch (error) {
    console.error('Error fetching data with IDs');
    throw error;
  }
}
