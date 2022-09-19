import clsx from 'clsx';
import Link from 'next/link';
import { Text, Grid, Button } from '@mantine/core';
import { Tasks } from '@components/Tasks/Tasks';
import useStyles from './Welcome.styles';

export function Welcome() {
  const { classes } = useStyles();

  return (
    <>
      <Grid className={classes.welcome}>
        <Grid.Col span={12}>
          <div id="logo-wrapper" style={{ justifyContent: 'center', display: 'flex' }}>
            <img src="exemplum-logo.svg" alt="Exemplum Logo" width="400" height="260" />
          </div>
        </Grid.Col>
        <Grid.Col span={12}>
          <Text align="center" className={classes.titles}>
            Get off to a good start
          </Text>
          <Text align="center" className={clsx(classes.titles, classes.smaller)}>
            A template for faster .net service development
          </Text>
        </Grid.Col>
        <Grid.Col span={12}>
          <Text align="center" className={classes.description}>
            Exemplum is the ideal starting point for .NET developers who want to rapidly build
            amazing microservices using a good starting point for libraries and tools.
          </Text>
        </Grid.Col>
        <Grid.Col span={12}>
          <div style={{ justifyContent: 'center', display: 'flex', paddingTop: '15px' }}>
            <Link href="https://forresttech.github.io/exemplum/" passHref>
              <Button
                className={clsx(classes.button, classes.getStarted)}
                size="lg"
                variant="gradient"
                radius="sm"
                role="button"
                component="a"
              >
                GET STARTED
              </Button>
            </Link>
            <Link href="https://github.com/ForrestTech/Exemplum" passHref>
              <Button
                className={clsx(classes.button, classes.gitHub)}
                size="lg"
                variant="gradient"
                radius="sm"
                role="button"
                component="a"
                target="_blank"
              >
                GITHUB
              </Button>
            </Link>
          </div>
        </Grid.Col>
        <Grid.Col span={12}>
          <Tasks />
        </Grid.Col>
      </Grid>
    </>
  );
}
