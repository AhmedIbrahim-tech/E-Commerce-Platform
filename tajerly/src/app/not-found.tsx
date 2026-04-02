'use client';

import Link from 'next/link';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { Home } from 'lucide-react';

export default function NotFound() {
    return (
        <div
            className="d-flex align-items-center justify-content-center min-vh-100"
            style={{
                background: 'var(--tj-bg)',
                color: 'var(--tj-text-primary)',
                transition: 'background 0.3s'
            }}
        >
            <Container>
                <Row className="justify-content-center text-center">
                    <Col lg={7} md={9}>
                        {/* Artwork */}
                        <div className="mb-4 animate-scale-in">
                            <img
                                src="/images/error400-cover.png"
                                alt="404 Error"
                                className="img-fluid"
                                style={{
                                    maxHeight: '380px',
                                    filter: 'drop-shadow(0 0 20px rgba(0,0,0,0.15))'
                                }}
                            />
                        </div>

                        {/* Heading */}
                        <h2
                            className="fw-bold mb-2 animate-fade-in"
                            style={{
                                color: 'var(--tj-text-primary)',
                                fontSize: '1.75rem',
                                letterSpacing: '-0.02em'
                            }}
                        >
                            Oops, something went wrong
                        </h2>

                        {/* Subtext */}
                        <p
                            className="mb-4 animate-fade-in"
                            style={{
                                color: 'var(--tj-text-muted)',
                                fontSize: '0.875rem',
                                maxWidth: '460px',
                                margin: '0 auto',
                                lineHeight: '1.6'
                            }}
                        >
                            Error 404 Page not found. Sorry the page you looking for doesn&apos;t exist or has been moved
                        </p>

                        {/* CTA Button */}
                        <div className="animate-slide-down" style={{ animationDelay: '0.2s' }}>
                            <Link href="/" passHref>
                                <Button
                                    className="px-4 py-2 rounded-2 fw-bold d-inline-flex align-items-center gap-2"
                                    style={{
                                        background: 'var(--tj-accent)',
                                        borderColor: 'var(--tj-accent)',
                                        color: 'var(--tj-accent-foreground)',
                                        textTransform: 'capitalize',
                                        fontSize: '0.875rem'
                                    }}
                                >
                                    <Home size={16} />
                                    Back to Home
                                </Button>
                            </Link>
                        </div>
                    </Col>
                </Row>
            </Container>
        </div>
    );
}
