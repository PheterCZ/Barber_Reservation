import { useState } from 'react';
import { registerUser } from '../services/AuthService';

const initialForm = {
  email: '',
  password: '',
  firstName: '',
  lastName: '',
  phone: '',
};

export default function Register() {
  const [formData, setFormData] = useState(initialForm);
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setSuccessMessage('');
    setErrorMessage('');

    try {
      await registerUser(formData);
      setSuccessMessage('Registrace proběhla úspěšně.');
      setFormData(initialForm);
    } catch (error) {
      setErrorMessage(error.message || 'Registrace se nepodařila.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <section style={{ maxWidth: '500px' }}>
      <h2>Registrace uživatele</h2>

      <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '0.75rem' }}>
        <label>
          Email
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            style={{ display: 'block', width: '100%', marginTop: '0.3rem' }}
          />
        </label>

        <label>
          Heslo (min. 6 znaků)
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
            minLength={6}
            style={{ display: 'block', width: '100%', marginTop: '0.3rem' }}
          />
        </label>

        <label>
          Jméno
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
            maxLength={50}
            style={{ display: 'block', width: '100%', marginTop: '0.3rem' }}
          />
        </label>

        <label>
          Příjmení
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
            maxLength={50}
            style={{ display: 'block', width: '100%', marginTop: '0.3rem' }}
          />
        </label>

        <label>
          Telefon
          <input
            type="text"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            maxLength={20}
            style={{ display: 'block', width: '100%', marginTop: '0.3rem' }}
          />
        </label>

        <button type="submit" disabled={loading}>
          {loading ? 'Odesílám...' : 'Registrovat'}
        </button>
      </form>

      {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}
      {errorMessage && <p style={{ color: 'crimson' }}>{errorMessage}</p>}
    </section>
  );
}
