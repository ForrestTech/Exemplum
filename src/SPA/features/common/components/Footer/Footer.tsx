import { Divider, Grid, Text, Title } from '@mantine/core';
import useStyles from './Footer.styles';

export const Footer = () => {
  const { classes } = useStyles();

  return (
    <>
      <Divider my="sm" />
      <Grid className={classes.footer}>
        <Grid.Col span={3}>
          <Title order={1}>Exemplum</Title>
        </Grid.Col>
        <Grid.Col span={3}>
          <Text>Community</Text>
          <ul>
            <li>
              <a href="https://discord.gg/exemplum" target="_blank" rel="noreferrer">
                Discord
              </a>
            </li>
            <li>
              <a
                href="https://github.com/ForrestTech/Exemplum/discussions"
                target="_blank"
                rel="noreferrer"
              >
                GitHub Discussions
              </a>
            </li>
          </ul>
        </Grid.Col>
        <Grid.Col span={3}>
          <Text>Contact</Text>
          <ul>
            <li>
              <a href="https://discord.gg/mudblazor" target="_blank" rel="noreferrer">
                Discord
              </a>
            </li>
            <li>
              <a href="mailto:richard.a.forrest@gmail.com">Richard Forrest</a>
            </li>
          </ul>
        </Grid.Col>
        <Grid.Col span={3}>
          <Text>Project</Text>
          <ul>
            <li>
              <a href="/project/about">About</a>
            </li>
            <li>
              <a href="/project/credit">Credits</a>
            </li>
          </ul>
        </Grid.Col>
        <Grid.Col span={12}>
          <Text className={classes.copyText}>
            Currently v0.1. Released under the{' '}
            <a
              href="https://github.com/MudBlazor/MudBlazor/blob/master/LICENSE"
              target="_blank"
              rel="noreferrer"
            >
              MIT License.
            </a>{' '}
            Copyright © 2020-2021 Forrest Tech.
          </Text>
        </Grid.Col>
      </Grid>
    </>
  );
};
