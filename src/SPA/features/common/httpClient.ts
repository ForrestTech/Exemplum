import axios from 'axios';

export const hostBaseUrl = process.env.NEXT_PUBLIC_TODO_HOST_BASE_URL ?? 'https://localhost:3001';
export const todoApiBaseUrl = process.env.NEXT_PUBLIC_TODO_API_BASE_URL ?? 'https://localhost:5001';

export const apiPath = (url: string) => `${todoApiBaseUrl}${url}`;

const httpClient = axios.create({
  baseURL: todoApiBaseUrl,
});

/* adds access tokens in all api requests this interceptor is only
 added when the auth0 instance is ready and exports the getAccessTokenSilently method
*/
export const addAccessTokenInterceptor = (getAccessTokenSilently: () => Promise<string>) => {
  httpClient.interceptors.request.use(async (config) => {
    const token = await getAccessTokenSilently();
    const updateConfig = config;
    if (updateConfig.headers) {
      updateConfig.headers.Authorization = `Bearer ${token}`;
    }
    return updateConfig;
  });
};

export const getClient = () => httpClient;
