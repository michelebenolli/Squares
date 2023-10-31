import { FilterRequest } from "./filter-request";

export interface BaseRequest {
    orderBy?: string[];
    filters?: FilterRequest[];
    [key: string]: any;
}
