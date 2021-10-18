import { useAppDispatch, useAppSelector } from "../../app/hooks";
import * as fromSnackbar from "./snackbarSlice";
import { Alert, Snackbar } from "@mui/material";

const CustomSnackbar = () => {
  const dispatch = useAppDispatch();
  const { open, type, message, position, duration } = useAppSelector(fromSnackbar.snackbarState);

  return (
    <Snackbar
      open={open}
      autoHideDuration={duration}
      anchorOrigin={position}
      onClose={() => dispatch(fromSnackbar.closeSnackbar())}
    >
      <Alert severity={type}>{message}</Alert>
    </Snackbar>
  );
};

export default CustomSnackbar;
