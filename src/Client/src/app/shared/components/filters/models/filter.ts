import { FilterRequest } from "src/app/shared/models/filter-request";
import { FilterSelect } from "./filter-select";
import { FilterType } from "./filter-type";

export interface Filter {
  type: FilterType;
  request: FilterRequest;
  label?: string;
  overlay?: boolean;
  select?: FilterSelect;
}