import Snackbar from '@material-ui/core/Snackbar';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import * as fromSnackbar from "./snackbarSlice";
import MuiAlert, { AlertProps } from '@material-ui/lab/Alert';

function Alert(props: AlertProps) {
  return <MuiAlert elevation={6} variant="filled" {...props} />;
}

const CustomSnackbar = () => {
  const dispatch = useAppDispatch();
  const { open, type, message } = useAppSelector(fromSnackbar.snackbarState);

  return (
    <Snackbar open={open} autoHideDuration={2000} onClose={() => dispatch(fromSnackbar.closeSnackbar())}>
      <Alert severity={type}>{message}</Alert>
    </Snackbar>
  );
}

export default CustomSnackbar;
