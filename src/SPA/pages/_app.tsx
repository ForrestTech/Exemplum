import { useState, useEffect } from 'react';
import { AppProps } from 'next/app';
import { getCookie, setCookies } from 'cookies-next';
import {
  MantineProvider,
  ColorScheme,
  ColorSchemeProvider,
  MantineThemeOverride,
} from '@mantine/core';
import { NotificationsProvider } from '@mantine/notifications';
import { Auth0Provider } from '@auth0/auth0-react';
import { ExemplumSwrConfig } from '@features/common/SwrConfig/ExemplumSwrConfig';
import { ExemplumHead } from '@components/Head/ExemplumHead';
import { ExemplumAppShell } from '@components/AppShell/ExemplumAppShell';

import '../public/global.css';

const exemplumDefaultTheme: MantineThemeOverride = {
  colorScheme: 'light',
  primaryColor: 'teal',
  defaultRadius: 0,
};

export default function App(props: AppProps) {
  const [theme] = useState<MantineThemeOverride>(exemplumDefaultTheme);
  const [colorScheme, setColorScheme] = useState<ColorScheme>('light');

  const toggleColorScheme = (value?: ColorScheme) => {
    const nextColorScheme = value || (colorScheme === 'dark' ? 'light' : 'dark');
    setColorScheme(nextColorScheme);
    setCookies('mantine-color-scheme', nextColorScheme, { maxAge: 60 * 60 * 24 * 30 });
    theme.colorScheme = nextColorScheme;
  };

  useEffect(() => {
    if (typeof window !== 'undefined') {
      const cookie = getCookie('mantine-color-scheme') || 'light';
      const cookieTheme = cookie as ColorScheme;
      setColorScheme(cookieTheme);
      theme.colorScheme = cookieTheme;
    }
  }, []);

  return (
    <>
      <ExemplumHead />
      <Auth0Provider
        domain={process.env.NEXT_PUBLIC_AUTH0_ISSUER_BASE_URL || ''}
        clientId={process.env.NEXT_PUBLIC_AUTH0_CLIENT_ID || ''}
        redirectUri={process.env.NEXT_PUBLIC_TODO_HOST_BASE_URL}
      >
        <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
          <MantineProvider theme={theme} withGlobalStyles withNormalizeCSS>
            <NotificationsProvider>
              <ExemplumSwrConfig>
                <ExemplumAppShell appProps={props} />
              </ExemplumSwrConfig>
            </NotificationsProvider>
          </MantineProvider>
        </ColorSchemeProvider>
      </Auth0Provider>
    </>
  );
}
