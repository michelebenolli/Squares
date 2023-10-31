import { BaseRequest } from "./base-request";

export interface PagedRequest extends BaseRequest {
    pageNumber: number;
    pageSize: number;
}
