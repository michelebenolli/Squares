import { FilterRequest } from "src/app/shared/models/filter-request";

export interface FilterConfiguration {
  service?: any;
  filter?: FilterRequest;
  label: (x: any) => string;
}
