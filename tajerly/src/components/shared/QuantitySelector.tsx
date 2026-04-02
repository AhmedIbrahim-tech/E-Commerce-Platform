'use client';

import { Minus, Plus } from 'lucide-react';
import { Button } from 'react-bootstrap';

interface QuantitySelectorProps {
    value: number;
    min?: number;
    max?: number;
    onChange: (value: number) => void;
}

export function QuantitySelector({ value, min = 1, max = 99, onChange }: QuantitySelectorProps) {
    return (
        <div className="d-flex align-items-center gap-2 border rounded-3 px-2 py-1" style={{ borderColor: 'var(--tj-border)' }}>
            <Button
                variant="link"
                size="sm"
                className="p-1"
                disabled={value <= min}
                onClick={() => onChange(Math.max(min, value - 1))}
                style={{ color: 'var(--tj-text-muted)' }}
            >
                <Minus size={16} />
            </Button>
            <span className="fw-semibold px-2" style={{ minWidth: 32, textAlign: 'center', color: 'var(--tj-text-primary)', fontSize: '0.9rem' }}>
                {value}
            </span>
            <Button
                variant="link"
                size="sm"
                className="p-1"
                disabled={value >= max}
                onClick={() => onChange(Math.min(max, value + 1))}
                style={{ color: 'var(--tj-text-muted)' }}
            >
                <Plus size={16} />
            </Button>
        </div>
    );
}
