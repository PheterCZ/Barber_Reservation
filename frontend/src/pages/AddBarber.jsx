import React, { useEffect, useState } from 'react';
import { request } from '../services/AuthService';

const AddBarber = () => {
    const [form, setForm] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        specialization: '',
        startWork: ''
    });
    const [errors, setErrors] = useState({});
    const [status, setStatus] = useState('');
    const [loading, setLoading] = useState(false);
    const [barbers, setBarbers] = useState([]);
    const [fetchError, setFetchError] = useState('');

    useEffect(() => {
        const fetchBarbers = async () => {
            try {
                const data = await request('/api/barber', 'GET');
                setBarbers(Array.isArray(data) ? data : []);
            } catch (err) {
                setFetchError(err.message || 'Nepodařilo se načíst seznam barberů.');
            }
        };

        fetchBarbers();
    }, []);

    const validate = () => {
        const newErrors = {};
        if (!form.firstName.trim()) newErrors.firstName = 'Jméno je povinné.';
        if (!form.lastName.trim()) newErrors.lastName = 'Příjmení je povinné.';
        if (!form.email.trim()) {
            newErrors.email = 'E-mail je povinný.';
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
            newErrors.email = 'Zadejte platný e-mail.';
        }
        if (!form.phone.trim()) {
            newErrors.phone = 'Telefon je povinný.';
        }
        if (!form.startWork) newErrors.startWork = 'Datum nástupu je povinné.';

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm((prev) => ({ ...prev, [name]: value }));
        if (errors[name]) {
            setErrors((prev) => ({ ...prev, [name]: undefined }));
        }
        setStatus('');
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validate()) return;

        setLoading(true);
        setStatus('');

        try {
            const payload = {
                firstName: form.firstName.trim(),
                lastName: form.lastName.trim(),
                email: form.email.trim(),
                phone: form.phone.trim(),
                specialization: form.specialization.trim() || null,
                startWork: new Date(form.startWork).toISOString()
            };

            const createdBarber = await request('/api/barber', 'POST', payload);

            setBarbers((prev) => [...prev, createdBarber]);
            
            setForm({
                firstName: '',
                lastName: '',
                email: '',
                phone: '',
                specialization: '',
                startWork: ''
            });
            
            setStatus('Barber byl úspěšně přidán.');
            setErrors({});
        } catch (err) {
            setStatus(err.message || 'Nelze přidat barbera.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ maxWidth: 600, margin: '0 auto', padding: 20, fontFamily: 'sans-serif' }}>
            <h1>Správa barberů</h1>

            {fetchError && (
                <div style={{ color: 'white', background: '#d32f2f', padding: '10px', borderRadius: 4, marginBottom: 16 }}>
                    {fetchError}
                </div>
            )}

            <form onSubmit={handleSubmit} style={{ background: '#f9f9f9', padding: 20, borderRadius: 8, border: '1px solid #ddd' }}>
                <h3>Nový barber</h3>
                
                <div style={{ marginBottom: 12 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>Jméno</label>
                    <input type="text" name="firstName" value={form.firstName} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                    {errors.firstName && <small style={{ color: 'red' }}>{errors.firstName}</small>}
                </div>

                <div style={{ marginBottom: 12 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>Příjmení</label>
                    <input type="text" name="lastName" value={form.lastName} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                    {errors.lastName && <small style={{ color: 'red' }}>{errors.lastName}</small>}
                </div>

                <div style={{ marginBottom: 12 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>E-mail</label>
                    <input type="email" name="email" value={form.email} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                    {errors.email && <small style={{ color: 'red' }}>{errors.email}</small>}
                </div>

                <div style={{ marginBottom: 12 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>Telefon</label>
                    <input type="text" name="phone" value={form.phone} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                    {errors.phone && <small style={{ color: 'red' }}>{errors.phone}</small>}
                </div>

                <div style={{ marginBottom: 12 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>Specializace</label>
                    <input type="text" name="specialization" value={form.specialization} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                </div>

                <div style={{ marginBottom: 20 }}>
                    <label style={{ display: 'block', marginBottom: 4 }}>Nástup do práce</label>
                    <input type="date" name="startWork" value={form.startWork} onChange={handleChange} style={{ width: '100%', padding: 8, boxSizing: 'border-box' }} />
                    {errors.startWork && <small style={{ color: 'red' }}>{errors.startWork}</small>}
                </div>

                <button type="submit" disabled={loading} style={{ padding: '10px 20px', background: '#007bff', color: 'white', border: 'none', borderRadius: 4, cursor: loading ? 'not-allowed' : 'pointer', width: '100%' }}>
                    {loading ? 'Odesílám...' : 'Uložit barbera'}
                </button>

                {status && (
                    <div style={{ marginTop: 16, textAlign: 'center', fontWeight: 'bold', color: status.includes('úspěšně') ? 'green' : 'red' }}>
                        {status}
                    </div>
                )}
            </form>

            <section style={{ marginTop: 40 }}>
                <h2>Seznam aktivních barberů</h2>
                {barbers.length === 0 && !fetchError ? (
                    <p>Žádní barbaři nebyli nalezeni.</p>
                ) : (
                    <ul style={{ listStyle: 'none', padding: 0 }}>
                        {barbers.map((b) => (
                            <li key={b.id || b.email} style={{ padding: '12px', borderBottom: '1px solid #eee', display: 'flex', justifyContent: 'space-between' }}>
                                <strong>{b.firstName} {b.lastName}</strong>
                                <span style={{ color: '#666' }}>{b.specialization || 'Bez specializace'}</span>
                            </li>
                        ))}
                    </ul>
                )}
            </section>
        </div>
    );
};

export default AddBarber;