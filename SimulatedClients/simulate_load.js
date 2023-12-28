import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 1000,
    duration: '1h'
};

export default function () {
    const gaussianResponse = http.get(`https://${__ENV.WEATHER_SERVICE_HOSTNAME}/WeatherForecast/GetWithGaussianLatency`);
    check(gaussianResponse, { 'status was 200': (r) => r.status == 200 });
    const uniformResponse = http.get(`https://${__ENV.WEATHER_SERVICE_HOSTNAME}/WeatherForecast/GetWithUniformLatency`);
    check(uniformResponse, { 'status was 200': (r) => r.status == 200 });
}
