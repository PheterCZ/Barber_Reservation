import { useState, useEffect } from 'react';
import { fetchBarbers, fetchAppointments, createAppointment } from '../services/BookingService';

export const useBooking = () => {
  const [barbers, setBarbers] = useState([]);
  const [appointments, setAppointments] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchBarbers().then(setBarbers).catch(err => setError(err.message));
  }, []);

  const loadAppointments = async (barberId) => {
    try {
      const allAppointments = await fetchAppointments();
      setAppointments(allAppointments.filter(a => a.barberId === barberId));
    } catch (err) { console.error(err); }
  };

  const bookSlot = async (dto) => {
    await createAppointment(dto);
    setAppointments((prev) => [...prev, dto]);
  };

  return { barbers, appointments, error, loadAppointments, bookSlot };
};