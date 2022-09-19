import useSWR from 'swr';
import { WeatherForecast } from './weather';

export const useWeatherForecast = (lat: string | undefined, long: string | undefined) => {
  const { data: forecast, error } = useSWR<WeatherForecast>(
    lat && long ? `/api/weatherforecast?lat=${lat}&lon=${long}` : null
  );

  return {
    forecast,
    error,
  };
};
