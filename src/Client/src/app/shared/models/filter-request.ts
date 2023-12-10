export interface FilterRequest {
  logic?: 'and' | 'or' | 'xor';
  operator?: 'eq' | 'neq' | 'lt' | 'lte' | 'gt' | 'gte' | 'startswith' | 'endswith' | 'contains';
  filters?: FilterRequest[];
  field?: string;
  value?: any;
}
