import { Role } from "./role";

export interface User {
  id?: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  image?: string;
  isActive: boolean;

  roles?: Role[];
}
