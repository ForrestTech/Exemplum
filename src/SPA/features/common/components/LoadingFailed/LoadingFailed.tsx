import { AxiosError } from 'axios';

interface LoadingFailedProps {
  error: AxiosError;
}

const LoadingFailed = ({ error }: LoadingFailedProps) => {
  return <div>Loading failed</div>;
};

export default LoadingFailed;
