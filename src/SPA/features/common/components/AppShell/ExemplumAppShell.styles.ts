import { createStyles } from '@mantine/core';
import { getBackGroundColor } from '@features/common/styles/styles';

const hoverColor = 'rgba(33, 37, 41, 0.2)';

export default createStyles((theme) => ({
  appShell: {
    transition: 'padding-left 600ms ease',
    background: getBackGroundColor(theme),
  },
  headerContainer: {
    background: theme.colorScheme === 'dark' ? theme.colors.dark[8] : theme.colors.teal[7],
    boxShadow: theme.shadows.md,
  },
  header: {
    display: 'flex',
    alignItems: 'center',
    height: '100%',
  },
  headerTitle: {
    letterSpacing: '0.5rem',
    fontWeight: 300,
    marginLeft: '12px',
    fontSize: '24px',
    userSelect: 'none',
    color: theme.white,
  },
  navBar: {
    overflow: 'hidden',
    transition: 'width 600ms ease, min-width 600ms ease',
    background: getBackGroundColor(theme),
    boxShadow: theme.shadows.md,
  },
  supportMenu: {
    cursor: 'pointer',
    marginRight: '5px',
    paddingLeft: '8px',
    borderRadius: '5px',
    paddingBottom: '5px',
    '&:hover': {
      backgroundColor: hoverColor,
    },
  },
  burger: {
    marginRight: '10px',
    padding: '5px',
    borderRadius: '5px',
    '&:hover': {
      backgroundColor: hoverColor,
    },
  },
}));
