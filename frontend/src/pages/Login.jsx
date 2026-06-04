import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { loginUser } from '../services/AuthService';

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
      navigate('/');
    } catch (error) {
      setErrorMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="card card--narrow">
        <header>
          <h1 className="card__title">Přihlášení</h1>
          <p className="card__subtitle">
            Přihlaste se do BarberOrder a pokračujte k rezervaci nebo administraci.
          </p>
        </header>

        <form onSubmit={handleSubmit} className="form">
          <label className="form-field">
            <span className="form-label">Email</span>
            <input
              type="email"
              name="email"
              className="form-input"
              placeholder="např. jan@barberorder.cz"
              value={formData.email}
              onChange={handleChange}
              required
              disabled={loading}
              autoComplete="email"
            />
          </label>

          <label className="form-field">
            <span className="form-label">Heslo</span>
            <input
              type="password"
              name="password"
              className="form-input"
              placeholder="Zadejte heslo"
              value={formData.password}
              onChange={handleChange}
              required
              disabled={loading}
              autoComplete="current-password"
            />
          </label>

          {errorMessage && (
            <p role="alert" className="alert alert--error">
              {errorMessage}
            </p>
          )}

          <button type="submit" disabled={loading} className="btn btn--primary btn--block">
            {loading ? 'Přihlašuji…' : 'Přihlásit se'}
          </button>

          <p className="card__subtitle auth-footer">
            Nemáte účet? <Link to="/registrace">Registrovat se</Link>
          </p>
        </form>
      </div>
    </div>
  );
}
