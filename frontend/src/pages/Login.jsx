import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { loginUser } from '../services/AuthService';
import { styles } from '../styles/Login.styles';
const initialFormState = {
  email: '',
  password: ''
};

export default function Login() {
  const [formData, setFormData] = useState(initialFormState);
  const [errorMessage, setErrorMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setErrorMessage('');
    setLoading(true);

    try {
      await loginUser(formData);
      
      setFormData(initialFormState);
      navigate('/dashboard');
    } catch (error) {
      setErrorMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <section style={styles.page}>
      <div style={styles.card}>
        <header>
          <h1 style={styles.heading}>Přihlášení</h1>
          <p style={styles.subheading}>
            Přihlas se do BarberOrder a pokračuj do administrace.
          </p>
        </header>

        <form onSubmit={handleSubmit} style={styles.form}>
          <label style={styles.field}>
            <span style={styles.label}>Email</span>
            <input
              type="email"
              name="email"
              placeholder="napr. jan@barberorder.cz"
              value={formData.email}
              onChange={handleChange}
              required
              disabled={loading}
              autoComplete="email"
              style={styles.input}
            />
          </label>

          <label style={styles.field}>
            <span style={styles.label}>Heslo</span>
            <input
              type="password"
              name="password"
              placeholder="Zadej heslo"
              value={formData.password}
              onChange={handleChange}
              required
              disabled={loading}
              autoComplete="current-password"
              style={styles.input}
            />
          </label>

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
            {loading ? 'Přihlašuji...' : 'Přihlásit se'}
          </button>
        </form>
      </div>
    </section>
  );
}