import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import HttpClient from "../helpers/httpClient";
import history from "../history";
import { IApiUser } from "./interfaces/apiUser";
import { IApiUserState } from "./interfaces/apiUserState";

const login = createAsyncThunk(
    `users/login`,
    async () => {
        return await HttpClient.get<IApiUser>('users/login');
    }
)

const logout = createAsyncThunk(
    `users/logout`,
    async (user: IApiUser) => {
        return HttpClient.post<IApiUser, boolean>('users/logout', user);
    }
)

const initialState: IApiUserState = {
    user: JSON.parse(localStorage.getItem('user')!),
    error: null
}

export const userSlice = createSlice({
    name: 'users',
    initialState: initialState,
    reducers: {
        showState: (state) => {
            console.info(state);
        }
    },
    extraReducers: (builder) => {
        builder.addCase(login.pending, (state) => {
            state.error = null;
        });
        builder.addCase(login.fulfilled, (state, action) => {
            const user = action.payload;

            if (user.id && user.name && user.token) {
                localStorage.setItem('user', JSON.stringify(user));
                state.user = user;

                if (history.location?.pathname) {
                    history.navigate?.(history.location?.pathname)
                }

                history.navigate?.('/');
            }
        });
        builder.addCase(login.rejected, (state, action) => {
            state.error = action.error;
        });
        builder.addCase(logout.pending, (state) => {
            state.error = null;
        });
        builder.addCase(logout.fulfilled, (state, action) => {
            const success = action.payload;

            if (success) {
                state.user = null;
                localStorage.removeItem('user');

                history.navigate?.('/login');
            }
        });
        builder.addCase(logout.rejected, (state, action) => {
            state.error = action.error;
        });
    }
})

export const userActions = { ...userSlice.actions, login, logout };
export const userReducer = userSlice.reducer;