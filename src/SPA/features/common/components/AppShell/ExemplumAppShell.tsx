import { useState } from 'react';
import { AppProps } from 'next/app';
import Link from 'next/link';
import {
  AppShell,
  Navbar,
  Header,
  Text,
  Burger,
  useMantineTheme,
  Divider,
  Menu,
  Anchor,
} from '@mantine/core';
import { ChevronDown } from 'tabler-icons-react';
import { NavLinks } from '@components/NavLinks/NavLinks';
import { AppBarIconList } from '@features/common/components/AppBarIconsList/AppBarIconList';

import useStyles from './ExemplumAppShell.styles';

interface ExemplumAppShellProps {
  appProps: AppProps;
}

export const ExemplumAppShell = ({ appProps }: ExemplumAppShellProps) => {
  const { Component, pageProps } = appProps;

  const { classes } = useStyles();
  const theme = useMantineTheme();

  const [opened, setOpened] = useState(false);

  return (
    <AppShell
      padding="md"
      fixed
      className={classes.appShell}
      navbar={
        <Navbar
          p="sm"
          className={classes.navBar}
          width={{ sm: opened ? 300 : 0 }}
          style={{
            visibility: opened ? 'visible' : 'hidden',
          }}
        >
          <Navbar.Section grow mt="md">
            <NavLinks />
          </Navbar.Section>
        </Navbar>
      }
      header={
        <Header height={70} p="md" className={classes.headerContainer}>
          <div className={classes.header}>
            <Burger
              opened={opened}
              onClick={() => setOpened((o) => !o)}
              size="sm"
              color={theme.colors.gray[0]}
              className={classes.burger}
              mr="xl"
            />
            <Link href="/" passHref>
              <Text className={classes.headerTitle} style={{ cursor: 'pointer' }}>
                Exemplum
              </Text>
            </Link>
            <div style={{ flexGrow: 1 }} />
            <Menu
              control={
                <div className={classes.supportMenu}>
                  <Text color="white" component="span">
                    SUPPORT
                  </Text>
                  <ChevronDown
                    color="white"
                    style={{ marginLeft: '5px', paddingTop: '10px', paddingRight: '5px' }}
                  />
                </div>
              }
            >
              <Menu.Item>
                <Menu.Label>Community Support</Menu.Label>
              </Menu.Item>
              <Menu.Item>
                <Anchor href="https://github.com/ForrestTech/Exemplum/discussions" target="_blank">
                  {' '}
                  Github Discussions{' '}
                </Anchor>
              </Menu.Item>
            </Menu>
            <Divider
              style={{ height: '30px', marginTop: '3px' }}
              orientation="vertical"
              size="sm"
            />
            <AppBarIconList />
          </div>
        </Header>
      }
    >
      <Component {...pageProps} />
    </AppShell>
  );
};
