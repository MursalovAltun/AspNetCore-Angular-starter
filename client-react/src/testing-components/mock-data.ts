type RecursivePartial<T> = {
  [P in keyof T]?: RecursivePartial<T[P]>;
};

const mockData = <T>(data: RecursivePartial<T>): T => {
  return {
    ...data,
  } as T;
};

export default mockData;
