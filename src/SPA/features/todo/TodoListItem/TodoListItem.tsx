import { ChangeEvent, useState } from 'react';
import { ActionIcon, Group, TextInput, Text, Tooltip, Switch } from '@mantine/core';
import { DeviceFloppy, X, Edit, Trash } from 'tabler-icons-react';
import { TodoItem } from '@features/todo/todoItem';
import { deleteTodoItem, updateTodoItem } from '@features/todo/todoItem-api';

const TodoListItem = ({ item }: { item: TodoItem }) => {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [title, setTitle] = useState<string>(item.title);
  const [complete, setComplete] = useState<boolean>(item.complete);

  const handleEdit = () => {
    setEditMode(true);
  };

  const handleKeyPress = async (event: React.KeyboardEvent, itemToEdit: TodoItem) => {
    if (event.key === 'Enter') {
      handleSave(itemToEdit);
      setEditMode(false);
    }
  };

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setTitle(event.target.value);
  };

  const handleSave = (itemToSave: TodoItem) => {
    const updated = itemToSave;
    updated.title = title;
    updated.complete = complete;

    //save to api
    updateTodoItem(updated);

    setEditMode(false);
  };

  const handleCompleted = (itemToComplete: TodoItem, event: ChangeEvent<HTMLInputElement>) => {
    setComplete(event.currentTarget.checked);
    handleSave(itemToComplete);
  };

  const handleDelete = (itemToDelete: TodoItem) => {
    deleteTodoItem(itemToDelete);
  };

  const handleCancel = () => {
    setEditMode(false);
  };

  return editMode ? (
    <Group style={{ marginTop: '10px' }}>
      <TextInput
        value={title}
        sx={{ width: '400px' }}
        onChange={(event) => handleChange(event)}
        onKeyPress={(event) => handleKeyPress(event, item)}
        autoFocus
      />
      <span style={{ flexGrow: 1 }} />
      <Tooltip label="Save">
        <ActionIcon>
          <DeviceFloppy onClick={() => handleSave(item)} />
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
      <Text>{item.title}</Text>
      <span style={{ flexGrow: 1 }} />
      <Tooltip label="Edit">
        <ActionIcon>
          <Edit onClick={() => handleEdit()} />
        </ActionIcon>
      </Tooltip>
      <Tooltip label="Mark as done">
        <Switch
          checked={complete}
          onChange={(event) => {
            handleCompleted(item, event);
          }}
        />
      </Tooltip>
      <Tooltip label="Delete">
        <ActionIcon>
          <Trash onClick={() => handleDelete(item)} />
        </ActionIcon>
      </Tooltip>
    </Group>
  );
};

export default TodoListItem;
