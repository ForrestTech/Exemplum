import create from 'zustand';
import { Customer } from './customer';

export interface CustomerState {
  selectedCustomers: Customer[];
  setSelectedCustomers: (rowSelected: Customer[]) => void;
}

export const useStore = create<CustomerState>((set) => ({
  selectedCustomers: [] as Customer[],
  setSelectedCustomers: (selectedCustomers: Customer[]) =>
    set((state) => ({ ...state, selectedCustomers })),
}));
