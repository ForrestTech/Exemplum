import React, { useCallback, useMemo, useRef, useState } from 'react';
import useSWR from 'swr';
import { AgGridReact } from 'ag-grid-react';
import { format } from 'date-fns';

import { hostBaseUrl } from '@features/common/httpClient';
import { Customer } from '@features/customer/customer';

import LoadingWrapper from '@components/LoadingWrapper/LoadingWrapper';

import { useStore } from '../store';

import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';

const columns = [
  { field: 'select', width: 20, checkboxSelection: true },
  { field: 'id', width: 100 },
  { field: 'firstName', filter: true },
  { field: 'lastName', filter: true },
  {
    field: 'dateOfBirth',
    filter: 'agDateColumnFilter',
    cellRenderer: (data: { value: string }) =>
      data.value ? format(Date.parse(data.value), 'dd/MM/yyyy') : '',
  },
  { field: 'email', editable: true },
];

const CustomerGrid = () => {
  const { data, error } = useSWR<Customer[]>(`${hostBaseUrl}/api/customer`);
  const setSelectedCustomers = useStore((state) => state.setSelectedCustomers);

  const gridRef = useRef<any>(null);
  const [columnDefs] = useState(columns);

  const defaultColDef = useMemo(
    () => ({
      sortable: true,
      //grid styles are defined in a global stylesheet global.css
      headerClass() {
        return 'data-grid-header';
      },
    }),
    []
  );

  const onSelectionChanged = useCallback(() => {
    if (gridRef.current) {
      const selectedRows = (gridRef.current as AgGridReact).api?.getSelectedNodes();
      setSelectedCustomers(selectedRows.map((row) => row.data));
    }
  }, []);

  return (
    <>
      <LoadingWrapper error={error} data={data}>
        {data && (
          <div className="ag-theme-alpine" style={{ height: 400, width: 900 }}>
            <AgGridReact
              ref={gridRef}
              rowData={data}
              columnDefs={columnDefs}
              defaultColDef={defaultColDef}
              rowSelection="multiple"
              onSelectionChanged={onSelectionChanged}
              pagination
              paginationPageSize={10}
            />
          </div>
        )}
      </LoadingWrapper>
    </>
  );
};

export default CustomerGrid;
