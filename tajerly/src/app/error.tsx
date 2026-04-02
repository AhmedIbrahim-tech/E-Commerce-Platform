'use client';

import { useEffect } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { AlertTriangle, RotateCcw, Home } from 'lucide-react';
import Link from 'next/link';

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    // Optionally log the error to an error reporting service
    console.error('Unhandled runtime error:', error);
  }, [error]);

  return (
    <div 
        className="d-flex align-items-center justify-content-center min-vh-100 py-5"
        style={{ 
            background: 'var(--tj-bg)', 
            color: 'var(--tj-text-primary)' 
        }}
    >
      <Container>
        <Row className="justify-content-center text-center">
          <Col lg={6} md={8} className="animate-scale-in">
            {/* Error Icon */}
            <div 
                className="d-inline-flex p-4 rounded-circle mb-4" 
                style={{ background: 'var(--tj-danger-soft)' }}
            >
              <AlertTriangle size={64} style={{ color: 'var(--tj-danger)' }} />
            </div>

            {/* Error Message */}
            <h1 className="fw-black mb-3" style={{ fontSize: '2.5rem', letterSpacing: '-0.025em' }}>
              Unexpected Glitch
            </h1>
            <p 
                className="mb-5" 
                style={{ 
                    color: 'var(--tj-text-muted)', 
                    fontSize: '1.125rem' 
                }}
            >
              Something went wrong on our end. We&apos;ve been notified and are working to fix it.
            </p>

            {/* Actions */}
            <div className="d-flex flex-column flex-sm-row align-items-center justify-content-center gap-3">
              <Button 
                variant="primary" 
                size="lg" 
                className="px-4 py-3 d-flex align-items-center gap-2"
                onClick={() => reset()}
              >
                <RotateCcw size={20} />
                Try Again
              </Button>
              <Link href="/" passHref>
                <Button 
                  variant="outline-primary" 
                  size="lg" 
                  className="px-4 py-3 d-flex align-items-center gap-2"
                >
                  <Home size={20} />
                  Return Home
                </Button>
              </Link>
            </div>

            {/* Error Details for Admins / Developers */}
            {process.env.NODE_ENV === 'development' && (
              <div 
                className="mt-5 p-3 rounded-3 text-start overflow-auto" 
                style={{ 
                    background: 'var(--tj-bg-secondary)', 
                    maxHeight: '200px', 
                    fontSize: '0.75rem',
                    border: '1px solid var(--tj-border)' 
                }}
              >
                <code style={{ color: 'var(--tj-danger)' }}>{error.message}</code>
                <pre className="mt-2" style={{ color: 'var(--tj-text-muted)' }}>{error.stack}</pre>
              </div>
            )}
          </Col>
        </Row>
      </Container>
    </div>
  );
}
