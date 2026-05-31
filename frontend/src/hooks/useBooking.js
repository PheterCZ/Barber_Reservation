import { useState, useEffect } from 'react';
import { fetchBarbers, fetchAppointments, createAppointment } from '../services/BookingService';

export const useBooking = () => {
  const [barbers, setBarbers] = useState([]);
  const [bookedSlots, setBookedSlots] = useState({});
  const [error, setError] = useState('');

  useEffect(() => {
    fetchBarbers().then(setBarbers).catch(err => setError(err.message));
  }, []);

  const loadAppointments = async (barberId) => {
    try {
      const appointments = await fetchAppointments();
      const slots = {};
      appointments.filter(a => a.barberId === barberId).forEach(a => {
        slots[`${a.startTime.split('T')[0]}_${a.startTime.substring(11, 16)}`] = true;
      });
      setBookedSlots(slots);
    } catch (err) { console.error(err); }
  };

  const bookSlot = async (dto) => {
    await createAppointment(dto);
    setBookedSlots(prev => ({ ...prev, [`${dto.startTime.split('T')[0]}_${dto.startTime.substring(11, 16)}`]: true }));
  };

  return { barbers, bookedSlots, error, loadAppointments, bookSlot };
};