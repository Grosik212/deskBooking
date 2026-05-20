'use client';

import { useEffect, useState } from 'react';

type Booking = {
  id: string;
  deskId: string;
  deskName: string;
  userName: string;
  startTime: string;
  endTime: string;
};

type Desk = {
  id: string;
  name: string;
};

export default function BookingsPage() {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [desks, setDesks] = useState<Desk[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  // Form state
  const [selectedDeskId, setSelectedDeskId] = useState('');
  const [userName, setUserName] = useState('');
  const [bookingDate, setBookingDate] = useState(() => new Date().toISOString().split('T')[0]);
  const [startTimeStr, setStartTimeStr] = useState('08:00');
  const [endTimeStr, setEndTimeStr] = useState('16:00');
  const [adding, setAdding] = useState(false);

  const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5058/api';

  const fetchData = async () => {
    setLoading(true);
    try {
      const [bookingsRes, desksRes] = await Promise.all([
        fetch(`${API_URL}/bookings`),
        fetch(`${API_URL}/desks`)
      ]);
      
      if (bookingsRes.ok) setBookings(await bookingsRes.json());
      if (desksRes.ok) {
        const desksData = await desksRes.json();
        setDesks(desksData);
        if (desksData.length > 0 && !selectedDeskId) setSelectedDeskId(desksData[0].id);
      }
      setError('');
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd komunikacji z serwerem');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleAddBooking = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedDeskId || !userName.trim()) return;

    // Wysyłamy dokładnie to, co wybrał użytkownik (bez konwersji do UTC)
    const startTime = `${bookingDate}T${startTimeStr}:00`;
    const endTime = `${bookingDate}T${endTimeStr}:00`;

    setAdding(true);
    setError('');
    try {
      const res = await fetch(`${API_URL}/bookings`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 
          deskId: selectedDeskId,
          userName: userName,
          startTime: startTime,
          endTime: endTime
        })
      });

      if (!res.ok) {
        if (res.status === 409) {
          throw new Error('To biurko jest już zarezerwowane w wybranym terminie!');
        }
        const errorData = await res.json().catch(() => ({}));
        throw new Error(errorData.detail || 'Nie udało się dodać rezerwacji');
      }
      
      setUserName('');
      await fetchData();
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd podczas dodawania');
    } finally {
      setAdding(false);
    }
  };

  const handleCancelBooking = async (id: string) => {
    if (!confirm('Na pewno anulować tę rezerwację?')) return;
    
    try {
      const res = await fetch(`${API_URL}/bookings/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Nie udało się usunąć rezerwacji');
      await fetchData();
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd podczas usuwania');
    }
  };

  const formatDateTime = (dateStr: string) => {
    const d = new Date(dateStr);
    return d.toLocaleString('pl-PL', { 
      day: '2-digit', month: '2-digit', year: 'numeric',
      hour: '2-digit', minute: '2-digit'
    });
  };

  return (
    <div>
      <div className="page-header">
        <h1>Rezerwacje</h1>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div style={{ display: 'flex', gap: '2rem', alignItems: 'flex-start' }}>
        
        {/* Formularz rezerwacji */}
        <div className="glass-card" style={{ width: '380px', position: 'sticky', top: '2rem' }}>
          <h2 style={{ marginBottom: '1.5rem', fontSize: '1.25rem' }}>Nowa rezerwacja</h2>
          
          <form onSubmit={handleAddBooking}>
            <div className="form-group">
              <label className="form-label">Użytkownik (Imię i nazwisko)</label>
              <input 
                type="text" 
                className="form-input" 
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                placeholder="np. Jan Kowalski"
                required
              />
            </div>
            
            <div className="form-group">
              <label className="form-label">Wybierz biurko</label>
              <select 
                className="form-select"
                value={selectedDeskId}
                onChange={(e) => setSelectedDeskId(e.target.value)}
                required
              >
                {desks.map(d => (
                  <option key={d.id} value={d.id}>{d.name}</option>
                ))}
                {desks.length === 0 && <option value="">Brak dostępnych biurek</option>}
              </select>
            </div>

            <div className="form-group">
              <label className="form-label">Data</label>
              <input 
                type="date" 
                className="form-input" 
                value={bookingDate}
                onChange={(e) => setBookingDate(e.target.value)}
                required
              />
            </div>

            <div style={{ display: 'flex', gap: '1rem', marginBottom: '1.5rem' }}>
              <div style={{ flex: 1 }}>
                <label className="form-label">Od godziny</label>
                <input 
                  type="time" 
                  className="form-input" 
                  value={startTimeStr}
                  onChange={(e) => setStartTimeStr(e.target.value)}
                  required
                />
              </div>
              <div style={{ flex: 1 }}>
                <label className="form-label">Do godziny</label>
                <input 
                  type="time" 
                  className="form-input" 
                  value={endTimeStr}
                  onChange={(e) => setEndTimeStr(e.target.value)}
                  required
                />
              </div>
            </div>
            
            <button type="submit" className="btn btn-primary" style={{ width: '100%' }} disabled={adding || desks.length === 0}>
              {adding ? 'Rezerwowanie...' : 'Zarezerwuj biurko'}
            </button>
          </form>
        </div>

        {/* Lista rezerwacji */}
        <div style={{ flex: 1 }}>
          <h2 style={{ marginBottom: '1.5rem', fontSize: '1.25rem' }}>Aktualne rezerwacje</h2>
          {loading ? (
            <div className="empty-state">Ładowanie rezerwacji...</div>
          ) : bookings.length === 0 ? (
            <div className="empty-state">Nikt jeszcze nic nie zarezerwował.</div>
          ) : (
            <div className="list-group">
              {bookings.map(booking => (
                <div key={booking.id} className="list-item">
                  <div>
                    <h3 style={{ fontSize: '1.1rem', marginBottom: '0.25rem' }}>{booking.deskName}</h3>
                    <div style={{ color: 'var(--text-muted)', fontSize: '0.9rem', marginBottom: '0.5rem' }}>
                      Rezerwujący: <strong style={{ color: 'var(--text-main)' }}>{booking.userName}</strong>
                    </div>
                    <div style={{ display: 'flex', gap: '1rem', fontSize: '0.85rem' }}>
                      <span style={{ display: 'flex', alignItems: 'center', gap: '0.25rem' }}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="var(--success-color)" strokeWidth="2"><circle cx="12" cy="12" r="10"></circle><polyline points="12 6 12 12 16 14"></polyline></svg>
                        {formatDateTime(booking.startTime)}
                      </span>
                      <span style={{ display: 'flex', alignItems: 'center', gap: '0.25rem' }}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="var(--danger-color)" strokeWidth="2"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="8" x2="12" y2="12"></line><line x1="12" y1="16" x2="12.01" y2="16"></line></svg>
                        {formatDateTime(booking.endTime)}
                      </span>
                    </div>
                  </div>
                  
                  <button 
                    className="btn btn-danger" 
                    style={{ padding: '0.5rem 1rem' }}
                    onClick={() => handleCancelBooking(booking.id)}
                  >
                    Anuluj
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
