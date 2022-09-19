import React, { ReactNode } from 'react';

import { useAuth0 } from '@auth0/auth0-react';
import { Tooltip, ActionIcon, createStyles, Loader } from '@mantine/core';
import { showNotification } from '@mantine/notifications';
import { BrandGithub, Login, Logout } from 'tabler-icons-react';
import { ColorSchemeToggle } from '@components/ColorSchemeToggle/ColorSchemeToggle';

const useStyles = createStyles(() => ({
  hover: {
    '&:hover': {
      backgroundColor: 'rgba(33, 37, 41, 0.2)',
    },
  },
}));

export const AppBarIconList = () => {
  const { classes } = useStyles();

  return (
    <>
      <ColorSchemeToggle />
      <Tooltip label="Visit Repo" withArrow>
        <a href="https://github.com/ForrestTech/Exemplum" target="_blank" rel="noreferrer">
          <ActionIcon size="xl" radius="xl" className={classes.hover}>
            <BrandGithub color="White" />
          </ActionIcon>
        </a>
      </Tooltip>
      <LoginControl />
    </>
  );
};

const LoginControl = () => {
  const { isLoading, isAuthenticated, user, error, loginWithRedirect, logout } = useAuth0();

  const loginFragment = (
    <LoginWrapper toolTip="Login">
      <a onClick={loginWithRedirect} onKeyDown={loginWithRedirect} tabIndex={0} role="button">
        <Login color="White" />
      </a>
    </LoginWrapper>
  );

  if (isLoading) return <Loader />;
  if (error) {
    showNotification({
      title: 'Cant find user details',
      message: 'Can load information about the current user status! 🤥',
      color: 'red',
    });

    return loginFragment;
  }

  if (isAuthenticated) {
    return (
      <LoginWrapper toolTip={`Log out ${user?.name}`}>
        <a
          onClick={() => logout({ returnTo: window.location.origin })}
          onKeyDown={() => logout({ returnTo: window.location.origin })}
          tabIndex={0}
          role="button"
        >
          <Logout color="White" />
        </a>
      </LoginWrapper>
    );
  }
  return loginFragment;
};

interface LoginWrapperProps {
  toolTip: string;
  children: ReactNode;
}

const LoginWrapper = ({ toolTip, children }: LoginWrapperProps) => {
  const { classes } = useStyles();

  return (
    <Tooltip label={toolTip} withArrow>
      <ActionIcon size="xl" radius="xl" className={classes.hover}>
        {children}
      </ActionIcon>
    </Tooltip>
  );
};
