import { Container, createStyles } from '@mantine/core';
import { getBackGroundColor } from '@features/common/styles/styles';
import { Welcome } from '@components/Welcome/Welcome';
import { Footer } from '@components/Footer/Footer';

const useStyles = createStyles((theme) => ({
  mainContent: {
    backgroundColor: getBackGroundColor(theme),
    paddingTop: '86px',
    marginTop: '50px',
  },
  '@media (min-width: 960px)': {
    mainContent: {
      backgroundColor: getBackGroundColor(theme),
      paddingTop: '100px',
      paddingLeft: '12px',
      marginTop: '70px',
    },
  },
}));

const HomePage = () => {
  const { classes } = useStyles();

  return (
    <Container className={classes.mainContent}>
      <Welcome />
      <Footer />
    </Container>
  );
};

export default HomePage;
