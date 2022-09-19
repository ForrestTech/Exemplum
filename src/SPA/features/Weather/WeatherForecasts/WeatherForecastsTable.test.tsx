import { screen, waitForElementToBeRemoved } from '@testing-library/react';
import React from 'react';
import { rest } from 'msw';
import { setupServer } from 'msw/node';
import { testRender } from '@features/common/testing/testRenderer';
import { apiPath } from '@features/common/httpClient';
import { WeatherForecastsTable } from './WeatherForecastsTable';
import { WeatherForecast } from '../weather';

const dummyWeatherForecasts: WeatherForecast = {
  name: 'Newcastle upon tyne',
  weather: [{ main: 'Clouds', description: 'broken clouds' }],
  main: { temp: 297.4 },
};

const server = setupServer(
  rest.get(apiPath('/api/weatherforecast'), (req, res, ctx) =>
    res(ctx.status(200), ctx.json(dummyWeatherForecasts))
  )
);

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());

jest.mock('@auth0/auth0-react');

describe('Weather forecasts table component', () => {
  it('should have data"', async () => {
    testRender(<WeatherForecastsTable />);

    await waitForElementToBeRemoved(() => screen.queryByRole('presentation'));

    const element = screen.getByText(dummyWeatherForecasts.name);

    expect(element).toBeInTheDocument();
  });
});
