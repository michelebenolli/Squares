import { FilterRequest } from "src/app/shared/models/filter-request";
import { FilterConfiguration } from "./filter-configuration";
import { FilterType } from "./filter-type";
import { FilterValue } from "./filter-value";

export interface Filter {
  type?: FilterType;
  name?: string;
  placeholder?: string;
  values?: FilterValue[];
  config?: FilterConfiguration;
  request: FilterRequest;
}