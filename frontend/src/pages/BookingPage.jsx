import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useBooking } from '../hooks/useBooking';
import { getNextDays, generateHours } from '../utils/dateUtils';
import BookingCalendar from '../components/BookingCalendar';
import { getLoggedInUser } from '../services/AuthService';

export default function BookingPage() {
  const { barbers, appointments, error, loadAppointments, bookSlot } = useBooking();
  const [selectedBarberId, setSelectedBarberId] = useState('');
  const [selectedService, setSelectedService] = useState('');

  const selectedBarber = barbers.find((b) => b.id === selectedBarberId);
  const availableServices = Array.isArray(selectedBarber?.services)
    ? selectedBarber.services
    : [];
  const selectedServiceDuration = getServiceDurationMinutes(selectedService);

  useEffect(() => {
    if (selectedBarberId) loadAppointments(selectedBarberId);
  }, [selectedBarberId]);

  const handleBooking = async (dayId, hour) => {
    const customer = getLoggedInUser();
    const barber = selectedBarber;

    const selectedTime = new Date(`${dayId}T${hour}:00`);
    if (selectedTime < new Date()) {
      alert('Tento termín je již v minulosti!');
      return;
    }

    if (!customer) {
      alert('Přihlaste se.');
      return;
    }
    if (!selectedService) {
      alert('Vyberte službu.');
      return;
    }
    if (isSlotBooked(dayId, hour)) {
      alert('Tento termín je již obsazen nebo se překrývá s jinou rezervací.');
      return;
    }

    const startIso = `${dayId}T${hour}:00`;
    const startDate = new Date(startIso);
    const endDate = new Date(startDate.getTime() + selectedServiceDuration * 60 * 1000);
    const endIso = toLocalIsoWithoutSeconds(endDate);

    const appointmentDto = {
      id: '00000000-0000-0000-0000-000000000000',
      customerId: customer.id,
      barberId: selectedBarberId,
      service: selectedService,
      startTime: startIso,
      endTime: endIso,
      customerName: customer.name,
      barberName: barber.fullName || `${barber.firstName} ${barber.lastName}`,
    };

    try {
      await bookSlot(appointmentDto);
      alert('Rezervace úspěšná!');
      await loadAppointments(selectedBarberId);
    } catch (bookingError) {
      alert(bookingError.message || 'Rezervace se nepodařila. Zkuste jiný termín.');
      await loadAppointments(selectedBarberId);
    }
  };

  const isSlotBooked = (dayId, hour) => {
    const slotStart = new Date(`${dayId}T${hour}:00`);
    const slotEnd = new Date(slotStart.getTime() + selectedServiceDuration * 60 * 1000);
    return appointments.some((appointment) => {
      const appointmentStart = new Date(appointment.startTime);
      const appointmentEnd = new Date(appointment.endTime);
      return appointmentStart < slotEnd && appointmentEnd > slotStart;
    });
  };

  const customer = getLoggedInUser();

  return (
    <div className="page-container booking-panel">
      <header className="page-header">
        <h1>Rezervace termínu</h1>
        <p>Vyberte barbera, službu a volný čas v kalendáři níže.</p>
      </header>

      {error && (
        <p role="alert" className="alert alert--error">
          {error}
        </p>
      )}

      {!customer && (
        <p className="alert alert--info">
          Pro rezervaci se prosím{' '}
          <Link to="/login" style={{ fontWeight: 600, color: 'inherit' }}>
            přihlaste
          </Link>
          {' '}nebo{' '}
          <Link to="/registrace" style={{ fontWeight: 600, color: 'inherit' }}>
            zaregistrujte
          </Link>
          .
        </p>
      )}

      <div className="card">
        <div className="booking-filters">
          <label className="form-field">
            <span className="form-label">Barber</span>
            <select
              className="form-select"
              value={selectedBarberId}
              onChange={(e) => {
                setSelectedBarberId(e.target.value);
                setSelectedService('');
              }}
            >
              <option value="">Vyberte barbera…</option>
              {barbers.map((b) => (
                <option key={b.id} value={b.id}>
                  {b.fullName || `${b.firstName} ${b.lastName}`}
                </option>
              ))}
            </select>
          </label>

          {selectedBarberId && (
            <label className="form-field">
              <span className="form-label">Služba</span>
              <select
                className="form-select"
                value={selectedService}
                onChange={(e) => setSelectedService(e.target.value)}
              >
                <option value="">Vyberte službu…</option>
                {availableServices.map((service) => (
                  <option key={service} value={service}>
                    {service}
                  </option>
                ))}
              </select>
            </label>
          )}
        </div>

        {selectedBarberId && selectedService && (
          <BookingCalendar
            days={getNextDays(5)}
            hours={generateHours()}
            isSlotBooked={isSlotBooked}
            onBook={handleBooking}
          />
        )}

        {selectedBarberId && !selectedService && (
          <p className="card__subtitle" style={{ marginTop: '1.25rem' }}>
            Vyberte službu pro zobrazení volných termínů.
          </p>
        )}
      </div>
    </div>
  );
}

function getServiceDurationMinutes(serviceLabel) {
  const match = /\((\d+)\s*min\)/i.exec(serviceLabel || '');
  const parsed = match ? Number.parseInt(match[1], 10) : NaN;
  return Number.isFinite(parsed) ? parsed : 60;
}

function toLocalIsoWithoutSeconds(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  return `${year}-${month}-${day}T${hours}:${minutes}`;
}
