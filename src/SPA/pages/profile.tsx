import { useEffect, useState } from 'react';
import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Grid, Title, Container, Table, Text, createStyles, Divider } from '@mantine/core';
import LoginRedirect from '@components/LoginRedirect/LoginRedirect';

const useStyles = createStyles(() => ({
  title: {
    marginBottom: '20px',
  },
}));

const Profile = () => {
  const { classes } = useStyles();
  const [userClaims, setUserClaims] = useState<Record<string, string>>();

  //You can use swr to get these values and they will be cached but here is
  //an example of using basic fetch
  useEffect(() => {
    fetch('api/auth/me')
      .then((res) => res.json())
      .then((claimsData) => {
        setUserClaims(claimsData);
      });
  }, []);

  return (
    <Container size={900}>
      <Grid>
        <Grid.Col span={12}>
          <Title order={1} className={classes.title}>
            Profile
          </Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <Title order={2}>User Claims</Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <Text>This component shows the claims of the current user JWT.</Text>
          <Table horizontalSpacing="md" verticalSpacing="md" highlightOnHover>
            <thead>
              <tr>
                <th>Claim</th>
                <th>Value</th>
              </tr>
            </thead>
            <tbody>
              {userClaims &&
                Object.keys(userClaims).map((key) => (
                  <tr key={key}>
                    <td>{key}</td>
                    <td>{userClaims[key].toString()}</td>
                  </tr>
                ))}
            </tbody>
          </Table>
          <Divider />
        </Grid.Col>
      </Grid>
    </Container>
  );
};

export default withAuthenticationRequired(Profile, {
  onRedirecting: () => <LoginRedirect />,
});
