import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../app/store";
import { Color } from "@material-ui/lab";

const name = 'snackbar';

export enum SnackbarType {
  Success,
  Warning,
  Error,
  Info
}

export interface SnackbarState {
  open: boolean;
  message: string;
  type: Color;
}

const initialState: SnackbarState = {
  message: '',
  open: false,
  type: "info",
}

const snackbarSlice = createSlice({
  name,
  initialState,
  reducers: {
    showSnackbar: (state, action: PayloadAction<{ message: string, type: Color }>) => {
      state.open = true;
      state.message = action.payload.message;
      state.type = action.payload.type;
    },
    closeSnackbar: state => {
      state.open = false;
    }
  }
})

export const { showSnackbar, closeSnackbar } = snackbarSlice.actions;

export const snackbarState = (state: RootState) => state.snackbar;

export default snackbarSlice.reducer;
