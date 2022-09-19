import { MantineTheme } from '@mantine/core';

export const getBackGroundColor = (theme: MantineTheme) =>
  theme.colorScheme === 'dark' ? theme.colors.dark[8] : theme.colors.gray[0];
