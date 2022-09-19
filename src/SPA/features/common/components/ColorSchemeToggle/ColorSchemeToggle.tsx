import { ActionIcon, Tooltip, useMantineColorScheme, createStyles } from '@mantine/core';
import { SunIcon, MoonIcon } from '@modulz/radix-icons';

const useStyles = createStyles((theme) => ({
  icon: {
    color: theme.colorScheme === 'dark' ? theme.colors.yellow[4] : theme.colors.gray[1],
    '&:hover': {
      backgroundColor: 'rgba(33, 37, 41, 0.2)',
    },
  },
}));

export const ColorSchemeToggle = () => {
  const { classes } = useStyles();
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();

  return (
    <Tooltip label="Toggle dark mode">
      <ActionIcon
        className={classes.icon}
        onClick={() => toggleColorScheme()}
        size="xl"
        radius="xl"
      >
        {colorScheme === 'dark' ? (
          <SunIcon width={20} height={20} />
        ) : (
          <MoonIcon width={20} height={20} />
        )}
      </ActionIcon>
    </Tooltip>
  );
};
