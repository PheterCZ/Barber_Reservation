import { useState, useEffect } from 'react';
import { fetchBarbersApi } from '../services/AddBarberService';

export const useBarbers = () => {
    const [barbers, setBarbers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const refreshBarbers = async () => {
        setLoading(true);
        try {
            const data = await fetchBarbersApi();
            setBarbers(Array.isArray(data) ? data : []);
            setError('');
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        refreshBarbers();
    }, []);

    return { barbers, setBarbers, loading, error,  refreshBarbers};
};