import { createStyles } from '@mantine/core';

export default createStyles((theme) => ({
  footer: {
    padding: '48px 0',
    marginBottom: '-64px',
    h1: {
      fontWeight: 400,
      letterSpacing: '0.4rem',
      lineHeight: '1.167',
      fontSize: '1rem',
    },
    ul: {
      listStyle: 'none',
      margin: 0,
      padding: 0,
      li: {
        padding: '6px 0',
      },
      a: {
        color: theme.colors.teal[7],
      },
    },
  },
  copyText: {
    paddingTop: '50px',
    textAlign: 'center',
    a: {
      color: theme.colors.teal[7],
    },
  },
}));
