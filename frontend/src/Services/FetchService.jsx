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
