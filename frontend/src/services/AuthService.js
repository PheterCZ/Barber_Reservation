const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5254';

export async function registerUser(payload) {
  const response = await fetch(`${API_BASE_URL}/api/Auth/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  });

  const data = await response.json().catch(() => ({}));

  if (!response.ok) {
    const message =
      Array.isArray(data?.errors) && data.errors.length > 0
        ? data.errors.join(', ')
        : Array.isArray(data?.Errors) && data.Errors.length > 0
          ? data.Errors.join(', ')
          : 'Registrace selhala.';

    throw new Error(message);
  }

  return data;
}
