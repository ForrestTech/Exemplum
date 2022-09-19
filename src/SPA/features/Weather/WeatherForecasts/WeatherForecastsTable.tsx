import { Table } from '@mantine/core';
import { useEffect, useState } from 'react';
import LoadingWrapper from '@components/LoadingWrapper/LoadingWrapper';
import { useWeatherForecast } from '../weather-api';

const WeatherForecastsTable = () => {
  const [lat, setLat] = useState<string | undefined>(undefined);
  const [long, setLong] = useState<string | undefined>(undefined);

  useEffect(() => {
    navigator.geolocation.getCurrentPosition((position) => {
      setLat(position.coords.latitude.toString());
      setLong(position.coords.longitude.toString());
    });
  }, []);

  const { forecast, error } = useWeatherForecast(lat, long);

  return (
    <LoadingWrapper error={error} data={forecast}>
      <Table horizontalSpacing="md" verticalSpacing="md" highlightOnHover>
        <thead>
          <tr>
            <th>Location</th>
            <th>Temp</th>
            <th>Summary</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          {forecast && (
            <tr key={forecast.name}>
              <td>{forecast.name}</td>
              <td>{forecast.main.temp}</td>
              <td>{forecast.weather[0].main}</td>
              <td>{forecast.weather[0].description}</td>
            </tr>
          )}
        </tbody>
      </Table>
    </LoadingWrapper>
  );
};

export default WeatherForecastsTable;
