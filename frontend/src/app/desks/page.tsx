'use client';

import { useEffect, useState } from 'react';

type Desk = {
  id: string;
  name: string;
  isStandingDesk: boolean;
  bookingsCount: number;
};

export default function DesksPage() {
  const [desks, setDesks] = useState<Desk[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  const [newDeskName, setNewDeskName] = useState('');
  const [isStanding, setIsStanding] = useState(false);
  const [adding, setAdding] = useState(false);

  const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5058/api';

  const fetchDesks = async () => {
    setLoading(true);
    try {
      const res = await fetch(`${API_URL}/desks`);
      if (!res.ok) throw new Error('Nie udało się pobrać biurek');
      const data = await res.json();
      setDesks(data);
      setError('');
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDesks();
  }, []);

  const handleAddDesk = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newDeskName.trim()) return;

    setAdding(true);
    try {
      const res = await fetch(`${API_URL}/desks`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: newDeskName, isStandingDesk: isStanding })
      });

      if (!res.ok) throw new Error('Nie udało się dodać biurka');
      
      setNewDeskName('');
      setIsStanding(false);
      await fetchDesks();
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd podczas dodawania');
    } finally {
      setAdding(false);
    }
  };

  const handleDeleteDesk = async (id: string) => {
    if (!confirm('Czy na pewno chcesz usunąć to biurko?')) return;
    
    try {
      const res = await fetch(`${API_URL}/desks/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Nie udało się usunąć biurka');
      await fetchDesks();
    } catch (err: any) {
      setError(err.message || 'Wystąpił błąd podczas usuwania');
    }
  };

  return (
    <div>
      <div className="page-header">
        <h1>Zarządzanie Biurkami</h1>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div style={{ display: 'flex', gap: '2rem', alignItems: 'flex-start' }}>
        {/* Lista biurek */}
        <div style={{ flex: 1 }}>
          {loading ? (
            <div className="empty-state">Ładowanie biurek...</div>
          ) : desks.length === 0 ? (
            <div className="empty-state">Brak dodanych biurek. Dodaj pierwsze z menu obok.</div>
          ) : (
            <div className="grid-cards">
              {desks.map(desk => (
                <div key={desk.id} className="glass-card">
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '1rem' }}>
                    <h3 style={{ fontSize: '1.25rem' }}>{desk.name}</h3>
                    <span className={`badge ${desk.isStandingDesk ? 'badge-standing' : 'badge-normal'}`}>
                      {desk.isStandingDesk ? 'Stojące' : 'Standardowe'}
                    </span>
                  </div>
                  
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '1.5rem' }}>
                    <span style={{ color: 'var(--text-muted)', fontSize: '0.9rem' }}>
                      Rezerwacje: {desk.bookingsCount}
                    </span>
                    <button 
                      className="btn btn-danger" 
                      style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem' }}
                      onClick={() => handleDeleteDesk(desk.id)}
                    >
                      Usuń
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Formularz dodawania */}
        <div className="glass-card" style={{ width: '350px', position: 'sticky', top: '2rem' }}>
          <h2 style={{ marginBottom: '1.5rem', fontSize: '1.25rem' }}>Dodaj nowe biurko</h2>
          
          <form onSubmit={handleAddDesk}>
            <div className="form-group">
              <label className="form-label">Nazwa / Numer biurka</label>
              <input 
                type="text" 
                className="form-input" 
                value={newDeskName}
                onChange={(e) => setNewDeskName(e.target.value)}
                placeholder="np. Biurko 101"
                required
              />
            </div>
            
            <div className="form-group">
              <label className="form-checkbox-label">
                <input 
                  type="checkbox" 
                  checked={isStanding}
                  onChange={(e) => setIsStanding(e.target.checked)}
                />
                Biurko do pracy stojącej
              </label>
            </div>
            
            <button type="submit" className="btn btn-primary" style={{ width: '100%' }} disabled={adding}>
              {adding ? 'Dodawanie...' : 'Dodaj biurko'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
