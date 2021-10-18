import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { AlertColor, SnackbarOrigin } from "@mui/material";

const name = "snackbar";

export interface SnackbarState {
  open: boolean;
  message: string;
  type: AlertColor;
  position: SnackbarOrigin;
  duration: number;
}

const initialState: SnackbarState = {
  message: "",
  open: false,
  type: "info",
  position: { horizontal: "center", vertical: "bottom" },
  duration: 5000,
};

const snackbarSlice = createSlice({
  name,
  initialState,
  reducers: {
    showSnackbar: (state, action: PayloadAction<{ message: string; type: AlertColor }>) => {
      state.open = true;
      state.message = action.payload.message;
      state.type = action.payload.type;
    },
    closeSnackbar: state => {
      state.open = false;
    },
  },
});

export const { showSnackbar, closeSnackbar } = snackbarSlice.actions;

export const snackbarState = (state: RootState) => state.snackbar;

export default snackbarSlice.reducer;
