import React, { ReactNode, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { AxiosResponse, AxiosError } from 'axios';
import { addAccessTokenInterceptor, getClient } from '@features/common/httpClient';
import { SWRConfig, Cache } from 'swr';
import { Fetcher, PublicConfiguration } from 'swr/dist/types';
import { showNotification } from '@mantine/notifications';
import isDev from '../environment';

type Provider = { provider?: (cache: Readonly<Cache<any>>) => Cache<any> };

export function ExemplumSwrConfig({
  children,
  swrConfig,
}: {
  children?: ReactNode;
  swrConfig?: Partial<PublicConfiguration<any, any, Fetcher<any>>> & Provider;
}) {
  // configure the default http client to send token from the auth0 client
  // this has to be done at this level inside a component loaded by the root app that wraps it in the auth0 provider

  const { getAccessTokenSilently } = useAuth0();
  useEffect(() => {
    addAccessTokenInterceptor(getAccessTokenSilently);
  }, [getAccessTokenSilently]);

  return (
    <SWRConfig
      value={{
        fetcher: customFetcher,
        onError: (error: AxiosError, key) => {
          if (isDev()) {
            // eslint-disable-next-line no-console
            console.error(`Error fetching ${key}`, error);
          }

          showNotification({
            title: 'Fetching data error',
            message: 'An error happened while trying to fetch data! 🤥',
            color: 'red',
          });
        },
        ...swrConfig,
      }}
    >
      {children}
    </SWRConfig>
  );
}

const customFetcher = (url: string) =>
  getClient()
    .get(url)
    .then((response: AxiosResponse) => response.data);
