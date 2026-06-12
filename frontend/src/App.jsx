import { BrowserRouter, Link, NavLink, Routes, Route } from 'react-router-dom';
import { useState, useEffect } from 'react';
import Register from './pages/Register';
import Login from './pages/Login';
import AddBarber from './pages/BarberPage';
import Users from './pages/Users';
import Booking from './pages/BookingPage';
import { hasAdminRole } from './services/AuthService';

export default function App() {
    const [isAdmin, setIsAdmin] = useState(hasAdminRole());
    const [isLoggedIn, setIsLoggedIn] = useState(Boolean(localStorage.getItem('token')));

    useEffect(() => {
        const updateAuth = () => {
            setIsAdmin(hasAdminRole());
            setIsLoggedIn(Boolean(localStorage.getItem('token')));
        };

        updateAuth();

        const onAuthChange = () => updateAuth();
        const onStorage = (e) => {
            if (!e || !e.key) return;
            if (e.key === 'token' || e.key === 'user') updateAuth();
        };

        window.addEventListener('authChange', onAuthChange);
        window.addEventListener('storage', onStorage);

        return () => {
            window.removeEventListener('authChange', onAuthChange);
            window.removeEventListener('storage', onStorage);
        };
    }, []);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        setIsAdmin(false);
        setIsLoggedIn(false);
        try { window.dispatchEvent(new CustomEvent('authChange', { detail: { type: 'logout' } })); } catch {}
        window.location.href = '/login';
    };

    const navLinkClass = ({ isActive }) =>
        isActive ? 'navbar__link navbar__link--active' : 'navbar__link';

    const adminNavLinkClass = ({ isActive }) =>
        isActive ? 'navbar__link navbar__link--admin navbar__link--active' : 'navbar__link navbar__link--admin';

    return (
        <BrowserRouter>
            <div className="app">
                <nav className="navbar">
                    <Link to="/" className="navbar__brand">
                        BarberOrder
                    </Link>

                    <div className="navbar__links">
                        <NavLink to="/" end className={navLinkClass}>
                            Domů
                        </NavLink>
                        {!isLoggedIn && (
                            <NavLink to="/registrace" className={navLinkClass}>
                                Registrace
                            </NavLink>
                        )}
                        <NavLink to="/rezervace" className={navLinkClass}>
                            Rezervace
                        </NavLink>

                        {!isAdmin ? (
                            <NavLink to="/login" className={navLinkClass}>
                                Přihlášení
                            </NavLink>
                        ) : (
                            <>
                                <NavLink to="/pridat-barbera" className={adminNavLinkClass}>
                                    Přidat barbera
                                </NavLink>
                                <NavLink to="/uzivatele" className={adminNavLinkClass}>
                                    Uživatelé
                                </NavLink>
                                <button type="button" onClick={handleLogout} className="btn-logout">
                                    Odhlásit
                                </button>
                            </>
                        )}
                    </div>
                </nav>

                <main className="main">
                    <Routes>
                        <Route
                            path="/"
                            element={
                                <div className="page-container">
                                    <section className="hero">
                                        <span className="hero__badge">Barber shop</span>
                                        <h1>Vítejte v BarberOrder</h1>
                                        <p>
                                            Rezervujte si termín u vašeho barbera online. Pro administrátory
                                            správa barberů, služeb a uživatelů na jednom místě.
                                        </p>
                                        <div className="hero__actions">
                                            <Link to="/rezervace" className="btn btn--accent">
                                                Rezervovat termín
                                            </Link>
                                            {!isLoggedIn && (
                                                <Link to="/login" className="btn btn--outline">
                                                    Přihlásit se
                                                </Link>
                                            )}
                                        </div>
                                    </section>
                                </div>
                            }
                        />

                        <Route path="/registrace" element={<Register />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/pridat-barbera" element={<AddBarber />} />
                        <Route path="/uzivatele" element={<Users />} />
                        <Route path="/rezervace" element={<Booking />} />
                    </Routes>
                </main>
            </div>
        </BrowserRouter>
    );
}
