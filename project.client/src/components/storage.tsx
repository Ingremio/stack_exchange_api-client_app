import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { userReducer } from "./reducers/usersSlice";

export const storage = configureStore({
    reducer: {
        userState: userReducer,
        //Laterrrrrrr...
    }
});

declare global {
    export type RootState = ReturnType<typeof storage.getState>;
    export type AppDispatch = typeof storage.dispatch;
}

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;