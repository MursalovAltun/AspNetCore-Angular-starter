import { Controller } from "react-hook-form";
import { TextField } from "@mui/material";
import { FormInputProps } from "./form-input-props.model";

export const FormInputText = ({ name, control, label, ...rest }: FormInputProps) => {
  return (
    <Controller
      name={name as any}
      control={control}
      render={({ field: { onChange, value }, fieldState: { error }, formState }) => (
        <TextField
          helperText={error ? error.message : null}
          size="small"
          error={!!error}
          onChange={onChange}
          value={value}
          fullWidth
          label={label}
          variant="outlined"
          {...rest}
        />
      )}
    />
  );
};
