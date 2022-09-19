import { createStyles } from '@mantine/core';

export default createStyles((theme) => ({
  welcome: {
    marginBottom: '120px',
  },
  button: {
    width: '200px',
    padding: '12px 0',
    margin: '0 8px',
    boxShadow: theme.shadows.lg,
  },
  getStarted: {
    background: `transparent linear-gradient(90deg, ${theme.colors.teal[9]}  0%, ${theme.colors.teal[5]} 100%) 0% 0% no-repeat padding-box`,
  },
  gitHub: {
    background: `transparent linear-gradient(90deg, ${theme.colors.dark[4]}  0%, ${theme.colors.dark[9]} 100%) 0% 0% no-repeat padding-box`,
  },
  titles: {
    letterSpacing: '0.1rem',
    fontSize: '3rem',
    fontWeight: 300,
    color: theme.colors.teal[7],
  },
  smaller: {
    fontSize: '2.5rem',
  },
  description: {
    fontSize: theme.fontSizes.lg,
    fontWeight: 300,
    color: theme.colorScheme === 'dark' ? theme.white : theme.black,
  },
}));
