import { useState } from 'react';
import { registerUser } from '../services/AuthService';
import { styles } from '../styles/Login.styles';


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
    <section style={styles.page}>
      <div style={styles.card}>
        <header>
          <h1 style={styles.heading}>Registrace</h1>
          <p style={styles.subheading}>
            Vytvoř nový účet do BarberOrder administrace.
          </p>
        </header>

        <form onSubmit={handleSubmit} style={styles.form}>
          <label style={styles.field}>
            <span style={styles.label}>Email</span>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            disabled={loading}
            autoComplete="email"
            style={styles.input}
          />
          </label>

          <label style={styles.field}>
            <span style={styles.label}>Heslo (min. 6 znaků)</span>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
            minLength={6}
            disabled={loading}
            autoComplete="new-password"
            style={styles.input}
          />
          </label>

          <label style={styles.field}>
            <span style={styles.label}>Jméno</span>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
            maxLength={50}
            disabled={loading}
            autoComplete="given-name"
            style={styles.input}
          />
          </label>

          <label style={styles.field}>
            <span style={styles.label}>Příjmení</span>
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
            maxLength={50}
            disabled={loading}
            autoComplete="family-name"
            style={styles.input}
          />
          </label>

          <label style={styles.field}>
            <span style={styles.label}>Telefon</span>
          <input
            type="text"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            maxLength={20}
            disabled={loading}
            autoComplete="tel"
            style={styles.input}
          />
          </label>

          {successMessage && (
            <p role="status" style={{ ...styles.error, backgroundColor: '#f0fdf4', borderColor: '#bbf7d0', color: '#166534' }}>
              {successMessage}
            </p>
          )}
          {errorMessage && (
            <p role="alert" style={styles.error}>
              {errorMessage}
            </p>
          )}

          <button
            type="submit"
            disabled={loading}
            style={{ ...styles.button, opacity: loading ? 0.75 : 1 }}
          >
            {loading ? 'Odesílám...' : 'Registrovat'}
          </button>
        </form>
      </div>
    </section>
  );
}
