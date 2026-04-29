const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5254';

async function request(endpoint, method = 'GET', body = null) {
  const token = localStorage.getItem('token');
  
  const headers = {
    'Content-Type': 'application/json',
  };

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const options = {
    method,
    headers,
  };

  if (body) {
    options.body = JSON.stringify(body);
  }

  const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
  return parseResponse(response);
}

async function parseResponse(response) {
  const isJson = response.headers.get('content-type')?.includes('application/json');
  const data = isJson ? await response.json().catch(() => ({})) : {};

  if (!response.ok) {
    const errorMsg = data?.message || data?.error || (data?.errors && Object.values(data.errors).flat().join(', ')) || 'Server error';
    
    if (response.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }

    throw new Error(errorMsg);
  }

  return data;
}


export async function registerUser(payload) {
  return request('/api/Auth/register', 'POST', payload);
}

export async function loginUser(payload) {
  const data = await request('/api/Auth/login', 'POST', payload);
  
  if (data.token) {
    localStorage.setItem('token', data.token);
    if (data.user) localStorage.setItem('user', JSON.stringify(data.user));
  }
  
  return data;
}

