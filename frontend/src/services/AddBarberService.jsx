import { request } from './AuthService';
import { buildBarberServiceLabels } from '../constants/barberServices';

export const fetchBarbersApi = () => request('/api/barber', 'GET');

export const createBarberApi = (data) => {
    const normalizedServices = buildBarberServiceLabels(data.selectedServiceIds ?? []);

    const payload = {
        firstName: data.firstName.trim(),
        lastName: data.lastName.trim(),
        email: data.email.trim(),
        phone: data.phone.trim(),
        specialization: normalizedServices.join(', ') || null,
        services: normalizedServices,
        startWork: new Date(data.startWork).toISOString()
    };

    if(payload.firstName === '' || payload.lastName === '' || payload.email === '' || payload.phone === '' || payload.startWork === '' || payload.services.length === 0) {
        return Promise.reject(new Error('Všechna pole jsou povinná.'));
    }
    
    return request('/api/barber', 'POST', payload);
};