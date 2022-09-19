import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Grid, Title, Text, Container, createStyles } from '@mantine/core';

import WeatherForecastsTable from '@features/weather/WeatherForecasts/WeatherForecastsTable';
import { hasRole } from '@features/common/authUtils';

import LoginRedirect from '@components/LoginRedirect/LoginRedirect';

const useStyles = createStyles(() => ({
  title: {
    marginBottom: '20px',
  },
}));

const Weather = () => {
  const { classes } = useStyles();

  return (
    <Container size={800}>
      <Grid>
        <Grid.Col span={12}>
          <Title order={1} className={classes.title}>
            Weather
          </Title>
          <Text>
            This component demonstrates fetching data from a service and the use of the browser
            geolocation API.
          </Text>
        </Grid.Col>
        <Grid.Col span={12}>
          <Title order={2}>Forecasts</Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <WeatherForecastsTable />
        </Grid.Col>
      </Grid>
    </Container>
  );
};

export default withAuthenticationRequired(Weather, {
  onRedirecting: () => <LoginRedirect />,
  claimCheck: (claims?) => hasRole(claims, 'Forecaster'),
});
