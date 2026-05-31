import { request } from './AuthService';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5254';

export const fetchBarbers = async () => {
  const res = await fetch(`${API_BASE_URL}/api/barber`);
  if (!res.ok) throw new Error('Nepodařilo se načíst seznam barberů.');
  return res.json();
};

export const fetchAppointments = async () => {
  const res = await fetch(`${API_BASE_URL}/api/Appointment`);
  return res.json();
};

export const createAppointment = async (appointmentDto) => {
  return await request('/api/Appointment', 'POST', appointmentDto);
};