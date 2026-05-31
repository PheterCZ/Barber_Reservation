import { request } from './AuthService';

export const fetchBarbersApi = () => request('/api/barber', 'GET');

export const createBarberApi = (data) => {
    const payload = {
        firstName: data.firstName.trim(),
        lastName: data.lastName.trim(),
        email: data.email.trim(),
        phone: data.phone.trim(),
        specialization: data.specialization.trim() || null,
        startWork: new Date(data.startWork).toISOString()
    };

    if(payload.firstName === '' || payload.lastName === '' || payload.email === '' || payload.phone === '' || payload.startWork === '') {
        return Promise.reject(new Error('Všechna pole jsou povinná.'));
    }
    
    return request('/api/barber', 'POST', payload);
};