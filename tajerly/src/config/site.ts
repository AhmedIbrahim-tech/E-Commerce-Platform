/**
 * Central site branding and metadata.
 * Update name, tagline, and icon paths here to reflect across the app.
 */
export const siteConfig = {
  /** Main product name (tab, sidebar, login, footer) */
  name: "Tajerly",
  /** Short tagline for page titles */
  tagline: "Your Premium Shopping Destination",
  /** Full title for default tab and SEO */
  get fullTitle() {
    return `${this.name} | ${this.tagline}`;
  },
  /** SEO title variant (e.g. for Open Graph) */
  seoTitle: "Tajerly | Premium E-Commerce Platform",
  /** Default meta description */
  description:
    "Discover and shop premium products — curated categories, seamless checkout, and a personalized shopping experience.",
  /** Current year (dynamic) */
  get year() {
    return new Date().getFullYear();
  },
  /** Copyright text for footers */
  get copyright() {
    return `© ${this.year} Tajerly. All rights reserved.`;
  },
  /** Footer variant (e.g. login page) */
  get copyrightPlatform() {
    return `© ${this.year} Tajerly Platform`;
  },
  /** 404 / error page brand label */
  brandEngine: "Tajerly Engine",
  /** Icon paths */
  icons: {
    icon: "/icon.svg",
    apple: "/icon.png",
  },
  /** OG/Twitter image */
  ogImage: "/images/tajerly-banner.png",
};
