'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';

export default function Dashboard() {
  const [stats, setStats] = useState({ desks: 0, bookings: 0 });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchStats() {
      try {
        const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5058/api';
        
        const [desksRes, bookingsRes] = await Promise.all([
          fetch(`${apiUrl}/desks`).catch(() => null),
          fetch(`${apiUrl}/bookings`).catch(() => null)
        ]);
        
        let desksCount = 0;
        let bookingsCount = 0;

        if (desksRes?.ok) {
          const desks = await desksRes.json();
          desksCount = desks.length;
        }
        
        if (bookingsRes?.ok) {
          const bookings = await bookingsRes.json();
          bookingsCount = bookings.length;
        }

        setStats({ desks: desksCount, bookings: bookingsCount });
      } catch (err) {
        console.error("Failed to fetch stats", err);
      } finally {
        setLoading(false);
      }
    }

    fetchStats();
  }, []);

  return (
    <div>
      <div className="page-header">
        <h1>Dashboard</h1>
      </div>

      <div className="grid-cards">
        <div className="glass-card">
          <h3 style={{ color: 'var(--text-muted)', marginBottom: '0.5rem', fontWeight: 500 }}>Wszystkie biurka</h3>
          <div style={{ fontSize: '3rem', fontWeight: 700, color: 'var(--primary-color)' }}>
            {loading ? '...' : stats.desks}
          </div>
          <Link href="/desks" style={{ color: 'var(--text-muted)', fontSize: '0.9rem', marginTop: '1rem', display: 'inline-block', textDecoration: 'underline' }}>
            Zarządzaj biurkami &rarr;
          </Link>
        </div>

        <div className="glass-card">
          <h3 style={{ color: 'var(--text-muted)', marginBottom: '0.5rem', fontWeight: 500 }}>Wszystkie rezerwacje</h3>
          <div style={{ fontSize: '3rem', fontWeight: 700, color: '#10b981' }}>
            {loading ? '...' : stats.bookings}
          </div>
          <Link href="/bookings" style={{ color: 'var(--text-muted)', fontSize: '0.9rem', marginTop: '1rem', display: 'inline-block', textDecoration: 'underline' }}>
            Zobacz rezerwacje &rarr;
          </Link>
        </div>
      </div>
      
      <div className="glass-card" style={{ marginTop: '2rem' }}>
        <h2>Witaj w systemie deskBooking!</h2>
        <p style={{ color: 'var(--text-muted)', marginTop: '1rem', lineHeight: 1.6 }}>
          Wybierz opcję z menu po lewej stronie, aby zarządzać biurkami w swoim biurze lub sprawdzić rezerwacje. <br/>
          Zwróć uwagę, że system automatycznie blokuje możliwość rezerwacji jednego biurka przez wiele osób w tym samym czasie.
        </p>
      </div>
    </div>
  );
}
