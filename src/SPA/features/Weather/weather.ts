export type WeatherForecast = {
  name: string;
  weather: Weather[];
  main: Temperature;
};

export type Weather = {
  main: string;
  description: string;
};

export type Temperature = {
  temp: number;
};
