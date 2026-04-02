'use client';

import { Container, Row, Col, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { EmptyState } from '@/components/shared/EmptyState';
import { MapPin, Plus, Edit, Trash2, Star } from 'lucide-react';

const mockAddresses = [
    { id: 1, label: 'Home', fullName: 'Ahmed Ibrahim', phone: '+20 1234 567 890', addressLine1: '123 Nile Street, Zamalek', city: 'Cairo', country: 'Egypt', isDefault: true },
    { id: 2, label: 'Work', fullName: 'Ahmed Ibrahim', phone: '+20 1234 567 891', addressLine1: '456 Smart Village, 6th October', city: 'Giza', country: 'Egypt', isDefault: false },
];

export default function AddressesPage() {
    return (
        <Container className="py-4">
            <PageHeader
                title="My Addresses"
                subtitle="Manage your shipping addresses"
                breadcrumbs={[{ label: 'Addresses' }]}
                action={<Button variant="primary" className="d-flex align-items-center gap-2"><Plus size={16} /> Add Address</Button>}
            />

            {mockAddresses.length === 0 ? (
                <EmptyState icon={MapPin} title="No addresses saved" description="Add a shipping address to speed up checkout." actionLabel="Add Address" />
            ) : (
                <Row className="g-4">
                    {mockAddresses.map((address) => (
                        <Col key={address.id} lg={6}>
                            <div className="tj-card p-4 position-relative">
                                {address.isDefault && (
                                    <div className="position-absolute top-0 end-0 m-3 d-flex align-items-center gap-1 px-2 py-1 rounded" style={{ background: 'var(--tj-accent-soft)', color: 'var(--tj-accent)', fontSize: '0.75rem', fontWeight: 600 }}>
                                        <Star size={12} /> Default
                                    </div>
                                )}
                                <div className="d-flex align-items-start gap-3">
                                    <div className="p-2 rounded-3" style={{ background: 'var(--tj-accent-soft)' }}>
                                        <MapPin size={20} style={{ color: 'var(--tj-accent)' }} />
                                    </div>
                                    <div className="flex-grow-1">
                                        <h6 className="fw-bold mb-1" style={{ color: 'var(--tj-text-primary)' }}>{address.label}</h6>
                                        <p className="mb-1" style={{ color: 'var(--tj-text-secondary)', fontSize: '0.875rem' }}>{address.fullName}</p>
                                        <p className="mb-1" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>{address.addressLine1}</p>
                                        <p className="mb-1" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>{address.city}, {address.country}</p>
                                        <p className="mb-0" style={{ color: 'var(--tj-text-muted)', fontSize: '0.875rem' }}>{address.phone}</p>
                                    </div>
                                </div>
                                <div className="d-flex gap-2 mt-3 pt-3" style={{ borderTop: '1px solid var(--tj-border)' }}>
                                    <Button variant="outline-primary" size="sm" className="d-flex align-items-center gap-1"><Edit size={14} /> Edit</Button>
                                    <Button variant="outline-danger" size="sm" className="d-flex align-items-center gap-1"><Trash2 size={14} /> Remove</Button>
                                </div>
                            </div>
                        </Col>
                    ))}
                </Row>
            )}
        </Container>
    );
}
