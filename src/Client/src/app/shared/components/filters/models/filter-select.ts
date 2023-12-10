import { FilterRequest } from "src/app/shared/models/filter-request";
import { FilterOption } from "./filter-option";

export interface FilterSelect {
  service?: any;
  label?: (x: any) => string;
  options?: FilterOption[];
  request?: FilterRequest;
}
