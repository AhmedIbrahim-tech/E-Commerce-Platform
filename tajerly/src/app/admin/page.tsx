'use client';

import { Row, Col, Badge, Button } from 'react-bootstrap';
import { PageHeader } from '@/components/shared/PageHeader';
import { StatsCard } from '@/components/shared/StatsCard';
import { Package, ShoppingCart, Users, DollarSign, TrendingUp, Clock, ArrowUpRight } from 'lucide-react';
import { formatPrice } from '@/utils/formatters';
import { useTranslation } from 'react-i18next';

export default function AdminDashboardPage() {
    const { t } = useTranslation();

    return (
        <div className="animate-fade-in">
            <PageHeader 
                title={t('admin.dashboard')} 
                subtitle={t('admin.overview')} 
            />

            <Row className="g-4 mb-4">
                <Col lg={3} md={6}>
                    <StatsCard title={t('admin.totalRevenue')} value={formatPrice(125840)} icon={DollarSign} color="primary" trend={{ value: '+12.5%', isUp: true }} />
                </Col>
                <Col lg={3} md={6}>
                    <StatsCard title={t('admin.totalOrders')} value="1,248" icon={ShoppingCart} color="success" trend={{ value: '+8.2%', isUp: true }} />
                </Col>
                <Col lg={3} md={6}>
                    <StatsCard title={t('admin.totalProducts')} value="356" icon={Package} color="info" trend={{ value: '+15', isUp: true }} />
                </Col>
                <Col lg={3} md={6}>
                    <StatsCard title={t('admin.totalUsers')} value="4,128" icon={Users} color="warning" trend={{ value: '+241', isUp: true }} />
                </Col>
            </Row>

            <Row className="g-4">
                <Col lg={8}>
                    <div className="tj-card p-4 h-100 position-relative overflow-hidden">
                        <div className="d-flex justify-content-between align-items-center mb-4">
                            <h6 className="fw-bold mb-0 text-primary-emphasis">{t('admin.revenueOverview')}</h6>
                            <Badge bg="primary-soft" className="px-3 py-2 rounded-pill text-primary" style={{ fontSize: '0.7rem' }}>
                                <Clock size={12} className="me-1" /> Last 30 Days
                            </Badge>
                        </div>
                        
                        {/* Premium Simulated Chart */}
                        <div className="position-relative" style={{ height: 300 }}>
                            <div className="d-flex align-items-end justify-content-between h-100 pt-4 px-2">
                                {[40, 70, 45, 90, 65, 85, 100].map((h, i) => (
                                    <div key={i} className="flex-grow-1 mx-2 position-relative group" style={{ height: `${h}%` }}>
                                        <div 
                                            className="w-100 rounded-top-2 transition-all" 
                                            style={{ 
                                                height: '100%', 
                                                background: 'linear-gradient(to top, var(--tj-accent), var(--tj-accent-soft))',
                                                opacity: 0.8,
                                                cursor: 'pointer'
                                            }} 
                                        />
                                        <div className="position-absolute top-0 start-50 translate-middle-x mb-2 opacity-0 group-hover-opacity-100 transition-all bg-dark text-white px-2 py-1 rounded small" style={{ zIndex: 10 }}>
                                            {formatPrice(h * 1000)}
                                        </div>
                                    </div>
                                ))}
                            </div>
                            <div className="position-absolute bottom-0 w-100 d-flex justify-content-between text-muted x-small mt-2 border-top pt-2">
                                <span>Mon</span><span>Tue</span><span>Wed</span><span>Thu</span><span>Fri</span><span>Sat</span><span>Sun</span>
                            </div>
                        </div>
                    </div>
                </Col>
                <Col lg={4}>
                    <div className="tj-card p-4 h-100">
                        <h6 className="fw-bold mb-4 text-primary-emphasis">{t('admin.recentOrders')}</h6>
                        <div className="d-flex flex-column gap-3">
                            {[
                                { order: 'TJ-2025-003', customer: 'Sara Ali', amount: 849.99, status: 'Pending', time: '2m ago' },
                                { order: 'TJ-2025-002', customer: 'Mohamed Hassan', amount: 1299.99, status: 'Shipped', time: '1h ago' },
                                { order: 'TJ-2025-001', customer: 'Ahmed Ibrahim', amount: 3799.98, status: 'Delivered', time: '5h ago' },
                            ].map((o, i) => (
                                <div key={i} className="p-3 rounded-3 hover-bg-light transition-all border border-transparent hover-border-light cursor-pointer">
                                    <div className="d-flex justify-content-between align-items-start mb-2">
                                        <div>
                                            <p className="mb-0 fw-bold small text-primary-emphasis">{o.order}</p>
                                            <p className="mb-0 x-small text-muted">{o.time}</p>
                                        </div>
                                        <Badge bg={o.status === 'Pending' ? 'warning-soft' : o.status === 'Shipped' ? 'info-soft' : 'success-soft'} className={`px-2 py-1 rounded-pill ${o.status === 'Pending' ? 'text-warning' : o.status === 'Shipped' ? 'text-info' : 'text-success'}`} style={{ fontSize: '0.65rem' }}>
                                            {o.status}
                                        </Badge>
                                    </div>
                                    <div className="d-flex justify-content-between align-items-center">
                                        <span className="small text-secondary">{o.customer}</span>
                                        <span className="fw-bold text-accent small">{formatPrice(o.amount)}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                        <Button variant="link" className="w-100 mt-3 text-decoration-none small fw-bold text-muted d-flex align-items-center justify-content-center gap-1">
                            View All <ArrowUpRight size={14} />
                        </Button>
                    </div>
                </Col>
            </Row>
        </div>
    );
}
