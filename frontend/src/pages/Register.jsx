import { useState } from 'react';
import { Link } from 'react-router-dom';
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
      setSuccessMessage('Registrace proběhla úspěšně, posíláme vám úvodní e-mail!');
      setFormData(initialForm);
    } catch (error) {
      setErrorMessage(error.message || 'Registrace se nepodařila.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="card card--narrow">
        <header>
          <h1 className="card__title">Registrace</h1>
          <p className="card__subtitle">
            Vytvořte si účet a rezervujte si termín u vašeho barbera.
          </p>
        </header>

        <form onSubmit={handleSubmit} className="form">
          <label className="form-field">
            <span className="form-label">Email</span>
            <input
              type="email"
              name="email"
              className="form-input"
              value={formData.email}
              onChange={handleChange}
              required
              disabled={loading}
              autoComplete="email"
            />
          </label>

          <label className="form-field">
            <span className="form-label">Heslo (min. 6 znaků)</span>
            <input
              type="password"
              name="password"
              className="form-input"
              value={formData.password}
              onChange={handleChange}
              required
              minLength={6}
              disabled={loading}
              autoComplete="new-password"
            />
          </label>

          <label className="form-field">
            <span className="form-label">Jméno</span>
            <input
              type="text"
              name="firstName"
              className="form-input"
              value={formData.firstName}
              onChange={handleChange}
              required
              maxLength={50}
              disabled={loading}
              autoComplete="given-name"
            />
          </label>

          <label className="form-field">
            <span className="form-label">Příjmení</span>
            <input
              type="text"
              name="lastName"
              className="form-input"
              value={formData.lastName}
              onChange={handleChange}
              required
              maxLength={50}
              disabled={loading}
              autoComplete="family-name"
            />
          </label>

          <label className="form-field">
            <span className="form-label">Telefon</span>
            <input
              type="tel"
              name="phone"
              className="form-input"
              value={formData.phone}
              onChange={handleChange}
              maxLength={20}
              disabled={loading}
              autoComplete="tel"
            />
          </label>

          {successMessage && (
            <p role="status" className="alert alert--success">
              {successMessage}
            </p>
          )}
          {errorMessage && (
            <p role="alert" className="alert alert--error">
              {errorMessage}
            </p>
          )}

          <button type="submit" disabled={loading} className="btn btn--primary btn--block">
            {loading ? 'Odesílám…' : 'Registrovat'}
          </button>

          <p className="card__subtitle auth-footer">
            Už máte účet? <Link to="/login">Přihlásit se</Link>
          </p>
        </form>
      </div>
    </div>
  );
}
