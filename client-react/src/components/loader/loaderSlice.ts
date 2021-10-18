import { createSelector, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

interface IState {
  showLoader: boolean;
}

const initialState: IState = { showLoader: false };

const name = "loader";

const loaderSlice = createSlice({
  name,
  initialState,
  reducers: {
    toggleLoader: (state: IState) => {
      state.showLoader = !state.showLoader;
    },
  },
});

export const { toggleLoader } = loaderSlice.actions;

const loaderState = createSelector(
  (state: RootState) => state.loader,
  loader => loader
);

export const showLoader = createSelector(loaderState, loader => loader.showLoader);

export default loaderSlice.reducer;
