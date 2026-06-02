import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
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
        setIsAdmin(hasAdminRole());
        setIsLoggedIn(Boolean(localStorage.getItem('token')));
    }, []);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        setIsAdmin(false);
        setIsLoggedIn(false);
        window.location.href = '/login';
    };

    return (
        <BrowserRouter>
            <div>
                <nav style={navStyle}>
                    <span style={{ fontSize: '1.2rem', fontWeight: 'bold', marginRight: '1rem' }}>
                        BarberOrder
                    </span>
                    
                    <Link to="/" style={linkStyle}>Domů</Link>
                    {!isLoggedIn && <Link to="/registrace" style={linkStyle}>Registrace</Link>}
                    <Link to="/rezervace" style={linkStyle}>Rezervace</Link> 
                    
                    {!isAdmin ? (
                        <Link to="/login" style={linkStyle}>Přihlášení</Link>
                    ) : (
                        <>
                            <Link to="/pridat-barbera" style={adminLinkStyle}>Přidat barbera</Link>
                            <Link to="/uzivatele" style={adminLinkStyle}>Uživatelé</Link>
                            <button onClick={handleLogout} style={logoutButtonStyle}>
                                Odhlásit
                            </button>
                        </>
                    )}
                </nav>

                <main style={{ padding: '2rem' }}>
                    <Routes>
                        <Route path="/" element={
                            <div>
                                <h1>Vítejte na stránce BarberOrder</h1>
                                <p>Systém pro správu vašeho barber shopu.</p>
                            </div>
                        } />

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

const navStyle = { 
    padding: '1rem', 
    backgroundColor: '#333', 
    color: '#fff', 
    display: 'flex', 
    alignItems: 'center' 
};

const linkStyle = {
    marginLeft: '1rem',
    color: '#fff',
    textDecoration: 'none'
};

const adminLinkStyle = {
    marginLeft: '1rem',
    color: '#ffcc00', 
    textDecoration: 'none',
    fontWeight: 'bold'
};


const logoutButtonStyle = {
    marginLeft: 'auto',
    backgroundColor: 'transparent',
    color: '#ff4444',
    border: '1px solid #ff4444',
    cursor: 'pointer',
    padding: '0.3rem 0.8rem',
    borderRadius: '4px'
};