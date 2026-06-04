import React, { useState, useRef } from 'react';
import { useBarbers } from '../hooks/useBarber';
import { createBarberApi } from '../services/AddBarberService';
import { BARBER_SERVICES } from '../constants/barberServices';

const openDatePicker = (input) => {
    if (!input) return;
    if (typeof input.showPicker === 'function') {
        try { input.showPicker(); } catch {}
    } else {
        input.focus();
    }
};

const AddBarber = () => {
    const { barbers, setBarbers, error: fetchError } = useBarbers();

    const [form, setForm] = useState({
        firstName: '', lastName: '', email: '', phone: '', selectedServiceIds: [], startWork: ''
    });
    const [errors, setErrors] = useState({});
    const [status, setStatus] = useState('');
    const [loading, setLoading] = useState(false);
    const startWorkRef = useRef(null);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));

        if (errors[name]) setErrors(prev => ({ ...prev, [name]: undefined }));
        setStatus('');
    };

    const handleServiceToggle = (serviceId) => {
        setForm((prev) => {
            const selected = prev.selectedServiceIds.includes(serviceId)
                ? prev.selectedServiceIds.filter((id) => id !== serviceId)
                : [...prev.selectedServiceIds, serviceId];
            return { ...prev, selectedServiceIds: selected };
        });
        if (errors.selectedServiceIds) {
            setErrors((prev) => ({ ...prev, selectedServiceIds: undefined }));
        }
        setStatus('');
    };

    const validate = () => {
        const newErrors = {};
        if (!form.firstName.trim()) newErrors.firstName = 'Jméno je povinné.';
        if (!form.lastName.trim()) newErrors.lastName = 'Příjmení je povinné.';
        if (!form.email.trim()) newErrors.email = 'E-mail je povinný.';
        if (!form.selectedServiceIds.length) newErrors.selectedServiceIds = 'Vyberte alespoň jednu službu.';
        if (!form.startWork) newErrors.startWork = 'Datum nástupu je povinné.';
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validate()) return;
        setLoading(true);
        try {
            const newBarber = await createBarberApi(form);
            setBarbers(prev => [...prev, newBarber]);
            setStatus('Barber úspěšně přidán.');
            setForm({ firstName: '', lastName: '', email: '', phone: '', selectedServiceIds: [], startWork: '' });
        } catch (err) {
            setStatus(err.message || 'Chyba při ukládání.');
        } finally {
            setLoading(false);
        }
    };

    const statusIsSuccess = status.includes('úspěšně');

    return (
        <div className="page-container barber-layout">
            <header className="page-header" style={{ gridColumn: '1 / -1' }}>
                <h1>Správa barberů</h1>
                <p>Přidejte nového barbera a spravujte seznam aktivních členů týmu.</p>
            </header>

            {fetchError && (
                <p role="alert" className="alert alert--error" style={{ gridColumn: '1 / -1' }}>
                    {fetchError}
                </p>
            )}

            <div className="card">
                <h2 style={{ marginBottom: '0.25rem' }}>Nový barber</h2>
                <p className="card__subtitle" style={{ marginBottom: '1.25rem' }}>
                    Vyberte služby, které barber nabízí.
                </p>

                <form onSubmit={handleSubmit} className="form">
                    <div className="form--two-col" style={{ display: 'grid', gap: '1.1rem' }}>
                        <label className="form-field">
                            <span className="form-label">Jméno</span>
                            <input
                                type="text"
                                name="firstName"
                                className={`form-input${errors.firstName ? ' form-input--error' : ''}`}
                                value={form.firstName}
                                onChange={handleChange}
                            />
                            {errors.firstName && <span className="form-hint">{errors.firstName}</span>}
                        </label>

                        <label className="form-field">
                            <span className="form-label">Příjmení</span>
                            <input
                                type="text"
                                name="lastName"
                                className={`form-input${errors.lastName ? ' form-input--error' : ''}`}
                                value={form.lastName}
                                onChange={handleChange}
                            />
                            {errors.lastName && <span className="form-hint">{errors.lastName}</span>}
                        </label>

                        <label className="form-field form-field--full">
                            <span className="form-label">E-mail</span>
                            <input
                                type="email"
                                name="email"
                                className={`form-input${errors.email ? ' form-input--error' : ''}`}
                                value={form.email}
                                onChange={handleChange}
                            />
                            {errors.email && <span className="form-hint">{errors.email}</span>}
                        </label>

                        <label className="form-field form-field--full">
                            <span className="form-label">Telefon</span>
                            <input
                                type="text"
                                name="phone"
                                className="form-input"
                                value={form.phone}
                                onChange={handleChange}
                            />
                        </label>

                        <fieldset className="form-field form-field--full service-picker">
                            <legend className="form-label">Služby</legend>
                            <div className="service-picker__options">
                                {BARBER_SERVICES.map((service) => (
                                    <label key={service.id} className="service-picker__option">
                                        <input
                                            type="checkbox"
                                            checked={form.selectedServiceIds.includes(service.id)}
                                            onChange={() => handleServiceToggle(service.id)}
                                        />
                                        <span className="service-picker__label">
                                            {service.label}
                                            <span className="service-picker__duration">
                                                {service.durationMinutes} min
                                            </span>
                                        </span>
                                    </label>
                                ))}
                            </div>
                            {errors.selectedServiceIds && (
                                <span className="form-hint">{errors.selectedServiceIds}</span>
                            )}
                        </fieldset>

                        <label className="form-field form-field--full">
                            <span
                                className="form-label"
                                style={{ cursor: 'pointer' }}
                                onClick={() => openDatePicker(startWorkRef.current)}
                            >
                                Nástup do práce
                            </span>
                            <input
                                ref={startWorkRef}
                                id="startWork"
                                type="date"
                                name="startWork"
                                className={`form-input${errors.startWork ? ' form-input--error' : ''}`}
                                value={form.startWork}
                                onChange={handleChange}
                                onClick={(e) => openDatePicker(e.currentTarget)}
                                style={{ cursor: 'pointer' }}
                            />
                            {errors.startWork && <span className="form-hint">{errors.startWork}</span>}
                        </label>
                    </div>

                    <button type="submit" disabled={loading} className="btn btn--primary btn--block">
                        {loading ? 'Odesílám…' : 'Uložit barbera'}
                    </button>

                    {status && (
                        <p
                            role="status"
                            className={`alert ${statusIsSuccess ? 'alert--success' : 'alert--error'}`}
                        >
                            {status}
                        </p>
                    )}
                </form>
            </div>

            <div className="card">
                <h2 style={{ marginBottom: '1rem' }}>Aktivní barbaři</h2>
                {barbers.length === 0 && !fetchError ? (
                    <p className="empty-state">Žádní barbaři nebyli nalezeni.</p>
                ) : (
                    <ul className="barber-list">
                        {barbers.map((b) => (
                            <li key={b.id || b.email} className="barber-list__item">
                                <span className="barber-list__name">
                                    {b.firstName} {b.lastName}
                                </span>
                                <span className="barber-list__services">
                                    {Array.isArray(b.services) && b.services.length > 0
                                        ? b.services.join(', ')
                                        : (b.specialization || 'Bez specializace')}
                                </span>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
};

export default AddBarber;
