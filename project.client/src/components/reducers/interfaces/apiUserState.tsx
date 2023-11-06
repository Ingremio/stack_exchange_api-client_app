import { SerializedError } from "@reduxjs/toolkit";
import { IApiUser } from "./apiUser";

export interface IApiUserState {
    user: IApiUser | null; error: SerializedError | null;
}
