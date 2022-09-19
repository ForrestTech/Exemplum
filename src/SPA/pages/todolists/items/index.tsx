import { withAuthenticationRequired } from '@auth0/auth0-react';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/router';
import { Container, Grid, Title } from '@mantine/core';

import { getTodoList } from '@features/todo/todoList-api';
import { useTodoListItems } from '@features/todo/todoItem-api';
import AddTodoListItemForm from '@features/todo/AddTodoListItemForm/AddTodoListItemForm';
import TodoListItems from '@features/todo/TodoListItems/TodoListItems';

import LoginRedirect from '@components/LoginRedirect/LoginRedirect';
import LoadingWrapper from '@components/LoadingWrapper/LoadingWrapper';

const List = () => {
  const router = useRouter();
  const { listId } = router.query;
  const [listName, setListName] = useState<string>();

  useEffect(() => {
    const fetchData = async () => {
      const list = await getTodoList(listId as string);
      setListName(list.title);
    };

    fetchData();
  }, []);

  const { todoItems, error } = useTodoListItems(listId as string);

  return (
    <Container size={800}>
      <Grid>
        <Grid.Col span={12}>
          <Title order={1}>Todo Lists</Title>
          <Title order={2}>{listName}</Title>
        </Grid.Col>
        <Grid.Col span={12}>
          <AddTodoListItemForm listId={listId as string} />
        </Grid.Col>
        <Grid.Col span={12} sx={{ marginTop: '20px' }}>
          <LoadingWrapper error={error} data={todoItems}>
            <TodoListItems listItems={todoItems?.items} />
          </LoadingWrapper>
        </Grid.Col>
      </Grid>
    </Container>
  );
};

export default withAuthenticationRequired(List, {
  onRedirecting: () => <LoginRedirect />,
});
