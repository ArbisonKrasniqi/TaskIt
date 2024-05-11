import { GetToken } from './TokenService';


//api eshte URL i api endpoint
export async function getData(api) {
    const response = await fetch(api, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Authorization': `Bearer ${GetToken()}`
        }
    });

    if (!response.ok) {
        throw new Error ("Error");
    }
    return await response.json();
}

//input eshte zakonisht id
export async function getDataWithId(api, id) {
    const url = `${api}=${id}`;
    const response = await fetch(url, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Authorization': `Bearer ${GetToken()}`
        }
    });

    if (!response.ok) {
        throw new Error("Error");
    }

    return await response.json();
}

//data variable should be a javascript object
export async function postData(api, data) {
    const response = await fetch(api, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Bearer ${GetToken()}`
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        throw new Error("Error");
    }

    return await response.json();
}


export async function deleteData(api, data) {
    const response = await fetch(api, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Bearer ${GetToken()}`
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        throw new Error("Error");
    }

    return await response.json();
}

export async function putData(api, data) {
    const response = await fetch(api, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Bearer ${GetToken()}`
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        throw new Error("Error");
    }

    return await response.json();
}