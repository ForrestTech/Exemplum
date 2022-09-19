import { Loader } from '@mantine/core';
import LoadingFailed from '@components/LoadingFailed/LoadingFailed';
import Empty from '@components/Empty/Empty';
import { AxiosError } from 'axios';

interface LoadingWrapperProps {
  error: AxiosError | undefined;
  data: any[] | any | undefined;
  children: JSX.Element | JSX.Element[] | string | undefined;
}

const LoadingWrapper = ({ error, data, children }: LoadingWrapperProps) => (
  <>
    {!error && !data && <Loader />}
    {error && <LoadingFailed error={error} />}
    {!error && data && children}
    {!error && data && Array.isArray(data) && data.length === 0 && <Empty />}
  </>
);

export default LoadingWrapper;
