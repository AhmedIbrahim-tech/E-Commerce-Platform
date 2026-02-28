'use client'
import { useCustomizer } from '@/hooks/useCustomizer';
import Link from "next/link";
import { styled } from "@mui/material/styles";
import config from '@/config/config'
import Image from "next/image";
import { useContext } from "react";

const Logo = () => {
  const { isCollapse, isSidebarHover, activeDir, activeMode } = useCustomizer();

  const TopbarHeight = config.topbarHeight;

  const LinkStyled = styled(Link)(() => ({
    height: TopbarHeight,

    width: isCollapse == "mini-sidebar" && !isSidebarHover ? '40px' : '180px',
    overflow: "hidden",
    display: "block",
  }));

  if (activeDir === "ltr") {
    return (
      <LinkStyled href="/">
        {activeMode === "dark" ? (
          <Image
            src="/images/logos/light-logo.svg"
            alt="logo"
            height={TopbarHeight}
            width={174}
            priority
          />
        ) : (
          <Image
            src={"/images/logos/dark-logo.svg"}
            alt="logo"
            height={TopbarHeight}
            width={174}
            priority
          />
        )}
      </LinkStyled>
    );
  }

  return (
    <LinkStyled href="/">
      {activeMode === "dark" ? (
        <Image
          src="/images/logos/dark-rtl-logo.svg"
          alt="logo"
          height={TopbarHeight}
          width={174}
          priority
        />
      ) : (
        <Image
          src="/images/logos/light-logo-rtl.svg"
          alt="logo"
          height={TopbarHeight}
          width={174}
          priority
        />
      )}
    </LinkStyled>
  );
};

export default Logo;
