import { StorefrontHeader } from '@/components/layout/StorefrontHeader';
import { StorefrontFooter } from '@/components/layout/StorefrontFooter';

export default function StorefrontLayout({ children }: { children: React.ReactNode }) {
    return (
        <>
            <StorefrontHeader />
            <main style={{ paddingTop: 80, minHeight: '100vh' }}>
                {children}
            </main>
            <StorefrontFooter />
        </>
    );
}
