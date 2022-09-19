import React from 'react';
import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Grid, Title, Container, createStyles } from '@mantine/core';

import CustomerGrid from '@features/customer/CustomerGrid/CustomerGrid';
import { useStore } from '@features/customer/store';

import LoginRedirect from '@components/LoginRedirect/LoginRedirect';

const useStyles = createStyles(() => ({
  title: {
    marginBottom: '20px',
  },
}));

const DataGrid = () => {
  const { classes } = useStyles();

  const selectedCustomers = useStore((state) => state.selectedCustomers);

  return (
    <Container size={900}>
      <Grid>
        <Grid.Col span={12}>
          <Title order={1} className={classes.title}>
            Data Grid
          </Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <CustomerGrid />
        </Grid.Col>
        {selectedCustomers && selectedCustomers.length > 0 && (
          <>
            <Grid.Col span={12}>
              <Title order={2} className={classes.title}>
                Selected Customers
              </Title>
            </Grid.Col>
            <Grid.Col span={12}>
              {selectedCustomers.map((customer) => (
                <div key={customer.id}>{customer.firstName}</div>
              ))}
            </Grid.Col>
          </>
        )}
      </Grid>
    </Container>
  );
};

export default withAuthenticationRequired(DataGrid, {
  onRedirecting: () => <LoginRedirect />,
});
