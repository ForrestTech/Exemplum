export type PagedResults<T> = {
  items: T[];
  pageIndex: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: false;
  hasNextPage: false;
};
