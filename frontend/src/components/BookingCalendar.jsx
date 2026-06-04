export default function BookingCalendar({ days, hours, isSlotBooked, onBook }) {
  const now = new Date();

  return (
    <div className="booking-calendar" style={{ marginTop: '1.5rem' }}>
      {days.map((day) => (
        <div key={day.id} className="calendar-day">
          <h4 className="calendar-day__title">{day.label}</h4>
          <div className="calendar-day__slots">
            {hours.map((hour) => {
              const slotDateTime = new Date(`${day.id}T${hour}:00`);
              const isBooked = isSlotBooked(day.id, hour);
              const isPast = slotDateTime < now;

              let buttonClass = 'btn--slot';
              if (isBooked) buttonClass += ' btn--slot-booked';
              else if (isPast) buttonClass += ' btn--slot-past';

              return (
                <button
                  key={hour}
                  type="button"
                  className={buttonClass}
                  disabled={isBooked || isPast}
                  onClick={() => onBook(day.id, hour)}
                >
                  {isBooked ? 'Obsazeno' : isPast ? 'Minulost' : hour}
                </button>
              );
            })}
          </div>
        </div>
      ))}
    </div>
  );
}
