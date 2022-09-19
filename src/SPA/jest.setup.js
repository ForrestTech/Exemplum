import '@testing-library/jest-dom/extend-expect';
import 'whatwg-fetch';

const mockGeolocation = {
  getCurrentPosition: jest.fn().mockImplementationOnce((success) =>
    Promise.resolve(
      success({
        coords: {
          latitude: 51.1,
          longitude: 45.3,
        },
      })
    )
  ),
  watchPosition: jest.fn(),
};

global.navigator.geolocation = mockGeolocation;

// const JSDOM = require('jsdom').JSDOM;

// Object.defineProperty(global.self, 'crypto', {
//   value: {
//     getRandomValues: (arr) => crypto.randomBytes(arr.length),
//   },
// });
// global.crypto.subtle = {};
