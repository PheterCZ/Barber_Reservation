import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import Register from './pages/Register';
import Login from './pages/Login';
import AddBarber from './pages/AddBarber';

export default function App() {
    return (
        <BrowserRouter>
            <div>
                <nav style={{ padding: '1rem', backgroundColor: '#333', color: '#fff' }}>
                    <span style={{ fontSize: '1.2rem', fontWeight: 'bold' }}>BarberOrder</span>
                    <Link
                        to="/registrace"
                        style={{
                            marginLeft: '2rem',
                            color: '#fff',
                            textDecoration: 'none',
                            fontWeight: '500'
                        }}
                    >
                        Registrace uživatele
                    </Link>
                    <Link
                        to="/login"
                        style={{
                            marginLeft: '1rem',
                            color: '#fff',
                            textDecoration: 'none',
                            fontWeight: '500'
                        }}
                    >
                        Přihlášení
                    </Link>
                    <Link
                        to="/pridat-barbera"
                        style={{
                            marginLeft: '1rem',
                            color: '#fff',
                            textDecoration: 'none',
                            fontWeight: '500'
                        }}
                    >
                        Přidat barbera
                    </Link>
                    <Link to="/" style={{ marginLeft: '1rem', color: '#ccc', textDecoration: 'none' }}>Domů</Link>
                </nav>

                <main style={{ padding: '2rem' }}>
                    <Routes>
                        <Route path="/" element={
                            <div>
                                <h1>Vítejte na stránce BarberOrder</h1>
                                <p>Pro registraci uživatele klikněte v navigaci na odkaz "Registrace uživatele".</p>
                            </div>
                        } />

                        <Route path="/registrace" element={<Register />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/pridat-barbera" element={<AddBarber />} />
                    </Routes>
                </main>
            </div>
        </BrowserRouter>
    )
}