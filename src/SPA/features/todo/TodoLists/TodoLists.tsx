import { Paper } from '@mantine/core';
import { TodoList } from '@features/todo/todoList';

import { PagedResults } from '@features/common/pageResults';
import TodoListEntry from '../TodoListEntry/TodoListEntry';

const TodoLists = ({ todoLists }: { todoLists: PagedResults<TodoList> | undefined }) => (
  <Paper shadow="xs" p="md">
    {todoLists?.items &&
      todoLists.items.map((list) => <TodoListEntry key={list.id} todoList={list} />)}
  </Paper>
);

export default TodoLists;
