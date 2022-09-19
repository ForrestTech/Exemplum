import useSWR, { mutate } from 'swr';
import { PagedResults } from '@features/common/pageResults';
import { getClient } from '@features/common/httpClient';
import { CreateTodoList, TodoList } from './todoList';

const httpClient = getClient();
export const todoListApi = '/api/todolist';

export const useTodoLists = () => {
  const { data: todoLists, error } = useSWR<PagedResults<TodoList>>(todoListApi);

  return {
    todoLists,
    error,
  };
};

export const getTodoList = async (listId: string): Promise<TodoList> => {
  const response = await httpClient.get<TodoList>(`${todoListApi}?id=${listId}`);
  return response.data;
};

export const addTodoList = async (newTodoList: CreateTodoList) => {
  await mutate(todoListApi, async (currentState: TodoList[]) => {
    const newListResponse = await httpClient.post<TodoList>(todoListApi, newTodoList);

    if (newListResponse.status === 200) {
      currentState.push(newListResponse.data);
    }

    return currentState;
  });
};

export const updateTodoList = async (listToUpdate: TodoList) => {
  await mutate(todoListApi, async (currentState: TodoList[]) => {
    await httpClient.put(`${todoListApi}?id=${listToUpdate.id}`, listToUpdate);

    return [...currentState.filter((list) => list.id !== listToUpdate.id), listToUpdate];
  });
};

export const deleteTodoList = async (listToDelete: TodoList) => {
  await mutate(todoListApi, async (currentState: TodoList[]) => {
    await httpClient.delete(`${todoListApi}?id=${listToDelete.id}`);

    return currentState.filter((list) => list.id !== listToDelete.id);
  });
};
