import { useEffect, useState } from 'react';
import { getUsers } from '../services/AuthService';

export default function Users() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    async function loadUsers() {
      try {
        const data = await getUsers();
        setUsers(Array.isArray(data) ? data : []);
      } catch (error) {
        setErrorMessage(error.message || 'Nepodařilo se načíst uživatele.');
      } finally {
        setLoading(false);
      }
    }

    loadUsers();
  }, []);

  return (
    <section style={{ padding: '1rem' }}>
      <h1>Seznam uživatelů</h1>

      {loading && <p>Načítám uživatele...</p>}

      {!loading && errorMessage && (
        <p role="alert" style={{ color: '#b91c1c' }}>
          {errorMessage}
        </p>
      )}

      {!loading && !errorMessage && (
        <>
          {users.length === 0 ? (
            <p>V systému zatím nejsou žádní uživatelé.</p>
          ) : (
            <table
              style={{
                width: '100%',
                borderCollapse: 'collapse',
                marginTop: '1rem',
                backgroundColor: '#fff',
                border: '1px solid #e2e8f0'
              }}
            >
              <thead>
                <tr style={{ backgroundColor: '#f8fafc' }}>
                  <th style={thStyle}>Jméno</th>
                  <th style={thStyle}>Email</th>
                  <th style={thStyle}>Role</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.id}>
                    <td style={tdStyle}>{user.fullName || '-'}</td>
                    <td style={tdStyle}>{user.email || '-'}</td>
                    <td style={tdStyle}>
                      {user.roles && user.roles.length > 0 
                        ? user.roles.join(', ') 
                        : 'Bez role'}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </>
      )}
    </section>
  );
}

const thStyle = { 
    textAlign: 'left', 
    padding: '0.75rem', 
    borderBottom: '2px solid #e2e8f0',
    fontWeight: 'bold' 
};

const tdStyle = { 
    padding: '0.75rem', 
    borderBottom: '1px solid #f1f5f9' 
};