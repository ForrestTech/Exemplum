import { FC, ReactElement, ReactNode } from 'react';
import { render, RenderOptions } from '@testing-library/react';
import { AxiosResponse } from 'axios';
import { SWRConfig, Cache } from 'swr';
import { Fetcher, PublicConfiguration } from 'swr/dist/types';
import { getClient } from '../httpClient';

type Provider = { provider?: (cache: Readonly<Cache<any>>) => Cache<any> };

export function TestSwrConfig({
  children,
  swrConfig,
}: {
  children?: ReactNode;
  swrConfig?: Partial<PublicConfiguration<any, any, Fetcher<any>>> & Provider;
}) {
  return (
    <SWRConfig
      value={{
        fetcher: customFetcher,
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

const Providers: FC = ({ children }) => (
  <TestSwrConfig
    swrConfig={{ dedupingInterval: 0, shouldRetryOnError: false, provider: () => new Map() }}
  >
    {children}
  </TestSwrConfig>
);

export const testRender = (ui: ReactElement, options?: Omit<RenderOptions, 'wrapper'>) =>
  render(ui, { wrapper: Providers, ...options });
