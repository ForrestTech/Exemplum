import { Button, Group, TextInput } from '@mantine/core';
import { useForm } from '@mantine/hooks';
import { v4 as uuidv4 } from 'uuid';
import { TodoItem } from '@features/todo/todoItem';
import { addTodoItem } from '@features/todo/todoItem-api';

interface TodoListItemForm {
  title: string;
}

const AddTodoListItemForm = ({ listId }: { listId: string }) => {
  const { reset, values, onSubmit, getInputProps } = useForm<TodoListItemForm>({
    initialValues: {
      title: '',
    },
    validationRules: {
      title: (value: string) => value.length > 2,
    },
    errorMessages: {
      title: 'Todo list title must be at least 2 characters long',
    },
  });

  const canAdd = () => values.title.length > 2;

  const handleSubmit = (submittedValues: TodoListItemForm) => {
    const todoItem: TodoItem = {
      listId,
      id: uuidv4(),
      title: submittedValues.title,
      complete: false,
    };

    //save to api
    addTodoItem(todoItem);

    reset();
  };

  return (
    <form
      onSubmit={onSubmit((submittedValues: TodoListItemForm) => {
        handleSubmit(submittedValues);
      })}
    >
      <Group mt="md">
        <TextInput
          required
          placeholder="Add a todo item"
          aria-label="Todo item title"
          {...getInputProps('title')}
          sx={{ width: '400px' }}
        />
        <Button type="submit" radius="sm" disabled={!canAdd()}>
          Add
        </Button>
      </Group>
    </form>
  );
};

export default AddTodoListItemForm;
