export default function BookingCalendar({ days, hours, bookedSlots, onBook }) {
  const now = new Date();

  return (
    <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(140px, 1fr))', gap: '16px' }}>
      {days.map(day => (
        <div key={day.id} style={{ border: '1px solid #ddd', padding: '12px' }}>
          <h4>{day.label}</h4>
          {hours.map(hour => {
            const slotDateTime = new Date(`${day.id}T${hour}:00`);
            const isBooked = bookedSlots[`${day.id}_${hour}`];
            const isPast = slotDateTime < now;

            return (
              <button 
                key={hour} 
                disabled={isBooked || isPast} 
                onClick={() => onBook(day.id, hour)}
                style={{ display: 'block', width: '100%', marginBottom: '4px' }}
              >
                {isBooked ? 'Obsazeno' : (isPast ? 'Minulost' : hour)}
              </button>
            );
          })}
        </div>
      ))}
    </div>
  );
}