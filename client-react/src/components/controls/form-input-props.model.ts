import { InputProps } from "@mui/material";

export interface FormInputProps {
  name: string;
  control: any;
  label: string;
  setValue?: any;
  type?: string;
  InputProps?: InputProps;
}
