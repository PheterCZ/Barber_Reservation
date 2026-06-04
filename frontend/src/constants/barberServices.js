export const BARBER_SERVICES = [
  { id: 'strih', label: 'Klasický střih', durationMinutes: 45 },
  { id: 'vousy', label: 'Vousy', durationMinutes: 30 },
  { id: 'vlasy-vousy', label: 'Vlasy + vousy', durationMinutes: 60 },
];

export function formatBarberServiceLabel({ label, durationMinutes }) {
  return `${label} (${durationMinutes} min)`;
}

export function buildBarberServiceLabels(selectedIds) {
  const idSet = new Set(selectedIds);
  return BARBER_SERVICES
    .filter((service) => idSet.has(service.id))
    .map(formatBarberServiceLabel);
}
