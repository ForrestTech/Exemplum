import { ChangeEvent, useState } from 'react';
import Link from 'next/link';
import { ActionIcon, Group, TextInput, Text, Tooltip } from '@mantine/core';
import { TodoList } from '@features/todo/todoList';
import { deleteTodoList, updateTodoList } from '@features/todo/todoList-api';
import { DeviceFloppy, Edit, ListCheck, Trash, X } from 'tabler-icons-react';

const TodoListEntry = ({ todoList }: { todoList: TodoList }) => {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [listName, setListName] = useState<string>(todoList.title);

  const handleEdit = () => {
    setEditMode(true);
  };

  const handleKeyPress = async (event: React.KeyboardEvent, listToEdit: TodoList) => {
    if (event.key === 'Enter') {
      handleSave(listToEdit);
      setEditMode(false);
    }
  };

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setListName(event.target.value);
  };

  const handleSave = (listToUpdate: TodoList) => {
    //save changes
    const updated = listToUpdate;
    updated.title = listName;

    updateTodoList(updated);

    setEditMode(false);
  };

  const handleDelete = (listToDelete: TodoList) => {
    deleteTodoList(listToDelete);
  };

  const handleCancel = () => {
    setEditMode(false);
  };

  return editMode ? (
    <Group style={{ marginTop: '10px' }}>
      <TextInput
        value={listName}
        sx={{ width: '400px' }}
        onChange={(event) => handleChange(event)}
        onKeyPress={(event) => handleKeyPress(event, todoList)}
        autoFocus
      />
      <span style={{ flexGrow: 1 }} />
      <Tooltip label="Save">
        <ActionIcon>
          <DeviceFloppy onClick={() => handleSave(todoList)} />
        </ActionIcon>
      </Tooltip>
      <Tooltip label="Cancel">
        <ActionIcon>
          <X onClick={() => handleCancel()} />
        </ActionIcon>
      </Tooltip>
    </Group>
  ) : (
    <Group style={{ marginTop: '10px' }}>
      <Text color={todoList.colour}>{todoList.title}</Text>
      <span style={{ flexGrow: 1 }} />
      <Tooltip label="Edit">
        <ActionIcon>
          <Edit onClick={() => handleEdit()} />
        </ActionIcon>
      </Tooltip>
      <Tooltip label="View">
        <Link href={`/todolists/items?listId=${todoList.id}`} passHref>
          <ActionIcon>
            <ListCheck />
          </ActionIcon>
        </Link>
      </Tooltip>
      <Tooltip label="Delete">
        <ActionIcon>
          <Trash onClick={() => handleDelete(todoList)} />
        </ActionIcon>
      </Tooltip>
    </Group>
  );
};

export default TodoListEntry;
