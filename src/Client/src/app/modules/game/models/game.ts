import { User } from "../../identity/models/user";

export interface Game {
    id?: number;
    userId: number;
    score: number;
    dateTime: string;

    user: User;
  }
  