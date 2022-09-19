import { getClient } from '@features/common/httpClient';
import { PagedResults } from '@features/common/pageResults';
import useSWR, { mutate } from 'swr';
import { TodoItem } from './todoItem';
import { todoListApi } from './todoList-api';

const httpClient = getClient();

const todoItemsApi = (listId: string) => `${todoListApi}/${listId}/todo`;
const todoItemApi = (listId: string, itemId: string) => `${todoListApi}/${listId}/todo/${itemId}`;

export const useTodoListItems = (listId: string) => {
  const { data: todoItems, error } = useSWR<PagedResults<TodoItem>>(todoItemsApi(listId));

  return {
    todoItems,
    error,
  };
};

export const addTodoItem = (newTodoItem: TodoItem) => {
  mutate(todoItemsApi(newTodoItem.listId), async (currentState: TodoItem[]) => {
    await httpClient.post(todoItemApi(newTodoItem.listId, newTodoItem.id), newTodoItem);

    currentState.push(newTodoItem);

    return currentState;
  });
};

export const updateTodoItem = (itemToUpdate: TodoItem) => {
  mutate(todoItemsApi(itemToUpdate.listId), async (currentState: TodoItem[]) => {
    await httpClient.put(todoItemApi(itemToUpdate.listId, itemToUpdate.id), itemToUpdate);

    return [...currentState.filter((list) => list.id !== itemToUpdate.id), itemToUpdate];
  });
};

export const deleteTodoItem = (itemToDelete: TodoItem) => {
  mutate(todoItemsApi(itemToDelete.listId), async (currentState: TodoItem[]) => {
    await httpClient.delete(todoItemApi(itemToDelete.listId, itemToDelete.id));

    return currentState.filter((list) => list.id !== itemToDelete.id);
  });
};
