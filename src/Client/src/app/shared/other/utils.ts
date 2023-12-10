import { Filter } from "../components/filters/models/filter";
import { FilterRequest } from "../models/filter-request";
import { PagedRequest } from "../models/paged-request";

export function hasValue(request: FilterRequest): boolean {
  return request.value !== undefined || !!request.filters?.some(x => hasValue(x));
}

export function getRequest(filters?: Filter[], pageSize?: number): PagedRequest {
  return { 
    pageNumber: 1, 
    pageSize: pageSize ?? 10, 
    filters: filters?.map(x => x.request).filter(x => hasValue(x)) 
  };
}