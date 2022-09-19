import { Button, ColorInput, Group, TextInput } from '@mantine/core';
import { useForm } from '@mantine/hooks';
import { CreateTodoList } from '@features/todo/todoList';
import { addTodoList } from '@features/todo/todoList-api';
import { formSubmissionHandler } from '@features/common/formsUtils';

const AddTodoListsForm = () => {
  const { reset, values, onSubmit, getInputProps, setFieldValue, setFieldError } =
    useForm<CreateTodoList>({
      initialValues: {
        title: '',
        colour: '',
      },
      validationRules: {
        title: (value: string) => value.length > 2,
        colour: (value: string) => value.length > 2,
      },
      errorMessages: {
        title: 'List name must be at least 2 characters long',
        colour: 'Select a color',
      },
    });

  const canAdd = () => values.title.length > 2 && values.colour?.length > 2;

  const handleSubmit = async (submittedValues: CreateTodoList) => {
    await formSubmissionHandler<CreateTodoList>(async () => {
      await addTodoList(submittedValues);
      reset();
    }, setFieldError);
  };

  return (
    <form
      onSubmit={onSubmit((submittedValues: CreateTodoList) => {
        handleSubmit(submittedValues);
      })}
    >
      <Group mt="md">
        <TextInput
          required
          placeholder="Add a list name"
          aria-label="Todo list name"
          {...getInputProps('title')}
          sx={{ width: '400px' }}
        />
        <ColorInput
          onFocus={() => setFieldValue('colour', '#7950f2')}
          placeholder="List colour"
          {...getInputProps('colour')}
          swatches={[
            '#25262b',
            '#868e96',
            '#fa5252',
            '#e64980',
            '#be4bdb',
            '#7950f2',
            '#4c6ef5',
            '#228be6',
            '#15aabf',
            '#12b886',
            '#40c057',
            '#82c91e',
            '#fab005',
            '#fd7e14',
          ]}
        />
        <Button type="submit" radius="sm" disabled={!canAdd()}>
          Add
        </Button>
      </Group>
    </form>
  );
};

export default AddTodoListsForm;
