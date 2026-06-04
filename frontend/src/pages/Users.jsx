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
    <section className="page-container">
      <header className="page-header">
        <h1>Seznam uživatelů</h1>
        <p>Přehled registrovaných účtů a jejich rolí v systému.</p>
      </header>

      {loading && <p className="loading-text">Načítám uživatele…</p>}

      {!loading && errorMessage && (
        <p role="alert" className="alert alert--error">
          {errorMessage}
        </p>
      )}

      {!loading && !errorMessage && users.length === 0 && (
        <p className="empty-state">V systému zatím nejsou žádní uživatelé.</p>
      )}

      {!loading && !errorMessage && users.length > 0 && (
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Jméno</th>
                <th>Email</th>
                <th>Role</th>
              </tr>
            </thead>
            <tbody>
              {users.map((user) => (
                <tr key={user.id}>
                  <td>{user.fullName || '—'}</td>
                  <td>{user.email || '—'}</td>
                  <td>
                    {user.roles && user.roles.length > 0
                      ? user.roles.join(', ')
                      : 'Bez role'}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}
