import { Paper } from '@mantine/core';
import { TodoItem } from '@features/todo/todoItem';

import TodoListItem from '@features/todo/TodoListItem/TodoListItem';

const TodoListItems = ({ listItems }: { listItems: TodoItem[] | undefined }) => (
  <Paper shadow="xs" p="md">
    {listItems && listItems.map((item) => <TodoListItem key={item.id} item={item} />)}
  </Paper>
);

export default TodoListItems;
