import type { NextApiRequest, NextApiResponse } from 'next';
import { Customer } from '@features/customer/customer';
import { faker } from '@faker-js/faker';

const customers: Customer[] = new Array(100).fill({}).map(() => ({
  id: faker.datatype.number(),
  firstName: faker.name.firstName(),
  lastName: faker.name.lastName(),
  dateOfBirth: faker.date.birthdate(),
  email: faker.internet.email(),
}));

// a dummy api to return fake customer data for showing in a data grid demo
export default function handler(req: NextApiRequest, res: NextApiResponse<Customer[]>) {
  res.status(200).json(customers);
}
