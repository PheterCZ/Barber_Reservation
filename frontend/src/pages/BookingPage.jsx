import { useState, useEffect } from 'react';
import { useBooking } from '../hooks/useBooking';
import { getNextDays, generateHours } from '../utils/dateUtils';
import BookingCalendar from '../components/BookingCalendar';
import { getLoggedInUser } from '../services/AuthService'; 

export default function BookingPage() {
  const { barbers, bookedSlots, error, loadAppointments, bookSlot } = useBooking();
  const [selectedBarberId, setSelectedBarberId] = useState('');

  useEffect(() => {
    if (selectedBarberId) loadAppointments(selectedBarberId);
  }, [selectedBarberId]);

  const handleBooking = async (dayId, hour) => {
    const customer = getLoggedInUser();
    const barber = barbers.find(b => b.id === selectedBarberId);

    const selectedTime = new Date(`${dayId}T${hour}:00`);
    if (selectedTime < new Date()) {
      alert('Tento termín je již v minulosti!');
      return;
    }

    if (!customer) { alert('Přihlaste se.'); return; }
    
    const startIso = `${dayId}T${hour}:00`;
    const endHour = (parseInt(hour.substring(0, 2), 10) + 1).toString().padStart(2, '0');
    const endIso = `${dayId}T${endHour}:00`;

    const appointmentDto = {
      id: '00000000-0000-0000-0000-000000000000',
      customerId: customer.id,
      barberId: selectedBarberId,
      service: 'Pánský střih',
      startTime: startIso,
      endTime: endIso,
      customerName: customer.name,
      barberName: barber.fullName || `${barber.firstName} ${barber.lastName}`,
    };

    await bookSlot(appointmentDto);
    alert('Rezervace úspěšná!');
  };

  return (
    <div>
      <h2>Rezervace termínu u barbera</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      
      <select onChange={(e) => setSelectedBarberId(e.target.value)}>
        <option value="">Vyberte barbera...</option>
        {barbers.map(b => (
          <option key={b.id} value={b.id}>
            {b.fullName || `${b.firstName} ${b.lastName}`}
          </option>
        ))}
      </select>

      {selectedBarberId && (
        <BookingCalendar 
          days={getNextDays(5)} 
          hours={generateHours()} 
          bookedSlots={bookedSlots} 
          onBook={handleBooking} 
        />
      )}
    </div>
  );
}