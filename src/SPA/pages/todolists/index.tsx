import { withAuthenticationRequired } from '@auth0/auth0-react';
import { Grid, Title, Container } from '@mantine/core';

import AddTodoListsForm from '@features/todo/AddTodoListForm/AddTodoListForm';
import TodoLists from '@features/todo/TodoLists/TodoLists';
import { useTodoLists } from '@features/todo/todoList-api';

import LoginRedirect from '@components/LoginRedirect/LoginRedirect';
import LoadingWrapper from '@components/LoadingWrapper/LoadingWrapper';

const List = () => {
  const { todoLists, error } = useTodoLists();

  return (
    <Container size={800}>
      <Grid>
        <Grid.Col span={12}>
          <Title order={1}>Todo Lists</Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <AddTodoListsForm />
        </Grid.Col>
        <Grid.Col span={12} sx={{ marginTop: '20px' }}>
          <LoadingWrapper error={error} data={todoLists}>
            <TodoLists todoLists={todoLists} />
          </LoadingWrapper>
        </Grid.Col>
      </Grid>
    </Container>
  );
};

export default withAuthenticationRequired(List, {
  onRedirecting: () => <LoginRedirect />,
});
