export interface Pagination<T> {
  items: T[];
  count: number;
  page: number;
  size: number;
}
