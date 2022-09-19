import React from 'react';
import Link from 'next/link';
import { useRouter } from 'next/router';
import { Home, ListCheck, Table, Sun, User } from 'tabler-icons-react';
import { ActionIcon, UnstyledButton, Group, Text } from '@mantine/core';
import useStyles from './NavLinks.styles';

interface NavLinkProps {
  icon: React.ReactNode;
  href: string;
  label: string;
}

const data: NavLinkProps[] = [
  { icon: <Home />, label: 'Home', href: '/' },
  { icon: <ListCheck />, label: 'Todo Lists', href: '/todolists' },
  { icon: <Table />, label: 'Data Grid', href: '/datagrid' },
  { icon: <Sun />, label: 'Weather Forecast', href: '/weather' },
  { icon: <User />, label: 'Profile', href: '/profile' },
];

function NavLink({ icon, href, label }: NavLinkProps) {
  const { classes } = useStyles();
  const router = useRouter();

  const isActive = (currentPath: string, url: string) => currentPath === url;

  return (
    <Link href={href} passHref>
      <UnstyledButton component="a" className={classes.navButton}>
        <Group>
          <ActionIcon size="xl" variant="hover">
            {icon}
          </ActionIcon>

          <Text
            sx={(theme) => ({
              color: isActive(router.pathname, href)
                ? theme.colors.teal[9]
                : theme.colorScheme === 'dark'
                ? theme.white
                : theme.black,
              fontWeight: isActive(router.pathname, href) ? 'bold' : 'inherit',
            })}
            size="sm"
          >
            {label}
          </Text>
        </Group>
      </UnstyledButton>
    </Link>
  );
}

export function NavLinks() {
  const links = data.map((link) => <NavLink {...link} key={link.label} />);
  return <div>{links}</div>;
}
