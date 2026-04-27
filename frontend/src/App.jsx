import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import Register from './pages/Register'; // Předpokládám, že tvůj formulář je v tomto souboru

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
                    {/* Přidal jsem odkaz domů, aby ses mohl vracet */}
                    <Link to="/" style={{ marginLeft: '1rem', color: '#ccc', textDecoration: 'none' }}>Domů</Link>
                </nav>

                <main style={{ padding: '2rem' }}>
                    {/* TADY JE TA CHYBĚJÍCÍ ČÁST: */}
                    <Routes>
                        {/* Cesta pro hlavní stránku */}
                        <Route path="/" element={
                            <div>
                                <h1>Vítejte na stránce BarberOrder</h1>
                                <p>Pro registraci uživatele klikněte v navigaci na odkaz "Registrace uživatele".</p>
                            </div>
                        } />

                        {/* Cesta pro registraci */}
                        <Route path="/registrace" element={<Register />} />
                    </Routes>
                </main>
            </div>
        </BrowserRouter>
    )
}