import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: __ENV.VU_COUNT,
    duration: '1h'
};

const cities = [
    'New York',
    'London',
    'Dubai',
    'Hong Kong',
    'Bend',
    'St. Louis'
];

export default function () {
    const city = cities[Math.floor(Math.random() * cities.length)];
    const includeRadar = Math.floor(Math.random() * 2) == 1;
    const includeSatellite = Math.floor(Math.random() * 2) == 1;
    const gaussianResponse = http.get(`${__ENV.PROTOCOL}://${__ENV.WEATHER_SERVICE_HOSTNAME}/WeatherForecast?city=${city}&includeRadar=${includeRadar}&includeSatellite=${includeSatellite}`);
    check(gaussianResponse, { 'status was 200': (r) => r.status == 200 });
}
