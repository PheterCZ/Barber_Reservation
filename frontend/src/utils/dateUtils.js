export const getNextDays = (count) => {
  const days = [];
  const options = { weekday: 'short', month: 'numeric', day: 'numeric' };

  for (let i = 0; i < count; i++) {
    const date = new Date();
    date.setHours(12, 0, 0, 0);
    date.setDate(date.getDate() + i);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    days.push({
      id: `${year}-${month}-${day}`,
      label: date.toLocaleDateString('cs-CZ', options)
    });
  }
  return days;
};

export const generateHours = () => {
  const hours = [];
  for (let h = 8; h <= 20; h++) {
    hours.push(`${h.toString().padStart(2, '0')}:00`);
  }
  return hours;
};
