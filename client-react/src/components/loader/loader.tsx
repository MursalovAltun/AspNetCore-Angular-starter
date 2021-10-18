import styles from "./loader.module.scss";
import { useAppSelector } from "../../app/hooks";
import * as fromLoader from "./loaderSlice";

const Loader = () => {
  const showLoader = useAppSelector(fromLoader.showLoader);

  return (
    <>
      {showLoader && (
        <div className={styles.overlay}>
          <div className={styles.container}>
            <div className={styles.loader}>Loading...</div>
          </div>
        </div>
      )}
    </>
  );
};

export default Loader;
