import { Filter } from "../components/filters/models/filter";
import { FilterRequest } from "../models/filter-request";
import { PagedRequest } from "../models/paged-request";

export function hasValue(request: FilterRequest): boolean {
  return request.value || request.filters?.some(x => hasValue(x));
}

export function getRequest(filters?: Filter[], pageSize?: number): PagedRequest {
  let request: PagedRequest = { pageNumber: 1, pageSize: pageSize ?? 10 };
  setFilters(request, filters?.map(x => x.request));
  return request;
}

export function setFilters(request: PagedRequest, filters?: FilterRequest[]) {
  // Get only filters with a value set
  filters = filters?.filter(x => hasValue(x));
  request.filters = filters?.filter(x => !x.specificField); 

  // Add filters on specific fields
  filters?.filter(x => x.specificField).forEach(x => request[x.specificField!] = x.value);
}